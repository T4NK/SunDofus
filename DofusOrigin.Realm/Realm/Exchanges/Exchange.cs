using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Exchanges
{
    class Exchange
    {
        public Characters.Character player1;
        private long player1Gold;
        private List<ExchangeItem> player1Items = new List<ExchangeItem>();

        public Characters.Character player2;
        private long player2Gold;
        private List<ExchangeItem> player2Items = new List<ExchangeItem>();

        public Exchange(Characters.Character new1, Characters.Character new2)
        {
            player1 = new1;
            player2 = new2;
        }

        public void Reset()
        {
            player1.m_state.onExchangeAccepted = false;
            player2.m_state.onExchangeAccepted = false;

            player1.m_networkClient.Send(string.Format("EK0{0}", player1.m_id));
            player1.m_networkClient.Send(string.Format("EK0{0}", player2.m_id));
            player2.m_networkClient.Send(string.Format("EK0{0}", player2.m_id));
            player2.m_networkClient.Send(string.Format("EK0{0}", player1.m_id));
        }

        public void MoveGold(Characters.Character character, long kamas)
        {
            if (player1 == character)
            {
                Reset();
                player1Gold = kamas;
                player1.m_networkClient.Send(string.Format("EMKG{0}", kamas));
                player2.m_networkClient.Send(string.Format("EmKG{0}", kamas));
            }
            else if (player2 == character)
            {
                Reset();
                player2Gold = kamas;
                player2.m_networkClient.Send(string.Format("EMKG{0}", kamas));
                player1.m_networkClient.Send(string.Format("EmKG{0}", kamas));
            }
        }

        public void MoveItem(Characters.Character character, Characters.Items.CharacterItem item, int quantity, bool add)
        {
            if (player1 == character)
            {
                if (add)
                {
                    Reset();

                    var newItem = new ExchangeItem(item);
                    newItem.quantity = quantity;
                    player1Items.Add(newItem);

                    player1.m_networkClient.Send(string.Format("EMKO+{0}|{1}", newItem.u_ID, newItem.quantity));
                    player2.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", newItem.u_ID, newItem.quantity, newItem.t_ID, newItem.effects));
                }
                else
                {
                    Reset();

                    var Item = player1Items.First(x => x.u_ID == item.m_id);
                    if (Item.quantity <= quantity)
                    {
                        player1.m_networkClient.Send(string.Format("EMKO-{0}", Item.u_ID));
                        player2.m_networkClient.Send(string.Format("EmKO-{0}", Item.u_ID));
                        player1Items.Remove(Item);
                    }
                    else
                    {
                        Item.quantity -= quantity;
                        player1.m_networkClient.Send(string.Format("EMKO+{0}|{1}", Item.u_ID, Item.quantity));
                        player2.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", Item.u_ID, Item.quantity, Item.t_ID, Item.effects));
                    }
                }
            }
            else if (player2 == character)
            {
                if (add)
                {
                    Reset();

                    var newItem = new ExchangeItem(item);
                    newItem.quantity = quantity;
                    player2Items.Add(newItem);

                    player2.m_networkClient.Send(string.Format("EMKO+{0}|{1}", newItem.u_ID, newItem.quantity));
                    player1.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", newItem.u_ID, newItem.quantity, newItem.t_ID, newItem.effects));
                }
                else
                {
                    Reset();

                    var Item = player2Items.First(x => x.u_ID == item.m_id);
                    if (Item.quantity <= quantity)
                    {
                        player2.m_networkClient.Send(string.Format("EMKO-{0}", Item.u_ID));
                        player1.m_networkClient.Send(string.Format("EmKO-{0}", Item.u_ID));
                        player2Items.Remove(Item);
                    }
                    else
                    {
                        Item.quantity -= quantity;
                        player2.m_networkClient.Send(string.Format("EMKO+{0}|{1}", Item.u_ID, Item.quantity));
                        player1.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", Item.u_ID, Item.quantity, Item.t_ID, Item.effects));
                    }
                }
            }
        }

        public void ValideExchange()
        {
            player1.m_kamas = player1.m_kamas - player1Gold + player2Gold;
            player2.m_kamas = player2.m_kamas - player2Gold + player1Gold;

            foreach (var item in player1Items)
            {
                var charItem = item.myitem.Copy();
                charItem.m_quantity = item.quantity;
                player2.m_inventary.AddItem(charItem, false);

                player1.m_inventary.DeleteItem(item.myitem.m_id, item.quantity);
            }

            foreach (var item in player2Items)
            {
                var charItem = item.myitem.Copy();
                charItem.m_quantity = item.quantity;
                player1.m_inventary.AddItem(charItem, false);

                player2.m_inventary.DeleteItem(item.myitem.m_id, item.quantity);
            }

            player1.SendChararacterStats();
            player2.SendChararacterStats();

            player1.m_networkClient.Send("EVa");
            player2.m_networkClient.Send("EVa");

            ExchangesManager.LeaveExchange(player1, false);
        }
    }
}
