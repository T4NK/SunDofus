using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Realm.World.Conditions;

namespace DofusOrigin.Database.Models.Maps
{
    class TriggerModel
    {
        public int m_mapID { get; set; }
        public int m_cellID { get; set; }
        public int m_actionID { get; set; }

        public string m_args { get; set; }
        public string m_conditions { get; set; }

        public TriggerModel()
        {
            m_conditions = "";
            m_mapID = -1;
        }
    }
}
