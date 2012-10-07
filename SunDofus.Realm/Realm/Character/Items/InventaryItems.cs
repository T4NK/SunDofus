﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class InventaryItems
    {
        public List<CharacterItem> ItemsList;
        public Character Client;
        public Dictionary<int,CharacterSet> SetsList;

        public InventaryItems(Character Ch)
        {
            Client = Ch;
            ItemsList = new List<CharacterItem>();
            SetsList = new Dictionary<int,CharacterSet>();
        }

        public void AddItem(int ID, bool OffLine)
        {
            if (OffLine == true)
            {
                if (!Database.Cache.ItemsCache.ItemsList.Any(x => x.ID == ID)) return;
                Database.Models.Items.ItemModel BaseItem = Database.Cache.ItemsCache.ItemsList.First(x => x.ID == ID);
                Items.CharacterItem m_I = new CharacterItem(BaseItem);
                m_I.ParseJet();
                m_I.GeneratItem();

                foreach (CharacterItem i2 in ItemsList)
                {
                    if (i2.BaseItem.ID == m_I.BaseItem.ID && i2.EffectsInfos() == m_I.EffectsInfos() && i2.Position == m_I.Position)
                    {
                        i2.Quantity += m_I.Quantity;
                        Client.Pods += (m_I.BaseItem.Pods * m_I.Quantity);
                        return;
                    }
                }

                m_I.ID = ItemsHandler.GetNewID();
                ItemsList.Add(m_I);

                Client.Pods += m_I.BaseItem.Pods;
            }
            else if (OffLine == false)
            {
                if (!Database.Cache.ItemsCache.ItemsList.Any(x => x.ID == ID)) return;
                Database.Models.Items.ItemModel BaseItem = Database.Cache.ItemsCache.ItemsList.First(x => x.ID == ID);
                Items.CharacterItem m_I = new CharacterItem(BaseItem);
                m_I.ParseJet();
                m_I.GeneratItem();

                foreach (CharacterItem i2 in ItemsList)
                {
                    if (i2.BaseItem.ID == m_I.BaseItem.ID && i2.EffectsInfos() == m_I.EffectsInfos() && i2.Position == m_I.Position)
                    {
                        i2.Quantity += m_I.Quantity;
                        Client.Pods += (m_I.BaseItem.Pods * m_I.Quantity);
                        RefreshBonus();
                        Client.Client.Send("OQ" + i2.ID + "|" + i2.Quantity);
                        return;
                    }
                }

                m_I.ID = ItemsHandler.GetNewID();
                ItemsList.Add(m_I);

                Client.Pods += m_I.BaseItem.Pods;
                RefreshBonus();

                Client.Client.Send("OAKO" + m_I.ToString());
            }
        }

        public void AddItem(CharacterItem m_I, bool OffLine)
        {
            if(OffLine == true)
            {
                foreach (CharacterItem i2 in ItemsList)
                {
                    if (i2.BaseItem.ID == m_I.BaseItem.ID && i2.EffectsInfos() == m_I.EffectsInfos() && i2.Position == m_I.Position)
                    {
                        i2.Quantity += m_I.Quantity;
                        Client.Pods += (m_I.BaseItem.Pods * m_I.Quantity);
                        return;
                    }
                }

                m_I.ID = ItemsHandler.GetNewID();
                ItemsList.Add(m_I);

                Client.Pods += m_I.BaseItem.Pods;
            }
            else if (OffLine == false)
            {
                foreach (CharacterItem i2 in ItemsList)
                {
                    if (i2.BaseItem.ID == m_I.BaseItem.ID && i2.EffectsInfos() == m_I.EffectsInfos() && i2.Position == m_I.Position)
                    {
                        i2.Quantity += m_I.Quantity;
                        Client.Pods += (m_I.BaseItem.Pods * m_I.Quantity);
                        RefreshBonus();
                        Client.Client.Send("OQ" + i2.ID + "|" + i2.Quantity);
                        return;
                    }
                }

                m_I.ID = ItemsHandler.GetNewID();
                ItemsList.Add(m_I);

                Client.Pods += m_I.BaseItem.Pods;
                RefreshBonus();

                Client.Client.Send("OAKO" + m_I.ToString());
            }
        }

        public void DeleteItem(int ID, int Quantity)
        {
            if (ItemsList.Any(x => x.ID == ID))
            {
                CharacterItem m_I = ItemsList.First(x => x.ID == ID);
                if (m_I.Quantity <= Quantity)
                {
                    Client.Pods -= (m_I.Quantity * m_I.BaseItem.Pods);

                    ItemsList.Remove(m_I);
                    Client.Client.Send("OR" + m_I.ID);

                    RefreshBonus();
                }
                else
                {
                    Client.Pods -= (Quantity * m_I.BaseItem.Pods);

                    m_I.Quantity -= Quantity;
                    Client.Client.Send("OQ" + m_I.ID + "|" + m_I.Quantity);

                    RefreshBonus();
                }
            }
        }

        public void MoveItem(int ID, int Pos, int Quantity)
        {
            if (!ItemsList.Any(x => x.ID == ID)) return;

            CharacterItem m_I = ItemsList.First(x => x.ID == ID);
            if (ItemsHandler.PositionAvaliable(m_I.BaseItem.Type, m_I.BaseItem.Usable, Pos) == false)
            {
                Client.Client.Send("BN");
                return;
            }

            if (Pos == 1 && m_I.BaseItem.TwoHands == true && isOccuptedPos(15)) // Arme à deux mains avec Bouclier
            {
                Client.Client.Send("BN");
                return;
            }

            if (ItemsHandler.ConditionsAvaliable(m_I.BaseItem, Client) == false)
            {
                Client.Client.Send("Im119|44");
                return;
            }

            if (Pos == 15 && isOccuptedPos(1)) // Bouclier avec Arme à deux mains
            {
                if (ItemsList.First(x => x.Position == 1).BaseItem.TwoHands == true)
                {
                    Client.Client.Send("BN");
                    return;
                }
            }

            if (m_I.BaseItem.Type == 23 && Pos != -1) // DOFUS
            {
                if (!ItemsList.Any(x => x.BaseItem.ID == m_I.BaseItem.ID && x.Position != -1 && x.BaseItem.Type == 23))
                {
                    Client.Client.Send("OAEA");
                    return;
                }
            }

            if (m_I.BaseItem.Level > Client.Level) //Si trop petit level
            {
                Client.Client.Send("OAEL");
                return;
            }

            m_I.Position = Pos;

            if (m_I.Position == -1)
            {
                foreach (CharacterItem i2 in ItemsList)
                {
                    if (i2.BaseItem.ID == m_I.BaseItem.ID && i2.EffectsInfos() == m_I.EffectsInfos() && i2.Position == m_I.Position
                        && i2 != m_I)
                    {
                        i2.Quantity += m_I.Quantity;
                        Client.Pods += (m_I.BaseItem.Pods * m_I.Quantity);
                        RefreshBonus();
                        Client.Client.Send("OQ" + i2.ID + "|" + i2.Quantity);
                        Client.Client.Send("OR" + m_I.ID);
                        ItemsList.Remove(m_I);
                        return;
                    }
                }
            }
            else
            {
                if (m_I.Quantity > 1)
                {
                    if (m_I.BaseItem.Type == 12 | m_I.BaseItem.Type == 13 | m_I.BaseItem.Type == 14 | m_I.BaseItem.Type == 28 |
                        m_I.BaseItem.Type == 33 | m_I.BaseItem.Type == 37 | m_I.BaseItem.Type == 42 | m_I.BaseItem.Type == 49 |
                        m_I.BaseItem.Type == 69 | m_I.BaseItem.Type == 87)
                    {
                        if (Quantity <= 0) return;
                        CharacterItem Copy = m_I;
                        Copy.Quantity -= Quantity;

                        if (m_I.Quantity == Quantity)
                            Copy.Position = Pos;
                        else
                            Copy.Position = -1;

                        m_I.Quantity = Quantity;
                        AddItem(Copy, false);
                    }
                    else
                    {
                        CharacterItem Copy = m_I;

                        Copy.Quantity -= 1;
                        Copy.Position = -1;

                        m_I.Quantity = 1;
                        AddItem(Copy, false);
                    }

                    Client.Client.Send("OQ" + m_I.ID + "|" + m_I.Quantity);
                }
            }

            Client.Client.Send("OM" + m_I.ID + "|" + (m_I.Position != -1 ? m_I.Position.ToString() : ""));
            Client.GetMap().Send("Oa" + Client.ID + "|" + Client.GetItemsPos());

            RefreshBonus();
        }

        public bool isOccuptedPos(int Pos)
        {
            if (ItemsList.Any(x => x.Position == Pos)) return true;
            return false;
        }

        public void ParseItems(string Data)
        {
            string[] Spliter = Data.Split(';');

            foreach (string Infos in Spliter)
            {
                string[] AllInfos = Infos.Split('~');
                Items.CharacterItem m_I = new CharacterItem(Database.Cache.ItemsCache.ItemsList.First(x => x.ID == Convert.ToInt32(AllInfos[0], 16)));

                m_I.ID = ItemsHandler.GetNewID();
                m_I.Quantity = Convert.ToInt32(AllInfos[1], 16);

                if (AllInfos[2] != "")
                    m_I.Position = Convert.ToInt32(AllInfos[2], 16);
                else
                    m_I.Position = -1;

                if (AllInfos[3] != "")
                {
                    string[] EffectsList = AllInfos[3].Split(',');

                    foreach (string Effect in EffectsList)
                    {
                        Effect.EffectsItems NewEffect = new Effect.EffectsItems();
                        string[] EffectInfos = Effect.Split('#');

                        NewEffect.ID = Convert.ToInt32(EffectInfos[0], 16);

                        if (EffectInfos[1] != "")
                            NewEffect.Value = Convert.ToInt32(EffectInfos[1], 16);

                        if (EffectInfos[2] != "")
                            NewEffect.Value2 = Convert.ToInt32(EffectInfos[2], 16);

                        if (EffectInfos[3] != "")
                            NewEffect.Value3 = Convert.ToInt32(EffectInfos[3], 16);

                        NewEffect.Effect = EffectInfos[4];

                        m_I.EffectsList.Add(NewEffect);
                    }

                }

                Client.Pods += (m_I.BaseItem.Pods * m_I.Quantity);

                ItemsList.Add(m_I);
            }
        }

        public void RefreshBonus()
        {
            Client.ResetItemsStats();
            SetsList.Clear();

            foreach (CharacterItem m_I in ItemsList)
            {
                if (m_I.Position != -1 && m_I.Position < 23)
                {
                    foreach (Effect.EffectsItems m_E in m_I.EffectsList)
                    {
                        m_E.ParseEffect(Client);
                    }
                }
                if (m_I.BaseItem.Set != -1 && m_I.Position != -1)
                {
                    if (SetsList.ContainsKey(m_I.BaseItem.Set))
                    {
                        if(! SetsList[m_I.BaseItem.Set].ItemsList.Contains(m_I.BaseItem.ID))
                        {
                            SetsList[m_I.BaseItem.Set].ItemsList.Add(m_I.BaseItem.ID);
                        }
                    }
                    else
                    {
                        SetsList.Add(m_I.BaseItem.Set, new CharacterSet(m_I.BaseItem.Set));
                        SetsList[m_I.BaseItem.Set].ItemsList.Clear();
                        SetsList[m_I.BaseItem.Set].ItemsList.Add(m_I.BaseItem.ID);
                    }
                }
            }

            foreach (CharacterSet m_S in SetsList.Values)
            {
                int NumberItems = m_S.ItemsList.Count;
                string StrItems = "";
                string StrEffects = "";

                foreach (int ItemID in m_S.ItemsList)
                {
                    StrItems += ItemID + ";";
                }

                foreach (Effect.EffectsItems m_E in m_S.BonusList[NumberItems])
                {
                    StrEffects += m_E.SetString() + ",";
                    m_E.ParseEffect(Client);
                }

                Client.Client.Send("OS+" + m_S.ID + "|" + (StrItems == "" ? "" : StrItems.Substring(0, StrItems.Length - 1)) + "|"
                    + (StrEffects == "" ? "" : StrEffects.Substring(0, StrEffects.Length - 1)));
            }

            Client.SendPods();
            Client.SendCharStats();
        }

        public void UseItem(string Data)
        {
            if (Client.State.OnMove == true)
            {
                Client.Client.Send("BN");
                return;
            }
            string[] AllData = Data.Split('|');

            int ItemID = int.Parse(AllData[0]);
            int CharID = Client.ID;
            int CellID = Client.MapCell;

            if (AllData.Length > 2)
            {
                CharID = int.Parse(AllData[1]);
                CellID = int.Parse(AllData[2]);
            }

            if (!ItemsList.Any(x => x.ID == ItemID))
            {
                Client.Client.Send("OUE");
                return;
            }

            CharacterItem Item = ItemsList.First(x => x.ID == ItemID);

            if (Item.BaseItem.Usable == false)
            {
                Client.Client.Send("BN");
                return;
            }

            Database.Models.Items.ItemUsableModel m_I = Database.Cache.ItemsCache.UsablesList.First(x => x.BaseItemID == Item.BaseItem.ID);
            Character m_C = CharactersManager.CharactersList.First(x => x.ID == CharID);

            if (!m_I.ConditionsAvaliable(m_C))
            {
                Client.Client.Send("Im119|44");
                return;
            }

            m_I.ParseEffect(m_C);

            DeleteItem(Item.ID, 1);
        }
    }
}