using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Realm.Characters.Items;

namespace DofusOrigin.Realm.Exchanges
{
    class ExchangeItem
    {
        public int quantity;

        public Characters.Items.CharacterItem myitem;

        public ExchangeItem(Characters.Items.CharacterItem item)
        {
            myitem = item;
        }

        public CharacterItem GetNewItem()
        {
            var newItem = new CharacterItem(myitem.m_base);
            newItem.m_effectsList.Clear();

            myitem.m_effectsList.ForEach(x => newItem.m_effectsList.Add(new Effects.EffectItem(x)));

            newItem.m_id = ItemsHandler.GetNewID();
            newItem.m_position = myitem.m_position;

            newItem.m_quantity = quantity;

            return newItem;
        }
    }
}
