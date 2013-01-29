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

                    if(player1Items.Any(x => x.myitem == item))
                    {
                        var item2 = player1Items.First(x => x.myitem == item);
                        item2.quantity += quantity;

                        if (item2.quantity > item.m_quantity)
                            item2.quantity = item.m_quantity;

                        player1.m_networkClient.Send(string.Format("EMKO+{0}|{1}", item.m_id, item2.quantity));
                        player2.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", item.m_id, item2.quantity, item.m_base.m_id, item.EffectsInfos()));
                    }
                    else
                    {
                        var newItem = new ExchangeItem(item);
                        newItem.quantity = quantity;

                        player1Items.Add(newItem);

                        player1.m_networkClient.Send(string.Format("EMKO+{0}|{1}", item.m_id, newItem.quantity));
                        player2.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", item.m_id, newItem.quantity, item.m_base.m_id, item.EffectsInfos()));
                    }
                }
                else
                {
                    Reset();

                    var Item = player1Items.First(x => x.myitem == item);
                    if (Item.quantity <= quantity)
                    {
                        player1.m_networkClient.Send(string.Format("EMKO-{0}", Item.myitem.m_id));
                        player2.m_networkClient.Send(string.Format("EmKO-{0}", Item.myitem.m_id));
                        player1Items.Remove(Item);
                    }
                    else
                    {
                        Item.quantity -= quantity;
                        player1.m_networkClient.Send(string.Format("EMKO+{0}|{1}", Item.myitem.m_id, Item.quantity));
                        player2.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", Item.myitem.m_id, Item.quantity, Item.myitem.m_base.m_id, Item.myitem.EffectsInfos()));
                    }
                }
            }
            else if (player2 == character)
            {
                if (add)
                {
                    Reset();

                    if (player2Items.Any(x => x.myitem == item))
                    {
                        var item2 = player2Items.First(x => x.myitem == item);
                        item2.quantity += quantity;

                        if (item2.quantity > item.m_quantity)
                            item2.quantity = item.m_quantity;

                        player2.m_networkClient.Send(string.Format("EMKO+{0}|{1}", item.m_id, item2.quantity));
                        player1.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", item.m_id, item2.quantity, item.m_base.m_id, item.EffectsInfos()));
                    }
                    else
                    {
                        var newItem = new ExchangeItem(item);
                        newItem.quantity = quantity;

                        player2Items.Add(newItem);

                        player2.m_networkClient.Send(string.Format("EMKO+{0}|{1}", item.m_id, newItem.quantity));
                        player1.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", item.m_id, newItem.quantity, item.m_base.m_id, item.EffectsInfos()));
                    }
                }
                else
                {
                    Reset();

                    var Item = player2Items.First(x => x.myitem == item);
                    if (Item.quantity <= quantity)
                    {
                        player2.m_networkClient.Send(string.Format("EMKO-{0}", Item.myitem.m_id));
                        player1.m_networkClient.Send(string.Format("EmKO-{0}", Item.myitem.m_id));
                        player2Items.Remove(Item);
                    }
                    else
                    {
                        Item.quantity -= quantity;
                        player2.m_networkClient.Send(string.Format("EMKO+{0}|{1}", Item.myitem.m_id, Item.quantity));
                        player1.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", Item.myitem.m_id, Item.quantity, Item.myitem.m_base.m_id, Item.myitem.EffectsInfos()));
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
                var charItem = item.GetNewItem();
                player2.m_inventary.AddItem(charItem, false);

                player1.m_inventary.DeleteItem(item.myitem.m_id, item.quantity);
            }

            foreach (var item in player2Items)
            {
                var charItem = item.GetNewItem();
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
