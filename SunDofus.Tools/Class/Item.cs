using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunDofus.Tools.Class
{
    class Item
    {
        public int m_id { get; set; }
        public int m_type { get; set; }
        public int m_level { get; set; }
        public int m_weight { get; set; }
        public int m_price { get; set; }
        public int m_gfxid { get; set; }

        public string m_name { get; set; }

        public bool isCreated { get; set; }

        public List<string> m_jets;
        public List<string> m_conditions;

        #region weapon

        public int m_costAP { get; set; }
        public int m_minRP { get; set; }
        public int m_maxRP { get; set; }
        public int m_critical { get; set; }
        public int m_fail { get; set; }

        public bool isTwohands { get; set; }
        public bool isInline { get; set; }

        #endregion

        public Item()
        {
            m_id = -1;

            m_jets = new List<string>();
            m_conditions = new List<string>();

            m_costAP = -1;
            m_minRP = -1;
            m_maxRP = -1;
            m_critical = -1;
            m_fail = -1;

            isTwohands = false;
            isInline = false;

            isCreated = false;
        }
    }
}
