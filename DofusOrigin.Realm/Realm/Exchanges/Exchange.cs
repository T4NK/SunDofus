using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Exchanges
{
    class Exchange
    {
        public Characters.Character player1;

        private long _player1Gold;
        private List<ExchangeItem> _player1Items = new List<ExchangeItem>();

        public Characters.Character player2;

        private long _player2Gold;
        private List<ExchangeItem> _player2Items = new List<ExchangeItem>();

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
                _player1Gold = kamas;

                player1.m_networkClient.Send(string.Format("EMKG{0}", kamas));
                player2.m_networkClient.Send(string.Format("EmKG{0}", kamas));
            }
            else if (player2 == character)
            {
                Reset();
                _player2Gold = kamas;

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

                    lock (_player1Items)
                    {
                        if (_player1Items.Any(x => x.Item == item))
                        {
                            var item2 = _player1Items.First(x => x.Item == item);
                            item2.Quantity += quantity;

                            if (item2.Quantity > item.m_quantity)
                                item2.Quantity = item.m_quantity;

                            player1.m_networkClient.Send(string.Format("EMKO+{0}|{1}", item.m_id, item2.Quantity));
                            player2.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", item.m_id, item2.Quantity, item.m_base.m_id, item.EffectsInfos()));
                        }
                        else
                        {
                            var newItem = new ExchangeItem(item);
                            newItem.Quantity = quantity;

                            _player1Items.Add(newItem);

                            player1.m_networkClient.Send(string.Format("EMKO+{0}|{1}", item.m_id, newItem.Quantity));
                            player2.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", item.m_id, newItem.Quantity, item.m_base.m_id, item.EffectsInfos()));
                        }
                    }
                }
                else
                {
                    Reset();

                    lock (_player1Items)
                    {
                        var Item = _player1Items.First(x => x.Item == item);
                        if (Item.Quantity <= quantity)
                        {
                            player1.m_networkClient.Send(string.Format("EMKO-{0}", Item.Item.m_id));
                            player2.m_networkClient.Send(string.Format("EmKO-{0}", Item.Item.m_id));
                            _player1Items.Remove(Item);
                        }
                        else
                        {
                            Item.Quantity -= quantity;
                            player1.m_networkClient.Send(string.Format("EMKO+{0}|{1}", Item.Item.m_id, Item.Quantity));
                            player2.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", Item.Item.m_id, Item.Quantity, Item.Item.m_base.m_id, Item.Item.EffectsInfos()));
                        }
                    }
                }
            }
            else if (player2 == character)
            {
                if (add)
                {
                    Reset();

                    lock (_player2Items)
                    {
                        if (_player2Items.Any(x => x.Item == item))
                        {
                            var item2 = _player2Items.First(x => x.Item == item);
                            item2.Quantity += quantity;

                            if (item2.Quantity > item.m_quantity)
                                item2.Quantity = item.m_quantity;

                            player2.m_networkClient.Send(string.Format("EMKO+{0}|{1}", item.m_id, item2.Quantity));
                            player1.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", item.m_id, item2.Quantity, item.m_base.m_id, item.EffectsInfos()));
                        }
                        else
                        {
                            var newItem = new ExchangeItem(item);
                            newItem.Quantity = quantity;

                            _player2Items.Add(newItem);

                            player2.m_networkClient.Send(string.Format("EMKO+{0}|{1}", item.m_id, newItem.Quantity));
                            player1.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", item.m_id, newItem.Quantity, item.m_base.m_id, item.EffectsInfos()));
                        }
                    }
                }
                else
                {
                    Reset();

                    lock (_player2Items)
                    {
                        var Item = _player2Items.First(x => x.Item == item);
                        if (Item.Quantity <= quantity)
                        {
                            player2.m_networkClient.Send(string.Format("EMKO-{0}", Item.Item.m_id));
                            player1.m_networkClient.Send(string.Format("EmKO-{0}", Item.Item.m_id));
                            _player2Items.Remove(Item);
                        }
                        else
                        {
                            Item.Quantity -= quantity;
                            player2.m_networkClient.Send(string.Format("EMKO+{0}|{1}", Item.Item.m_id, Item.Quantity));
                            player1.m_networkClient.Send(string.Format("EmKO+{0}|{1}|{2}|{3}", Item.Item.m_id, Item.Quantity, Item.Item.m_base.m_id, Item.Item.EffectsInfos()));
                        }
                    }
                }
            }
        }

        public void ValideExchange()
        {
            player1.m_kamas = player1.m_kamas - _player1Gold + _player2Gold;
            player2.m_kamas = player2.m_kamas - _player2Gold + _player1Gold;

            lock (_player1Items)
            {
                foreach (var item in _player1Items)
                {
                    var charItem = item.GetNewItem();
                    player2.m_inventary.AddItem(charItem, false);

                    player1.m_inventary.DeleteItem(item.Item.m_id, item.Quantity);
                }
            }

            lock (_player2Items)
            {
                foreach (var item in _player2Items)
                {
                    var charItem = item.GetNewItem();
                    player1.m_inventary.AddItem(charItem, false);

                    player2.m_inventary.DeleteItem(item.Item.m_id, item.Quantity);
                }
            }

            player1.SendChararacterStats();
            player2.SendChararacterStats();

            player1.m_networkClient.Send("EVa");
            player2.m_networkClient.Send("EVa");

            ExchangesManager.LeaveExchange(player1, false);
        }
    }
}
