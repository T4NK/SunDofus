using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Map
{
    class Trigger
    {
        public int MapID, CellID, NewMapID, NewCellID = -1;

        public static bool isTrigger(int Map, int Cell)
        {
            foreach (Trigger m_T in Database.Data.TriggerSql.ListOfTriggers)
            {
                if (m_T.CellID == Cell && m_T.MapID == Map) return true;
            }
            return false;
        }
    }
}
