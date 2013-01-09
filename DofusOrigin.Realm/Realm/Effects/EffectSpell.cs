using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.Characters.Spells;
namespace realm.Realm.Effects
{
    class EffectSpell
    {
        public int m_id { get; set; }

        public int m_value { get; set; }
        public int m_value2 { get; set; }
        public int m_value3 { get; set; }
        public int m_tour { get; set; }
        public int m_chance { get; set; }

        public string m_effect { get; set; }

        public Target m_target { get; set; }

        public EffectSpell()
        {
            m_value = 0;
            m_value2 = 0;
            m_value3 = 0;

            m_tour = 0;
            m_chance = 0;

            m_effect = "1d5+0";
        }
    }
}
