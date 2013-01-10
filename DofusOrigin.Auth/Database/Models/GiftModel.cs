using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models
{
    class GiftModel
    {
        public int m_id { get; set; }
        public int m_target { get; set; }
        public int m_itemID { get; set; }

        public string m_title { get; set; }
        public string m_message { get; set; }

        public override string ToString()
        {
            return string.Format("{0}~{1}~{2}~{3}", m_id, m_title, m_message, m_itemID);
        }
    }
}
