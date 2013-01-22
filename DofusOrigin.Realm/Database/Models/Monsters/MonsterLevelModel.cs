using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.Monsters
{
    class MonsterLevelModel
    {
        public int m_id { get; set; }
        public int m_creature { get; set; }
        public int m_grade { get; set; }

        public int m_level { get; set; }
        public int m_pa { get; set; }
        public int m_pm { get; set; }
        public int m_life { get; set; }

        public int m_rNeutral { get; set; }
        public int m_rStrenght { get; set; }
        public int m_rIntel { get; set; }
        public int m_rLuck { get; set; }
        public int m_rAgility { get; set; }

        public int m_rPa { get; set; }
        public int m_rPm { get; set; }

        public int m_wisdom { get; set; }
        public int m_strenght { get; set; }
        public int m_intel { get; set; }
        public int m_luck { get; set; }
        public int m_agility { get; set; }

        public int m_exp { get; set; }
        public List<Realm.Characters.Spells.CharacterSpell> m_spells { get; set; }

        public MonsterLevelModel()
        {
            m_spells = new List<Realm.Characters.Spells.CharacterSpell>();
        }
    }
}
