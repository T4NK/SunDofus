using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Maps
{
    class Map
    {
        public List<Characters.Character> Characters;
        public List<Database.Models.Maps.TriggerModel> Triggers;
        public List<Characters.NPC.NPCMap> Npcs;
        public List<Monsters.MonstersGroup> MonstersGroups;
        public List<int> RushablesCells;

        public Database.Models.Maps.MapModel _model;

        public Database.Models.Maps.MapModel GetModel
        {
            get
            {
                return _model;
            }
        }

        public Map(Database.Models.Maps.MapModel map)
        {
            _model = map;

            RushablesCells = UncompressDatas();

            Characters = new List<Characters.Character>();
            Triggers = new List<Database.Models.Maps.TriggerModel>();
            Npcs = new List<Characters.NPC.NPCMap>();
            MonstersGroups = new List<Monsters.MonstersGroup>();

            if (GetModel.m_monsters.Count != 0 && RushablesCells.Count != 0)
                RefreshAllMonsters();
        }

        private void RefreshAllMonsters()
        {
            for (int i = 1; i <= GetModel.maxMonstersGroup; i++)
                AddMonstersGroup();
        }

        public void AddMonstersGroup()
        {
            if (MonstersGroups.Count >= GetModel.maxMonstersGroup)
                return;

            lock(MonstersGroups)
                MonstersGroups.Add(new Monsters.MonstersGroup(GetModel.m_monsters, this));
        }

        public void Send(string message)
        {
            lock (Characters)
            {
                foreach (var character in Characters)
                    character.m_networkClient.Send(message);
            }
        }

        public void AddPlayer(Characters.Character character)
        {
            Send(string.Format("GM|+{0}", character.PatternDisplayChar()));

            lock (Characters)
                Characters.Add(character);

            character.m_networkClient.Send(string.Format("GM{0}", CharactersPattern()));

            if(Npcs.Count > 0)
                character.m_networkClient.Send(string.Format("GM{0}", NPCsPattern()));

            if (MonstersGroups.Count > 0)
                character.m_networkClient.Send(string.Format("GM{0}", MonstersGroupsPattern()));
        }

        public void DelPlayer(Characters.Character character)
        {
            Send(string.Format("GM|-{0}", character.m_id));

            lock(Characters)
                Characters.Remove(character);
        }

        public int NextNpcID()
        {
            var i = -1;

            while (Npcs.Any(x => x.m_idOnMap == i) || MonstersGroups.Any(x => x.ID == i))
                i -= 1;

            return i;
        }

        private string CharactersPattern()
        {
            var packet = "";

            lock(Characters)
                Characters.ForEach(x => packet += string.Format("|+{0}", x.PatternDisplayChar()));

            return packet;
        }

        private string NPCsPattern()
        {
            var packet = "";

            lock(Npcs)
                Npcs.ForEach(x => packet += string.Format("|+{0}", x.PatternOnMap()));

            return packet;
        }

        private string MonstersGroupsPattern()
        {
            var packet = "";

            lock(MonstersGroups)
                MonstersGroups.ForEach(x => packet += string.Format("|+{0}", x.PatternOnMap()));

            return packet;
        }

        private List<int> UncompressDatas()
        {
            List<int> newList = new List<int>();
            var lengh = GetModel.m_mapData.Length;
            var cells = 0;
            var id = 0;

            while (cells < lengh)
            {
                if (isValidCell(GetModel.m_mapData.Substring(cells, 10)))
                    newList.Add(id);

                cells += 10;
                id++;
            }

            return newList;
        }

        private string hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

        private int hashCodes(char a)
        {
            return hash.IndexOf(a);
        }

        private bool isValidCell(string datas)
        {
            var lengh = datas.Length - 1;
            var table = new int[5000];

            while (lengh >= 0)
            {
                table[lengh] = hashCodes(datas[lengh]);
                lengh -= 1;
            }

            return ((table[2] & 56) >> 3) != 0;
        }
    }
}
