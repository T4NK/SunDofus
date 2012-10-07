using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.Clients
{
    class GiftModel
    {
        public int id = -1, itemID = -1;
        public string title = "", message = "";
        public Realm.Character.Items.CharacterItem item = null;
    }
}
