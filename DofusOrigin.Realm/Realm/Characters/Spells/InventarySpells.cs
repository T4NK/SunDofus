using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Characters.Spells
{
    class InventarySpells
    {
        public List<CharacterSpell> m_spells { get; set; }
        public Character m_client { get; set; }

        public InventarySpells(Character _client)
        {
            m_spells = new List<CharacterSpell>();
            m_client = _client;
        }

        public void ParseSpells(string _datas)
        {
            var spells = _datas.Split('|');

            foreach (var spell in spells)
            {
                var infos = spell.Split(',');
                AddSpells(int.Parse(infos[0]), int.Parse(infos[1]), int.Parse(infos[2]));
            }
        }

        public void LearnSpells()
        {
            foreach (var spell in Database.Cache.SpellsCache.m_spellsToLearn.Where(x => x.m_race == m_client.m_class && x.m_level <= m_client.m_level))
            {
                if (m_spells.Any(x => x.m_id == spell.m_spellID)) 
                    continue;

                AddSpells(spell.m_spellID, 1, spell.m_pos);
            }
        }

        public void AddSpells(int _id, int _level, int _pos)
        {
            if (m_spells.Any(x => x.m_id == _id)) return;

            if (_level < 1) _level = 1;
            if (_level > 6) _level = 6;

            if (_pos > 25) _pos = 25;
            if (_pos < 1) _pos = 25;

            m_spells.Add(new CharacterSpell(_id, _level, _pos));
        }

        public void SendAllSpells()
        {
            var packet = "";

            foreach (var spell in m_spells)
                packet += string.Format("{0}~{1}~{2};", spell.m_id, spell.m_level, Maps.Cells.GetDirChar(spell.m_position));

            m_client.m_networkClient.Send(string.Format("SL{0}", packet));
        }

        public string SaveSpells()
        {
            return string.Join("|", m_spells);
        }
    }
}
