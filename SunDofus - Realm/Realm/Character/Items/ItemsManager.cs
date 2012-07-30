using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class ItemsManager
    {
        public static int LastID = 0;

        public static int GetNewID()
        {
            return ++LastID;
        }
    }
}
