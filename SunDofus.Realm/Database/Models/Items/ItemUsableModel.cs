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
        public int myBaseItemID = -1;
        public string myArgs = "";

        public void AttributeItem()
        {
            if (Database.Cache.ItemsCache.ItemsList.Any(x => x.myID == myBaseItemID))
                Database.Cache.ItemsCache.ItemsList.First(x => x.myID == myBaseItemID).meUsable = true;
        }

        public bool ConditionsAvaliable(Character Client)
        {
            return true;
        }

        public void ParseEffect(Character Client)
        {
            string[] Data = myArgs.Split('|');

            foreach (var AllData in Data)
            {
                string[] Infos = AllData.Split(';');
                Realm.Effect.EffectsActions.ParseEffect(Client, int.Parse(Infos[0]), Infos[1]);
            }
        }
    }
}
