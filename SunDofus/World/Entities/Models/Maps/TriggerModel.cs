using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus.World.Realm.World.Conditions;

namespace SunDofus.World.Entities.Models.Maps
{
    class TriggerModel
    {
        public int MapID;
        public int CellID;
        public int ActionID;

        public string Args;
        public string Conditions;

        public TriggerModel()
        {
            Conditions = "";
            MapID = -1;
        }
    }
}
