using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.World.Conditions;

namespace realm.Database.Models.Maps
{
    class TriggerModel
    {
        public int m_mapID { get; set; }
        public int m_cellID { get; set; }
        public int m_actionID { get; set; }

        public string m_args { get; set; }
        public string m_conditions { get; set; }

        public List<TriggerCondition> m_conds { get; set; }

        public TriggerModel()
        {
            m_conditions = "";
            m_mapID = -1;
        }

        public void ParseConditions()
        {
            m_conds = new List<Realm.World.Conditions.TriggerCondition>();
        }
    }
}
