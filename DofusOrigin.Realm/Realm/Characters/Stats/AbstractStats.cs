using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Characters.Stats
{
    class AbstractStats
    {
        public int m_bases { get; set; }
        public int m_items { get; set; }
        public int m_dons { get; set; }
        public int m_boosts { get; set; }

        public AbstractStats()
        {
            m_bases = 0;
            m_items = 0;
            m_dons = 0;
            m_boosts = 0;
        }

        public int Total()
        {
            return (m_bases + m_items + m_dons + m_boosts);
        }

        public int Base()
        {
            return m_bases;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", m_bases, m_items, m_dons, m_boosts);
        }
    }
}
