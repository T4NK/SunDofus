using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class CharUsableItem
    {
        public int BaseItemID = -1;
        public string Args = "";

        public void AttributeItem()
        {
            if(Database.Data.ItemSql.ItemsList.Any(x => x.ID == BaseItemID))
            {
                Database.Data.ItemSql.ItemsList.First(x => x.ID == BaseItemID).Usable = true;
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
                Effect.EffectsActions.ParseEffect(Client, int.Parse(Infos[0]), Infos[1]);
            }
        }
    }
}
