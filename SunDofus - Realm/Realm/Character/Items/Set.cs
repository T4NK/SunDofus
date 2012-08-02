using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class Set
    {
        public int ID = -1;
        public List<int> ItemsList = new List<int>();
        public Dictionary<int, List<Effect.EffectsItem>> BonusList = new Dictionary<int, List<Effect.EffectsItem>>();

        public Set(int _ID)
        {
            ID = _ID;
            BonusList = Database.Data.ItemSql.SetsList.First(x => x.ID == ID).BonusList;
            BonusList[1] = new List<Effect.EffectsItem>();
        }
    }
}
