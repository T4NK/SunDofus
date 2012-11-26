using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.Spells
{
    class SpellToLearnModel
    {
        public int m_race  { get; set; }
        public int m_level  { get; set; }
        public int m_spellID  { get; set; }
        public int m_pos { get; set; }

        public SpellToLearnModel()
        {
            m_race = 0;
            m_level = 0;
            m_spellID = 0;
            m_pos = 0;
        }
    }
}
