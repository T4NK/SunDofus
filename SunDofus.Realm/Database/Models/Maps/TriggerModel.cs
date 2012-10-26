using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.Maps
{
    class TriggerModel
    {
        public int myMapID = -1, myCellID = -1, myActionID;
        public string myArgs = "", myConditions = "";

        public void ParseConditions()
        {
            myConds = new List<Realm.World.Conditions.TriggerCondition>();
        }

        public List<realm.Realm.World.Conditions.TriggerCondition> myConds;
    }
}
