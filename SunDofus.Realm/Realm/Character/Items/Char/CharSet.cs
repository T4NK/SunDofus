using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class CharSet
    {
        public int ID = -1;
        public List<int> ItemsList = new List<int>();
        public Dictionary<int, List<Effect.EffectsItems>> BonusList = new Dictionary<int, List<Effect.EffectsItems>>();

        public CharSet(int _ID)
        {
            ID = _ID;
            BonusList = Database.Cache.ItemsCache.SetsList.First(x => x.ID == ID).BonusList;
            BonusList[1] = new List<Effect.EffectsItems>();
        }
    }
}
