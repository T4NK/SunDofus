using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class InventaryItems
    {
        public List<Item> ItemsList;
        public Character Client;

        public InventaryItems(Character Ch)
        {
            Client = Ch;
            ItemsList = new List<Item>();
        }

        public void AddItem(int ID)
        {
            AbstractItem BaseItem = Database.Data.ItemSql.ItemsList.First(x => x.ID == ID);
            Items.Item m_I = new Item(BaseItem);
            m_I.ParseJet();
            m_I.GeneratItem();

            foreach (Item i2 in ItemsList)
            {
                if (i2.BaseItem.ID == m_I.BaseItem.ID && i2.EffectsInfos() == m_I.EffectsInfos() && i2.Position == m_I.Position)
                {
                    i2.Quantity += m_I.Quantity;
                    Client.Pods += (m_I.BaseItem.Pods * m_I.Quantity);
                    Client.SendCharStats();
                    Client.SendPods();
                    Client.Client.Send("OQ" + i2.ID + "|" + i2.Quantity);
                    return;
                }
            }

            m_I.ID = ItemsManager.GetNewID();
            ItemsList.Add(m_I);

            Client.Pods += m_I.BaseItem.Pods;
            Client.SendCharStats();
            Client.SendPods();

            Client.Client.Send("OAKO" + m_I.ToString());
        }

        public void DeleteItem(int ID, int Quantity)
        {
            if (ItemsList.Any(x => x.ID == ID))
            {
                Item m_I = ItemsList.First(x => x.ID == ID);
                if (m_I.Quantity <= Quantity)
                {
                    ItemsList.Remove(m_I);
                    Client.Client.Send("OR" + m_I.ID);
                }
                else
                {
                    m_I.Quantity -= Quantity;
                    Client.Client.Send("OQ" + m_I.ID + "|" + m_I.Quantity);
                }
            }
        }
    }
}
