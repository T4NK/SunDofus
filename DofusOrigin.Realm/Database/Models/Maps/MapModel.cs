using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.Maps
{
    class MapModel
    {
        public int m_id { get; set; }
        public int m_date { get; set; }
        public int m_width { get; set; }
        public int m_height { get; set; }
        public int m_capabilities { get; set; }
        public int m_PosX { get; set; }
        public int m_PosY { get; set; }

        public string m_mapData { get; set; }
        public string m_key { get; set; }
        public string m_mappos { get; set; }

        public int maxMonstersGroup { get; set; }

        public Dictionary<int, List<int>> m_monsters { get; set; }

        public MapModel()
        {
            m_mapData = "";
            m_key = "";
            m_mappos = "";

            m_monsters = new Dictionary<int, List<int>>();
        }

        public void ParsePos()
        {
            var datas = m_mappos.Split(',');
            try
            {
                m_PosX = int.Parse(datas[0]);
                m_PosY = int.Parse(datas[1]);
            }
            catch { }
        }
    }
}
