using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.World
{
    class ConditionsHandler
    {
        public static bool HasCondition(Network.Realms.RealmClient _client, List<Conditions.ItemCondition> _conds)
        {
            return true;
        }

        public static bool HasCondition(Network.Realms.RealmClient _client, List<Conditions.TriggerCondition> _conds)
        {
            return true;
        }
    }
}
