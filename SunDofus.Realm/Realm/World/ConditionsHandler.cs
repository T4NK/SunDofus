using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.World
{
    class ConditionsHandler
    {
        public static bool HasCondition(Network.Realm.RealmClient Client, List<Conditions.ItemCondition> myCond)
        {
            return true;
        }

        public static bool HasCondition(Network.Realm.RealmClient Client, List<Conditions.TriggerCondition> myCond)
        {
            return true;
        }
    }
}
