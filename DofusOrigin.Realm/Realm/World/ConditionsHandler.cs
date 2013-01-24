using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.World
{
    class ConditionsHandler
    {
        public static bool HasCondition(Network.Realm.RealmClient _client, List<Conditions.ItemCondition> _conds)
        {
            return true;
        }

        public static bool HasCondition(Network.Realm.RealmClient _client, List<Conditions.TriggerCondition> _conds)
        {
            return true;
        }
    }
}
