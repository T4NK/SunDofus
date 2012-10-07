using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm;
using realm.Realm.Character;

namespace realm.Database.Models.Items
{
    class ItemUsableModel
    {
        public int BaseItemID = -1;
        public string Args = "";

        public void AttributeItem()
        {
            if(Database.Cache.ItemsCache.ItemsList.Any(x => x.ID == BaseItemID))
            {
                Database.Cache.ItemsCache.ItemsList.First(x => x.ID == BaseItemID).Usable = true;
            }
        }

        public bool ConditionsAvaliable(Character Client)
        {
            return true;
        }

        public void ParseEffect(Character Client)
        {
            string[] Data = Args.Split('|');

            foreach (string AllData in Data)
            {
                string[] Infos = AllData.Split(';');
                Realm.Effect.EffectsActions.ParseEffect(Client, int.Parse(Infos[0]), Infos[1]);
            }
        }
    }
}
