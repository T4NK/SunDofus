using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Characters.Items
{
    class CharacterSet
    {
        public int m_id { get; set; }
        public List<int> myItemsList { get; set; }
        public Dictionary<int, List<Effects.EffectItem>> myBonusList { get; set; }

        public CharacterSet(int _id)
        {
            m_id = _id;

            myItemsList = new List<int>();
            myBonusList = Database.Cache.ItemsCache.m_setsList.First(x => x.m_id == m_id).m_bonusList;
            myBonusList[1] = new List<Effects.EffectItem>();
        }
    }
}
