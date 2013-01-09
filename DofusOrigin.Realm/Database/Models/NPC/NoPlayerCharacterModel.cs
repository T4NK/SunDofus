using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.NPC
{
    class NoPlayerCharacterModel
    {
        public int m_id { get; set; }
        public int m_gfxid { get; set; }
        public int m_size { get; set; }
        public int m_sex { get; set; }

        public int m_color { get; set; }
        public int m_color2 { get; set; }
        public int m_color3 { get; set; }

        public int m_artWork { get; set; }
        public int m_bonus { get; set; }

        public string m_name { get; set; }
        public string m_items { get; set; }

        public List<int> m_sellingList { get; set; }
        public List<int> m_questions { get; set; }

        public NoPlayerCharacterModel()
        {
            m_sellingList = new List<int>();
            m_questions = new List<int>();
        }
    }
}
