using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Exchanges
{
    class ExchangeItem
    {
        public int u_ID;
        public int t_ID;
        public int quantity;

        public string effects;

        public Characters.Items.CharacterItem myitem;

        public ExchangeItem(Characters.Items.CharacterItem item)
        {
            u_ID = item.m_id;
            t_ID = item.m_base.m_id;

            effects = item.EffectsInfos();

            myitem = item;
        }
    }
}
