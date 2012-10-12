using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Spells
{
    class InventarySpells
    {
        public List<CharacterSpell> mySpells;
        public Character myClient;

        public InventarySpells(Character client)
        {
            mySpells = new List<CharacterSpell>();
            myClient = client;
        }

        public void ParseSpells(string Data)
        {
            string[] Spells = Data.Split('|');

            foreach (var Spell in Spells)
            {
                string[] Infos = Spell.Split(',');
                AddSpells(int.Parse(Infos[0]), int.Parse(Infos[1]), int.Parse(Infos[2]));
            }
        }

        public void LearnSpells()
        {
            foreach (var mySpell in Database.Cache.SpellsCache.SpellsToLearn.Where(x => x.myRace == myClient.Class && x.myLevel <= myClient.Level))
            {
                if (mySpells.Any(x => x.myId == mySpell.mySpellID)) continue;
                AddSpells(mySpell.mySpellID, 1, mySpell.myPos);
            }
        }

        public void AddSpells(int id, int level, int pos)
        {
            if (mySpells.Any(x => x.myId == id)) return;

            if (level < 1) level = 1;
            if (level > 6) level = 6;

            if (pos > 25) pos = 25;
            if (pos < 1) pos = 25;

            mySpells.Add(new CharacterSpell(id, level, pos));
        }

        public void SendAllSpells()
        {
            var Packet = "";

            foreach (var mySpell in mySpells)
                Packet += string.Format("{0}~{1}~{2};", mySpell.myId, mySpell.myLevel, Map.Cells.GetDirChar(mySpell.myPosition));

            myClient.Client.Send(string.Format("SL{0}", Packet));
        }

        public string SaveSpells()
        {
            var Data = "";

            foreach (var mySpell in mySpells)
                Data += string.Format("{0},{1},{2}|", mySpell.myId, mySpell.myLevel, mySpell.myPosition);

            if (Data == "")
                return Data;
            else
                return Data.Substring(0, Data.Length - 1);
        }
    }
}
