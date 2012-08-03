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

        public static bool PositionAvaliable(int ItemType, bool Usable, int Position)
        {

            return true;

        }

        public static bool ConditionsAvaliable(AbstractItem m_I, Character m_C)
        {

            return true;

        }
    }
}
