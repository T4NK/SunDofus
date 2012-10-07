using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Spells
{
    class InventarySpells
    {
        public List<CharSpell> mySpells;
        public Character Client;

        public InventarySpells(Character client)
        {
            mySpells = new List<CharSpell>();
            Client = client;
        }

        public void ParseSpells(string Data)
        {
            string[] Spells = Data.Split('|');

            foreach (string Spell in Spells)
            {
                string[] Infos = Spell.Split(',');
                AddSpells(int.Parse(Infos[0]), int.Parse(Infos[1]), int.Parse(Infos[2]));
            }
        }

        public void LearnSpells()
        {
            foreach (SpellToLearn m_S in Database.Cache.SpellsCache.SpellsToLearn.Where(x => x.Race == Client.Class && x.Level <= Client.Level))
            {
                if (mySpells.Any(x => x.id == m_S.SpellID)) continue;
                AddSpells(m_S.SpellID, 1, m_S.Pos);
            }
        }

        public void AddSpells(int id, int level, int pos)
        {
            if (mySpells.Any(x => x.id == id)) return;

            if (level < 1) level = 1;
            if (level > 6) level = 6;

            if (pos > 25) pos = 25;
            if (pos < 1) pos = 25;

            mySpells.Add(new CharSpell(id, level, pos));
        }

        public void SendAllSpells()
        {
            string Packet = "";

            foreach (CharSpell m_S in mySpells)
                Packet += m_S.id + "~" + m_S.level + "~" + Map.Cells.GetDirChar(m_S.position) + ";";

            Client.Client.Send("SL" + Packet);
        }

        public string SaveSpells()
        {
            string Data = "";

            foreach (CharSpell m_S in mySpells)
                Data += m_S.id + "," + m_S.level + "," + m_S.position + "|";

            if (Data == "")
                return Data;
            else
                return Data.Substring(0, Data.Length - 1);
        }
    }
}
