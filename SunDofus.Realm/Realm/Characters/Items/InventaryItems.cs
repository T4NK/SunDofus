using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Characters.Items
{
    class InventaryItems
    {
        public Character m_client { get; set; }
        public List<CharacterItem>  m_itemsList { get; set; }
        public Dictionary<int, CharacterSet> m_setsList { get; set; }

        public InventaryItems(Character _character)
        {
            m_client = _character;

            m_itemsList = new List<CharacterItem>();
            m_setsList = new Dictionary<int,CharacterSet>();
        }

        public void AddItem(int _id, bool _offline, int _jet = 4)
        {
            if (_offline == true)
            {
                if (!Database.Cache.ItemsCache.m_itemsList.Any(x => x.m_id == _id)) 
                    return;

                var baseItem = Database.Cache.ItemsCache.m_itemsList.First(x => x.m_id == _id);
                var item = new CharacterItem(baseItem);

                item.ParseJet();
                item.GeneratItem(_jet);

                if (m_itemsList.Any(x => x.EffectsInfos() == item.EffectsInfos() && x.m_id == item.m_id && x.m_position == item.m_position))
                {
                    var item2 = m_itemsList.First(x => x.EffectsInfos() == item.EffectsInfos() && x.m_id == item.m_id && x.m_position == item.m_position);

                    item2.m_quantity += item.m_quantity;
                    m_client.m_pods += (item.m_base.m_pods * item.m_quantity);

                    return;
                }

                item.m_id = ItemsHandler.GetNewID();

                m_itemsList.Add(item);
                m_client.m_pods += item.m_base.m_pods;
            }
            else if (_offline == false)
            {
                if (!Database.Cache.ItemsCache.m_itemsList.Any(x => x.m_id == _id))
                    return;

                var baseItem = Database.Cache.ItemsCache.m_itemsList.First(x => x.m_id == _id);
                var item = new CharacterItem(baseItem);

                item.ParseJet();
                item.GeneratItem(_jet);

                if (m_itemsList.Any(x => x.EffectsInfos() == item.EffectsInfos() && x.m_id == item.m_id && x.m_position == item.m_position))
                {
                    var item2 = m_itemsList.First(x => x.EffectsInfos() == item.EffectsInfos() && x.m_id == item.m_id && x.m_position == item.m_position);

                    item2.m_quantity += item.m_quantity;
                    m_client.m_pods += (item.m_base.m_pods * item.m_quantity);

                    RefreshBonus();
                    m_client.m_networkClient.Send(string.Format("OQ{0}|{1}", item2.m_id, item2.m_quantity));

                    return;
                }

                item.m_id = ItemsHandler.GetNewID();
                m_itemsList.Add(item);

                m_client.m_pods += item.m_base.m_pods;
                RefreshBonus();

                m_client.m_networkClient.Send(string.Format("OAKO{0}", item.ToString()));
            }
        }

        public void AddItem(CharacterItem _item, bool _offline)
        {
            if(_offline == true)
            {
                if (m_itemsList.Any(x => x.EffectsInfos() == _item.EffectsInfos() && x.m_id == _item.m_id && x.m_position == _item.m_position))
                {
                    var item2 = m_itemsList.First(x => x.EffectsInfos() == _item.EffectsInfos() && x.m_id == _item.m_id && x.m_position == _item.m_position);

                    item2.m_quantity += _item.m_quantity;
                    m_client.m_pods += (_item.m_base.m_pods * _item.m_quantity);

                    return;
                }

                _item.m_id = ItemsHandler.GetNewID();

                m_itemsList.Add(_item);
                m_client.m_pods += _item.m_base.m_pods;
            }
            else if (_offline == false)
            {
                if (m_itemsList.Any(x => x.EffectsInfos() == _item.EffectsInfos() && x.m_id == _item.m_id && x.m_position == _item.m_position))
                {
                    var item2 = m_itemsList.First(x => x.EffectsInfos() == _item.EffectsInfos() && x.m_id == _item.m_id && x.m_position == _item.m_position);

                    item2.m_quantity += _item.m_quantity;
                    m_client.m_pods += (_item.m_base.m_pods * _item.m_quantity);

                    RefreshBonus();
                    m_client.m_networkClient.Send(string.Format("OQ{0}|{1}", item2.m_id, item2.m_quantity));

                    return;
                }

                _item.m_id = ItemsHandler.GetNewID();
                m_itemsList.Add(_item);

                m_client.m_pods += _item.m_base.m_pods;
                RefreshBonus();

                m_client.m_networkClient.Send(string.Format("OAKO{0}", _item.ToString()));
            }
        }

        public void DeleteItem(int _id, int _quantity)
        {
            if (m_itemsList.Any(x => x.m_id == _id))
            {
                var item = m_itemsList.First(x => x.m_id == _id);

                if (item.m_quantity <= _quantity)
                {
                    m_client.m_pods -= (item.m_quantity * item.m_base.m_pods);

                    m_itemsList.Remove(item);
                    m_client.m_networkClient.Send(string.Format("OR{0}", item.m_id));

                    RefreshBonus();
                }
                else
                {
                    m_client.m_pods -= (_quantity * item.m_base.m_pods);

                    item.m_quantity -= _quantity;
                    m_client.m_networkClient.Send(string.Format("OQ{0}|{1}", item.m_id, item.m_quantity));

                    RefreshBonus();
                }
            }
            else
                m_client.m_networkClient.Send("BN");
        }

        public void MoveItem(int _id, int _pos, int _quantity)
        {
            if (!m_itemsList.Any(x => x.m_id == _id)) 
                return;

            var item = m_itemsList.First(x => x.m_id == _id);

            if (ItemsHandler.PositionAvaliable(item.m_base.m_type, item.m_base.isUsable, _pos) == false 
                || _pos == 1 && item.m_base.isTwoHands == true && isOccuptedPos(15) || _pos == 15 && isOccuptedPos(1))
            {
                m_client.m_networkClient.Send("BN");
                return;
            }

            if (ItemsHandler.ConditionsAvaliable(item.m_base, m_client) == false || !World.ConditionsHandler.HasCondition(m_client.m_networkClient, item.m_base.m_conds))
            {
                m_client.m_networkClient.Send("Im119|44");
                return;
            }

            if (item.m_base.m_type == 23 && _pos != -1)
            {
                if (!m_itemsList.Any(x => x.m_base.m_id == item.m_base.m_id && x.m_position != -1 && x.m_base.m_type == 23))
                {
                    m_client.m_networkClient.Send("OAEA");
                    return;
                }
            }

            if (item.m_base.m_level > m_client.m_level)
            {
                m_client.m_networkClient.Send("OAEL");
                return;
            }

            item.m_position = _pos;

            if (item.m_position == -1)
            {
                if (m_itemsList.Any(x => x.EffectsInfos() == item.EffectsInfos() && x.m_id == item.m_id && x.m_position == item.m_position))
                {
                    var item2 = m_itemsList.First(x => x.EffectsInfos() == item.EffectsInfos() && x.m_id == item.m_id && x.m_position == item.m_position);

                    item2.m_quantity += item.m_quantity;
                    m_client.m_pods += (item.m_base.m_pods * item.m_quantity);
                    RefreshBonus();

                    m_client.m_networkClient.Send(string.Format("OQ{0}|{1}", item2.m_id, item2.m_quantity));
                    m_client.m_networkClient.Send(string.Format("OR{0}", item.m_id));
                    m_itemsList.Remove(item);
                }
            }
            else
            {
                if (item.m_quantity > 1)
                {
                    if (item.m_base.m_type == 12 || item.m_base.m_type == 13 || item.m_base.m_type == 14 || item.m_base.m_type == 28 ||
                        item.m_base.m_type == 33 || item.m_base.m_type == 37 || item.m_base.m_type == 42 || item.m_base.m_type == 49 ||
                        item.m_base.m_type == 69 || item.m_base.m_type == 87)
                    {
                        if (_quantity <= 0) 
                            return;

                        var Copy = item;
                        Copy.m_quantity -= _quantity;

                        if (item.m_quantity == _quantity)
                            Copy.m_position = _pos;
                        else
                            Copy.m_position = -1;

                        item.m_quantity = _quantity;
                        AddItem(Copy, false);
                    }
                    else
                    {
                        var Copy = item;

                        Copy.m_quantity -= 1;
                        Copy.m_position = -1;

                        item.m_quantity = 1;
                        AddItem(Copy, false);
                    }

                    m_client.m_networkClient.Send(string.Format("OQ{0}|{1}", item.m_id, item.m_quantity));
                }
            }

            m_client.m_networkClient.Send(string.Format("OM{0}|{1}", item.m_id, (item.m_position != -1 ? item.m_position.ToString() : "")));
            m_client.GetMap().Send(string.Format("Oa{0}|{1}", m_client.m_id, m_client.GetItemsPos()));

            RefreshBonus();
        }

        public bool isOccuptedPos(int _pos)
        {
            return m_itemsList.Any(x => x.m_position == _pos);
        }

        public void ParseItems(string _datas)
        {
            var splited = _datas.Split(';');

            foreach (var infos in splited)
            {
                var allInfos = infos.Split('~');
                var item = new CharacterItem(Database.Cache.ItemsCache.m_itemsList.First(x => x.m_id == Convert.ToInt32(allInfos[0], 16)));

                item.m_id = ItemsHandler.GetNewID();
                item.m_quantity = Convert.ToInt32(allInfos[1], 16);

                if (allInfos[2] != "")
                    item.m_position = Convert.ToInt32(allInfos[2], 16);
                else
                    item.m_position = -1;

                if (allInfos[3] != "")
                {
                    var effectsList = allInfos[3].Split(',');

                    foreach (var effect in effectsList)
                    {
                        var NewEffect = new Effects.EffectItem();
                        string[] EffectInfos = effect.Split('#');

                        NewEffect.m_id = Convert.ToInt32(EffectInfos[0], 16);

                        if (EffectInfos[1] != "")
                            NewEffect.m_value = Convert.ToInt32(EffectInfos[1], 16);

                        if (EffectInfos[2] != "")
                            NewEffect.m_value2 = Convert.ToInt32(EffectInfos[2], 16);

                        if (EffectInfos[3] != "")
                            NewEffect.m_value3 = Convert.ToInt32(EffectInfos[3], 16);

                        NewEffect.m_effect = EffectInfos[4];

                        item.m_effectsList.Add(NewEffect);
                    }

                }

                m_client.m_pods += (item.m_base.m_pods * item.m_quantity);
                m_itemsList.Add(item);
            }
        }

        public void RefreshBonus()
        {
            m_client.ResetItemsStats();
            m_setsList.Clear();

            foreach (var item in m_itemsList)
            {
                if (item.m_position != -1 && item.m_position < 23)
                {
                    foreach (var effect in item.m_effectsList)
                        effect.ParseEffect(m_client);
                }
                if (item.m_base.m_set != -1 && item.m_position != -1)
                {
                    if (m_setsList.ContainsKey(item.m_base.m_set))
                    {
                        if (!m_setsList[item.m_base.m_set].myItemsList.Contains(item.m_base.m_id))
                            m_setsList[item.m_base.m_set].myItemsList.Add(item.m_base.m_id);
                    }
                    else
                    {
                        m_setsList.Add(item.m_base.m_set, new CharacterSet(item.m_base.m_set));
                        m_setsList[item.m_base.m_set].myItemsList.Clear();
                        m_setsList[item.m_base.m_set].myItemsList.Add(item.m_base.m_id);
                    }
                }
            }

            foreach (var set in m_setsList.Values)
            {
                var numberItems = set.myItemsList.Count;
                var strItems = string.Join(";", set.myItemsList);
                var strEffects = "";

                foreach (var effect in set.myBonusList[numberItems])
                {
                    strEffects += string.Format("{0},", effect.SetString());
                    effect.ParseEffect(m_client);
                }

                m_client.m_networkClient.Send(string.Format("OS+{0}|{1}|{2}", set.m_id, strItems,
                    (strEffects == "" ? "" : strEffects.Substring(0, strEffects.Length - 1))));
            }

            m_client.SendPods();
            m_client.SendChararacterStats();
        }

        public void UseItem(string _datas)
        {
            if (m_client.m_state.onMove == true)
            {
                m_client.m_networkClient.Send("BN");
                return;
            }

            var allDatas = _datas.Split('|');

            var itemID = int.Parse(allDatas[0]);
            var charID = m_client.m_id;
            var cellID = m_client.m_mapCell;

            if (allDatas.Length > 2)
            {
                charID = int.Parse(allDatas[1]);
                cellID = int.Parse(allDatas[2]);
            }

            if (!m_itemsList.Any(x => x.m_id == itemID))
            {
                m_client.m_networkClient.Send("OUE");
                return;
            }

            var item = m_itemsList.First(x => x.m_id == itemID);

            if (item.m_base.isUsable == false)
            {
                m_client.m_networkClient.Send("BN");
                return;
            }

            var usable = Database.Cache.ItemsCache.m_usablesList.First(x => x.m_base == item.m_base.m_id);
            var character = CharactersManager.m_charactersList.First(x => x.m_id == charID);

            if (!World.ConditionsHandler.HasCondition(m_client.m_networkClient, item.m_base.m_conds))
            {
                m_client.m_networkClient.Send("Im119|44");
                return;
            }

            usable.ParseEffect(character);

            if(usable.m_mustDelete == true)
                DeleteItem(item.m_id, 1);
        }
    }
}
