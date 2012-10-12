using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.Clients
{
    class GiftModel
    {
        public int myId = -1, myItemID = -1;
        public string myTitle = "", myMessage = "";
        public Realm.Character.Items.CharacterItem myItem = null;
    }
}
