using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus.Realm.Characters.Items;

namespace SunDofus.Database.Models.Clients
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
