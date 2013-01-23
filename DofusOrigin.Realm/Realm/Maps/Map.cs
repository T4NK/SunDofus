using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Maps
{
    class Map
    {
        public List<Characters.Character> m_characters { get; set; }
        public List<Database.Models.Maps.TriggerModel> m_triggers { get; set; }
        public List<Characters.NPC.NPCMap> m_npcs { get; set; }
        public List<Monsters.MonstersGroup> m_groups { get; set; }

        public Database.Models.Maps.MapModel m_map { get; set; }

        public Map(Database.Models.Maps.MapModel _map)
        {
            m_map = _map;

            m_characters = new List<Characters.Character>();
            m_triggers = new List<Database.Models.Maps.TriggerModel>();
            m_npcs = new List<Characters.NPC.NPCMap>();
            m_groups = new List<Monsters.MonstersGroup>();
        }

        public void AddMonstersGroup()
        {
            if (m_groups.Count >= m_map.maxMonstersGroup)
                return;

            m_groups.Add(new Monsters.MonstersGroup(m_map.m_monsters, this));
        }

        public void Send(string _message)
        {
            foreach (var character in m_characters)
            {
                character.m_networkClient.Send(_message);
            }
        }

        public void AddPlayer(Characters.Character _character)
        {
            Send(string.Format("GM|+{0}", _character.PatternDisplayChar()));
            m_characters.Add(_character);

            _character.m_networkClient.Send(string.Format("GM{0}", CharactersPattern()));

            if(m_npcs.Count > 0)
                _character.m_networkClient.Send(string.Format("GM{0}", NPCsPattern()));

            if (m_groups.Count > 0)
                _character.m_networkClient.Send(string.Format("GM{0}", MonstersGroupsPattern()));
        }

        public void DelPlayer(Characters.Character _character)
        {
            Send(string.Format("GM|-{0}", _character.m_id));
            m_characters.Remove(_character);
        }

        public int NextNpcID()
        {
            var i = -1;

            while (m_npcs.Any(x => x.m_idOnMap == i) || m_groups.Any(x => x.m_id == i))
                i -= 1;

            return i;
        }

        private string CharactersPattern()
        {
            var packet = "";

            m_characters.ForEach(x => packet += string.Format("|+{0}", x.PatternDisplayChar()));

            return packet;
        }

        private string NPCsPattern()
        {
            var packet = "";

            m_npcs.ForEach(x => packet += string.Format("|+{0}", x.PatternOnMap()));

            return packet;
        }

        private string MonstersGroupsPattern()
        {
            var packet = "";

            m_groups.ForEach(x => packet += string.Format("|+{0}", x.PatternOnMap()));

            return packet;
        }
    }
}
