using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Realm.Characters.Items;

namespace DofusOrigin.Database.Models.Clients
{
    class GiftModel
    {
        public int ID;
        public int ItemID;

        public string Title;
        public string Message;
        public string Image;

        public CharacterItem Item;
    }
}
