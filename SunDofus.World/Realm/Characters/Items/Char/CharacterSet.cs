using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.Realm.Characters.Items
{
    class CharacterSet
    {
        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
        }
        public List<int> ItemsList;
        public Dictionary<int, List<Effects.EffectItem>> BonusList;

        public CharacterSet(int id)
        {
            _ID = id;

            ItemsList = new List<int>();
            BonusList = Database.Cache.ItemsCache.SetsList.First(x => x.ID == ID).BonusList;
            BonusList[1] = new List<Effects.EffectItem>();
        }
    }
}
