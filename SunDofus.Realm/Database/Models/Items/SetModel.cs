using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm;

namespace realm.Database.Models.Items
{
    class SetModel
    {
        public List<int> ItemsList = new List<int>();
        public int ID = -1;
        public Dictionary<int, List<Realm.Effect.EffectsItems>> BonusList = new Dictionary<int, List<Realm.Effect.EffectsItems>>();

        public void ParseItems(string Data)
        {
            if (Data == "") return;
            foreach (string Infos in Data.Split(','))
            {
                int IID = int.Parse(Infos.Replace(" ", ""));
                if (Database.Cache.ItemsCache.ItemsList.Any(x => x.ID == IID))
                {
                    Database.Models.Items.ItemModel Item = Database.Cache.ItemsCache.ItemsList.First(x => x.ID == IID);
                    Item.Set = this.ID;
                }
                ItemsList.Add(ID);
            }
        }

        public void ParseBonus(string Data)
        {
            int Nb = 1;
            if (Data == "") return;
            foreach (string Infos in Data.Split(';'))
            {
                if (Infos == "") continue;
                BonusList.Add(++Nb, new List<Realm.Effect.EffectsItems>());
                foreach (string AllData in Infos.Split(','))
                {
                    if (AllData == "") continue;
                    Realm.Effect.EffectsItems m_B = new Realm.Effect.EffectsItems();
                    m_B.ID = int.Parse(AllData.Split(':')[0]);
                    m_B.Value = int.Parse(AllData.Split(':')[1]);
                    BonusList[Nb].Add(m_B);
                }
            }
        }
    }
}
