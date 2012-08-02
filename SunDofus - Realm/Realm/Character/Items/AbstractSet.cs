using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class AbstractSet
    {
        public List<int> ItemsList = new List<int>();
        public int ID = -1;
        public Dictionary<int, List<Effect.EffectsItem>> BonusList = new Dictionary<int, List<Effect.EffectsItem>>();

        public void ParseItems(string Data)
        {
            if (Data == "") return;
            foreach (string Infos in Data.Split(','))
            {
                int IID = int.Parse(Infos.Replace(" ", ""));
                if (Database.Data.ItemSql.ItemsList.Any(x => x.ID == IID))
                {
                    Realm.Character.Items.AbstractItem Item = Database.Data.ItemSql.ItemsList.First(x => x.ID == IID);
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
                BonusList.Add(++Nb, new List<Effect.EffectsItem>());
                foreach (string AllData in Infos.Split(','))
                {
                    if (AllData == "") continue;
                    Effect.EffectsItem m_B = new Effect.EffectsItem();
                    m_B.ID = int.Parse(AllData.Split(':')[0]);
                    m_B.Value = int.Parse(AllData.Split(':')[1]);
                    BonusList[Nb].Add(m_B);
                }
            }
        }
    }
}
