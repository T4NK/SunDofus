using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class InventaryItems
    {
        public List<CharacterItem> ItemsList;
        public Character myClient;
        public Dictionary<int,CharacterSet> SetsList;

        public InventaryItems(Character myCharacter)
        {
            myClient = myCharacter;
            ItemsList = new List<CharacterItem>();
            SetsList = new Dictionary<int,CharacterSet>();
        }

        public void AddItem(int ID, bool OffLine)
        {
            if (OffLine == true)
            {
                if (!Database.Cache.ItemsCache.ItemsList.Any(x => x.myID == ID)) return;

                var BaseItem = Database.Cache.ItemsCache.ItemsList.First(x => x.myID == ID);
                var myItem = new CharacterItem(BaseItem);

                myItem.ParseJet();
                myItem.GeneratItem();

                foreach (var myItem2 in ItemsList)
                {
                    if (myItem2.myBaseItem.myID == myItem.myBaseItem.myID && myItem2.EffectsInfos() == myItem.EffectsInfos() && myItem2.myPosition == myItem.myPosition)
                    {
                        myItem2.myQuantity += myItem.myQuantity;
                        myClient.Pods += (myItem.myBaseItem.myPods * myItem.myQuantity);
                        return;
                    }
                }

                myItem.myID = ItemsHandler.GetNewID();
                ItemsList.Add(myItem);

                myClient.Pods += myItem.myBaseItem.myPods;
            }
            else if (OffLine == false)
            {
                if (!Database.Cache.ItemsCache.ItemsList.Any(x => x.myID == ID)) return;

                var BaseItem = Database.Cache.ItemsCache.ItemsList.First(x => x.myID == ID);
                var myItem = new CharacterItem(BaseItem);

                myItem.ParseJet();
                myItem.GeneratItem();

                foreach (var myItem2 in ItemsList)
                {
                    if (myItem2.myBaseItem.myID == myItem.myBaseItem.myID && myItem2.EffectsInfos() == myItem.EffectsInfos() && myItem2.myPosition == myItem.myPosition)
                    {
                        myItem2.myQuantity += myItem.myQuantity;
                        myClient.Pods += (myItem.myBaseItem.myPods * myItem.myQuantity);
                        RefreshBonus();
                        myClient.Client.Send(string.Format("OQ{0}|{1}", myItem2.myID, myItem2.myQuantity));
                        return;
                    }
                }

                myItem.myID = ItemsHandler.GetNewID();
                ItemsList.Add(myItem);

                myClient.Pods += myItem.myBaseItem.myPods;
                RefreshBonus();

                myClient.Client.Send(string.Format("OAKO{0}", myItem.ToString()));
            }
        }

        public void AddItem(CharacterItem myItem, bool OffLine)
        {
            if(OffLine == true)
            {
                foreach (var myItem2 in ItemsList)
                {
                    if (myItem2.myBaseItem.myID == myItem.myBaseItem.myID && myItem2.EffectsInfos() == myItem.EffectsInfos() && myItem2.myPosition == myItem.myPosition)
                    {
                        myItem2.myQuantity += myItem.myQuantity;
                        myClient.Pods += (myItem.myBaseItem.myPods * myItem.myQuantity);
                        return;
                    }
                }

                myItem.myID = ItemsHandler.GetNewID();
                ItemsList.Add(myItem);

                myClient.Pods += myItem.myBaseItem.myPods;
            }
            else if (OffLine == false)
            {
                foreach (var myItem2 in ItemsList)
                {
                    if (myItem2.myBaseItem.myID == myItem.myBaseItem.myID && myItem2.EffectsInfos() == myItem.EffectsInfos() && myItem2.myPosition == myItem.myPosition)
                    {
                        myItem2.myQuantity += myItem.myQuantity;
                        myClient.Pods += (myItem.myBaseItem.myPods * myItem.myQuantity);
                        RefreshBonus();
                        myClient.Client.Send(string.Format("OQ{0}|{1}", myItem2.myID, myItem2.myQuantity));
                        return;
                    }
                }

                myItem.myID = ItemsHandler.GetNewID();
                ItemsList.Add(myItem);

                myClient.Pods += myItem.myBaseItem.myPods;
                RefreshBonus();

                myClient.Client.Send(string.Format("OAKO{0}", myItem.ToString()));
            }
        }

        public void DeleteItem(int ID, int Quantity)
        {
            if (ItemsList.Any(x => x.myID == ID))
            {
                var myItem = ItemsList.First(x => x.myID == ID);

                if (myItem.myQuantity <= Quantity)
                {
                    myClient.Pods -= (myItem.myQuantity * myItem.myBaseItem.myPods);

                    ItemsList.Remove(myItem);
                    myClient.Client.Send(string.Format("OR{0}", myItem.myID));

                    RefreshBonus();
                }
                else
                {
                    myClient.Pods -= (Quantity * myItem.myBaseItem.myPods);

                    myItem.myQuantity -= Quantity;
                    myClient.Client.Send(string.Format("OQ{0}|{1}", myItem.myID, myItem.myQuantity));

                    RefreshBonus();
                }
            }
        }

        public void MoveItem(int ID, int Pos, int Quantity)
        {
            if (!ItemsList.Any(x => x.myID == ID)) return;

            var myItem = ItemsList.First(x => x.myID == ID);

            if (ItemsHandler.PositionAvaliable(myItem.myBaseItem.myType, myItem.myBaseItem.meUsable, Pos) == false)
            {
                myClient.Client.Send("BN");
                return;
            }

            if (Pos == 1 && myItem.myBaseItem.meTwoHands == true && isOccuptedPos(15)) // Arme à deux mains avec Bouclier
            {
                myClient.Client.Send("BN");
                return;
            }

            if (ItemsHandler.ConditionsAvaliable(myItem.myBaseItem, myClient) == false)
            {
                myClient.Client.Send("Im119|44");
                return;
            }

            if (Pos == 15 && isOccuptedPos(1)) // Bouclier avec Arme à deux mains
            {
                if (ItemsList.First(x => x.myPosition == 1).myBaseItem.meTwoHands == true)
                {
                    myClient.Client.Send("BN");
                    return;
                }
            }

            if (myItem.myBaseItem.myType == 23 && Pos != -1) // DOFUS
            {
                if (!ItemsList.Any(x => x.myBaseItem.myID == myItem.myBaseItem.myID && x.myPosition != -1 && x.myBaseItem.myType == 23))
                {
                    myClient.Client.Send("OAEA");
                    return;
                }
            }

            if (myItem.myBaseItem.myLevel > myClient.Level) //Si trop petit level
            {
                myClient.Client.Send("OAEL");
                return;
            }

            myItem.myPosition = Pos;

            if (myItem.myPosition == -1)
            {
                foreach (var myItem2 in ItemsList)
                {
                    if (myItem2.myBaseItem.myID == myItem.myBaseItem.myID && myItem2.EffectsInfos() == myItem.EffectsInfos() && myItem2.myPosition == myItem.myPosition
                        && myItem2 != myItem)
                    {
                        myItem2.myQuantity += myItem.myQuantity;
                        myClient.Pods += (myItem.myBaseItem.myPods * myItem.myQuantity);
                        RefreshBonus();
                        myClient.Client.Send(string.Format("OQ{0}|{1}", myItem2.myID, myItem2.myQuantity));
                        myClient.Client.Send(string.Format("OR{0}", myItem.myID));
                        ItemsList.Remove(myItem);
                        return;
                    }
                }
            }
            else
            {
                if (myItem.myQuantity > 1)
                {
                    if (myItem.myBaseItem.myType == 12 | myItem.myBaseItem.myType == 13 | myItem.myBaseItem.myType == 14 | myItem.myBaseItem.myType == 28 |
                        myItem.myBaseItem.myType == 33 | myItem.myBaseItem.myType == 37 | myItem.myBaseItem.myType == 42 | myItem.myBaseItem.myType == 49 |
                        myItem.myBaseItem.myType == 69 | myItem.myBaseItem.myType == 87)
                    {
                        if (Quantity <= 0) return;

                        var Copy = myItem;
                        Copy.myQuantity -= Quantity;

                        if (myItem.myQuantity == Quantity)
                            Copy.myPosition = Pos;
                        else
                            Copy.myPosition = -1;

                        myItem.myQuantity = Quantity;
                        AddItem(Copy, false);
                    }
                    else
                    {
                        var Copy = myItem;

                        Copy.myQuantity -= 1;
                        Copy.myPosition = -1;

                        myItem.myQuantity = 1;
                        AddItem(Copy, false);
                    }

                    myClient.Client.Send(string.Format("OQ{0}|{1}", myItem.myID, myItem.myQuantity));
                }
            }

            myClient.Client.Send(string.Format("OM{0}|{1}", myItem.myID, (myItem.myPosition != -1 ? myItem.myPosition.ToString() : "")));
            myClient.GetMap().Send(string.Format("Oa{0}|{1}", myClient.ID, myClient.GetItemsPos()));

            RefreshBonus();
        }

        public bool isOccuptedPos(int Pos)
        {
            if (ItemsList.Any(x => x.myPosition == Pos)) return true;
            return false;
        }

        public void ParseItems(string Data)
        {
            string[] Spliter = Data.Split(';');

            foreach (var Infos in Spliter)
            {
                string[] AllInfos = Infos.Split('~');

                var myItem = new CharacterItem(Database.Cache.ItemsCache.ItemsList.First(x => x.myID == Convert.ToInt32(AllInfos[0], 16)));

                myItem.myID = ItemsHandler.GetNewID();
                myItem.myQuantity = Convert.ToInt32(AllInfos[1], 16);

                if (AllInfos[2] != "")
                    myItem.myPosition = Convert.ToInt32(AllInfos[2], 16);
                else
                    myItem.myPosition = -1;

                if (AllInfos[3] != "")
                {
                    string[] EffectsList = AllInfos[3].Split(',');

                    foreach (var Effect in EffectsList)
                    {
                        var NewEffect = new Effect.EffectsItems();
                        string[] EffectInfos = Effect.Split('#');

                        NewEffect.ID = Convert.ToInt32(EffectInfos[0], 16);

                        if (EffectInfos[1] != "")
                            NewEffect.Value = Convert.ToInt32(EffectInfos[1], 16);

                        if (EffectInfos[2] != "")
                            NewEffect.Value2 = Convert.ToInt32(EffectInfos[2], 16);

                        if (EffectInfos[3] != "")
                            NewEffect.Value3 = Convert.ToInt32(EffectInfos[3], 16);

                        NewEffect.Effect = EffectInfos[4];

                        myItem.myEffectsList.Add(NewEffect);
                    }

                }

                myClient.Pods += (myItem.myBaseItem.myPods * myItem.myQuantity);

                ItemsList.Add(myItem);
            }
        }

        public void RefreshBonus()
        {
            myClient.ResetItemsStats();
            SetsList.Clear();

            foreach (var myItem in ItemsList)
            {
                if (myItem.myPosition != -1 && myItem.myPosition < 23)
                {
                    foreach (var myEffect in myItem.myEffectsList)
                        myEffect.ParseEffect(myClient);
                }
                if (myItem.myBaseItem.mySet != -1 && myItem.myPosition != -1)
                {
                    if (SetsList.ContainsKey(myItem.myBaseItem.mySet))
                    {
                        if (!SetsList[myItem.myBaseItem.mySet].myItemsList.Contains(myItem.myBaseItem.myID))
                            SetsList[myItem.myBaseItem.mySet].myItemsList.Add(myItem.myBaseItem.myID);
                    }
                    else
                    {
                        SetsList.Add(myItem.myBaseItem.mySet, new CharacterSet(myItem.myBaseItem.mySet));
                        SetsList[myItem.myBaseItem.mySet].myItemsList.Clear();
                        SetsList[myItem.myBaseItem.mySet].myItemsList.Add(myItem.myBaseItem.myID);
                    }
                }
            }

            foreach (var mySet in SetsList.Values)
            {
                var NumberItems = mySet.myItemsList.Count;
                var StrItems = "";
                var StrEffects = "";

                foreach (var ItemID in mySet.myItemsList)
                    StrItems += string.Format("{0};", ItemID);

                foreach (var myEffect in mySet.myBonusList[NumberItems])
                {
                    StrEffects += string.Format("{0},", myEffect.SetString());
                    myEffect.ParseEffect(myClient);
                }

                myClient.Client.Send(string.Format("OS+{0}|{1}|{2}", mySet.myID, (StrItems == "" ? "" : StrItems.Substring(0, StrItems.Length - 1)),
                    (StrEffects == "" ? "" : StrEffects.Substring(0, StrEffects.Length - 1))));
            }

            myClient.SendPods();
            myClient.SendCharStats();
        }

        public void UseItem(string Data)
        {
            if (myClient.State.OnMove == true)
            {
                myClient.Client.Send("BN");
                return;
            }

            string[] AllData = Data.Split('|');

            var ItemID = int.Parse(AllData[0]);
            var CharID = myClient.ID;
            var CellID = myClient.MapCell;

            if (AllData.Length > 2)
            {
                CharID = int.Parse(AllData[1]);
                CellID = int.Parse(AllData[2]);
            }

            if (!ItemsList.Any(x => x.myID == ItemID))
            {
                myClient.Client.Send("OUE");
                return;
            }

            var myItem = ItemsList.First(x => x.myID == ItemID);

            if (myItem.myBaseItem.meUsable == false)
            {
                myClient.Client.Send("BN");
                return;
            }

            var myUsable = Database.Cache.ItemsCache.UsablesList.First(x => x.myBaseItemID == myItem.myBaseItem.myID);
            var myCharacter = CharactersManager.CharactersList.First(x => x.ID == CharID);

            if (!myUsable.ConditionsAvaliable(myCharacter))
            {
                myClient.Client.Send("Im119|44");
                return;
            }

            myUsable.ParseEffect(myCharacter);

            DeleteItem(myItem.myID, 1);
        }
    }
}
