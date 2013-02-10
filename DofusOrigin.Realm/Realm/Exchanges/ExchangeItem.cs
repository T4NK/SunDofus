using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Realm.Characters.Items;

namespace DofusOrigin.Realm.Exchanges
{
    class ExchangeItem
    {
        public CharacterItem Item;

        private int _quantity;

        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
            }
        }

        public ExchangeItem(CharacterItem item)
        {
            Item = item;
        }

        public CharacterItem GetNewItem()
        {
            var item = new CharacterItem(Item.m_base);

            item.m_effectsList.Clear();
            Item.m_effectsList.ForEach(x => item.m_effectsList.Add(new Effects.EffectItem(x)));

            item.m_id = ItemsHandler.GetNewID();
            item.m_position = Item.m_position;

            item.m_quantity = Quantity;

            return item;
        }
    }
}
