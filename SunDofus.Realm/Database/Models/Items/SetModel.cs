using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm;

namespace realm.Database.Models.Items
{
    class SetModel
    {
        public List<int> myItemsList = new List<int>();
        public int myID = -1;
        public Dictionary<int, List<Realm.Effect.EffectsItems>> myBonusList = new Dictionary<int, List<Realm.Effect.EffectsItems>>();

        public void ParseItems(string Data)
        {
            if (Data == "") return;
            foreach (var Infos in Data.Split(','))
            {
                var ID = int.Parse(Infos.Replace(" ", ""));

                if (Database.Cache.ItemsCache.ItemsList.Any(x => x.myID == ID))
                    Database.Cache.ItemsCache.ItemsList.First(x => x.myID == ID).mySet = this.myID;

                myItemsList.Add(myID);
            }
        }

        public void ParseBonus(string Data)
        {
            var myNb = 1;
            if (Data == "") return;

            foreach (var Infos in Data.Split(';'))
            {
                if (Infos == "") continue;
                myBonusList.Add(++myNb, new List<Realm.Effect.EffectsItems>());
                foreach (var AllData in Infos.Split(','))
                {
                    if (AllData == "") continue;
                    var myBonus = new Realm.Effect.EffectsItems();
                    myBonus.ID = int.Parse(AllData.Split(':')[0]);
                    myBonus.Value = int.Parse(AllData.Split(':')[1]);
                    myBonusList[myNb].Add(myBonus);
                }
            }
        }
    }
}
