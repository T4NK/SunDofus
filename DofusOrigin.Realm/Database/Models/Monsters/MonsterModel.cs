using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.Monsters
{
    class MonsterModel
    {
        public int m_id { get; set; }
        public int m_gfx { get; set; }
        public int m_align { get; set; }
        public int m_color { get; set; }
        public int m_color2 { get; set; }
        public int m_color3 { get; set; }
        public int m_ia { get; set; }

        public int max_kamas { get; set; }
        public int min_kamas { get; set; }

        public string m_name { get; set; }

        public List<MonsterLevelModel> m_levels { get; set; }
        public List<MonsterItem> m_items { get; set; }

        public MonsterModel()
        {
            m_levels = new List<MonsterLevelModel>();
            m_items = new List<MonsterItem>();
        }

        public class MonsterItem
        {
            public int m_id { get; set; }
            public double m_chance { get; set; }
            public int m_max { get; set; }

            public MonsterItem(int newID, double newChance, int newMax)
            {
                m_id = newID;
                m_chance = newChance;
                m_max = newMax;
            }
        }
    }
}
