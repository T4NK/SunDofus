using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Characters.Items
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
            BonusList = Database.Cache.ItemsCache.SetsList.First(x => x.m_id == ID).m_bonusList;
            BonusList[1] = new List<Effects.EffectItem>();
        }
    }
}
