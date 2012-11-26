using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Characters.Spells
{
    class CharacterSpell
    {
        public int m_id { get; set; }
        public int m_level { get; set; }
        public int m_position { get; set; }

        public CharacterSpell(int _id, int _lvl, int _pos)
        {
            m_id = _id;
            m_level = _lvl;
            m_position = _pos;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", m_id, m_level, m_position);
        }
    }
}
