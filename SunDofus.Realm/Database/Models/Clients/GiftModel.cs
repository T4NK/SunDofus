using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.Characters.Items;

namespace realm.Database.Models.Clients
{
    class GiftModel
    {
        public int m_id { get; set; }
        public int m_itemID { get; set; }
        public string m_title { get; set; }
        public string m_message { get; set; }
        public CharacterItem m_item { get; set; }
    }
}
