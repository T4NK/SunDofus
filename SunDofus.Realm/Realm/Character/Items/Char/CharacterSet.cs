using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class CharacterSet
    {
        public int myID = -1;
        public List<int> myItemsList = new List<int>();
        public Dictionary<int, List<Effect.EffectsItems>> myBonusList = new Dictionary<int, List<Effect.EffectsItems>>();

        public CharacterSet(int _ID)
        {
            myID = _ID;
            myBonusList = Database.Cache.ItemsCache.SetsList.First(x => x.myID == myID).myBonusList;
            myBonusList[1] = new List<Effect.EffectsItems>();
        }
    }
}
