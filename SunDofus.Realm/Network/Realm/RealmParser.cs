using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.Character;
using realm.Realm.Map;
using realm.Realm.Character.Stats;
using realm.Realm;
using realm.Realm.World;

namespace realm.Network.Realm
{
    class RealmParser
    {
        public RealmClient myClient;

        delegate void Packets(string myString);
        Dictionary<string, Packets> myPackets;

        public RealmParser(RealmClient newClient)
        {
            myClient = newClient;
            myPackets = new Dictionary<string, Packets>();
            RegisterPackets();
        }

        void RegisterPackets()
        {
            myPackets["AA"] = CreateCharacter;
            myPackets["AB"] = StatsBoosts;
            myPackets["AD"] = DeleteCharacter;
            myPackets["Ag"] = SendGifts;
            myPackets["AG"] = AcceptGift;
            myPackets["AL"] = SendCharacterList;
            myPackets["AP"] = SendRandomName;
            myPackets["AS"] = SelectCharacter;
            myPackets["AT"] = ParseTicket;
            myPackets["AV"] = AV_Packet;
            myPackets["BA"] = ParseConsoleMessage;
            myPackets["BD"] = SendDate;
            myPackets["BM"] = ParseChatMessage;
            myPackets["cC"] = ChangeChannel;
            myPackets["GA"] = GameAction;
            myPackets["GC"] = CreateGame;
            myPackets["GI"] = GameInformations;
            myPackets["GK"] = EndAction;
            myPackets["Od"] = DeleteItem;
            myPackets["OM"] = MoveItem;
            myPackets["OU"] = UseItem;
        }

        public void Parse(string Data)
        {
            if (Data == "ping")
                myClient.Send("pong");
            else if (Data == "qping")
                myClient.Send("qpong");

            if (Data.Length < 2) return;

            string Header = Data.Substring(0, 2);

            if (!myPackets.ContainsKey(Header))
            {
                myClient.Send("BN");
                return;
            }

            myPackets[Header](Data.Substring(2));
        }

        #region Ticket

        public void ParseTicket(string Data)
        {
            Data = Data.Replace("AT", "");
            if (Network.Authentication.AuthenticationKeys.myKeys.Any(x => x.myKey == Data))
            {
                var Key = Network.Authentication.AuthenticationKeys.myKeys.First(x => x.myKey == Data);

                myClient.myInfos = Key.myInfos;
                myClient.myInfos.ParseCharacters();
                myClient.ParseCharacters();

                myClient.isAuth = true;

                Network.ServersHandler.myAuthLink.Send(string.Format("SNC|{0}", myClient.myInfos.mymPseudo));
                myClient.Send("ATK0");
            }
            else
                myClient.Send("ATE");
        }

        #endregion
        
        #region Character

        public void SendRandomName(string myStr)
        {
            myClient.Send(string.Format("APK{0}", Utilities.Basic.RandomName()));
        }

        public void AV_Packet(string myStr)
        {
            myClient.Send("AV0");
        }

        public void SendCharacterList(string myStr)
        {
            string Pack = string.Format("ALK{0}|{1}", myClient.myInfos.mySubscription, myClient.myInfos.myCharacters.Count);

            if (myClient.myInfos.myCharacters.Count != 0)
            {
                foreach (realm.Realm.Character.Character m_C in myClient.myCharacters)
                    Pack += string.Format("|{0}", m_C.PatternList());
            }

            myClient.Send(Pack);
        }

        public void CreateCharacter(string Packet)
        {
            try
            {
                string[] CharData = Packet.Split('|');

                if (CharData[0] != "" | CharactersManager.ExistsName(CharData[0]) == false)
                {
                    var myCharacter = new Character();

                    myCharacter.ID = Database.Cache.CharactersCache.GetNewID();
                    myCharacter.myName = CharData[0];
                    myCharacter.Level = Utilities.Config.myConfig.GetIntElement("StartLevel");
                    myCharacter.Class = int.Parse(CharData[1]);
                    myCharacter.Sex = int.Parse(CharData[2]);
                    myCharacter.Skin = int.Parse(myCharacter.Class + "" + myCharacter.Sex);
                    myCharacter.Size = 100;
                    myCharacter.Color = int.Parse(CharData[3]);
                    myCharacter.Color2 = int.Parse(CharData[4]);
                    myCharacter.Color3 = int.Parse(CharData[5]);

                    myCharacter.MapID = Utilities.Config.myConfig.GetIntElement("StartMap");
                    myCharacter.MapCell = Utilities.Config.myConfig.GetIntElement("StartCell");
                    myCharacter.Dir = Utilities.Config.myConfig.GetIntElement("StartDir");

                    myCharacter.CharactPoint = (myCharacter.Level - 1) * 5;
                    myCharacter.SpellPoint = (myCharacter.Level - 1);

                    myCharacter.NewCharacter = true;

                    if (myCharacter.Class < 1 | myCharacter.Class > 12 | myCharacter.Sex < 0 | myCharacter.Sex > 1)
                    {
                        myClient.Send("AAE");
                        return;
                    }

                    myCharacter.mySpellInventary.LearnSpells();

                    Database.Cache.CharactersCache.CreateCharacter(myCharacter);
                    CharactersManager.CharactersList.Add(myCharacter);
                    myClient.myCharacters.Add(myCharacter);

                    Network.ServersHandler.myAuthLink.Send(string.Format("SNAC|{0}|{1}", myClient.myInfos.myId, myClient.myInfos.AddNewCharacterToAccount(myCharacter.myName)));

                    myClient.Send("TB");
                    myClient.Send("AAK");
                    SendCharacterList("");
                }
                else
                {
                    myClient.Send("AAE");
                }
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(e.ToString());
            }
        }

        public void DeleteCharacter(string Packet)
        {
            var ID = int.Parse(Packet.Split('|')[0]);
            var myCharacter = CharactersManager.CharactersList.First(x => x.ID == ID);

            if (Packet.Split('|')[1] != myClient.myInfos.myAnswer && myCharacter.Level >= 20)
            {
                myClient.Send("ADE");
                return;
            }

            CharactersManager.CharactersList.Remove(myCharacter);
            myClient.myCharacters.Remove(myCharacter);

            Network.ServersHandler.myAuthLink.Send(string.Format("SDAC|{0}|{1}", myClient.myInfos.myId, myClient.myInfos.RemoveCharacterToAccount(myCharacter.myName)));
            Database.Cache.CharactersCache.DeleteCharacter(myCharacter.myName);

            SendCharacterList("");
        }

        public void SelectCharacter(string Packet)
        {
            var myCharacter = CharactersManager.CharactersList.First(x => x.ID == int.Parse(Packet));

            if (myClient.myCharacters.Contains(myCharacter))
            {
                myClient.myPlayer = myCharacter;
                myClient.myPlayer.State = new CharacterState(myClient.myPlayer);
                myClient.myPlayer.Client = myClient;

                myClient.myPlayer.isConnected = true;

                myClient.Send(string.Format("ASK{0}", myClient.myPlayer.PatternSelect()));
            }
            else
                myClient.Send("ASE");
        }

        #endregion

        #region Gift

        public void SendGifts(string myStr)
        {
            myClient.SendGifts();
        }

        public void AcceptGift(string ID)
        {
            try
            {
                string[] Infos = ID.Split('|');

                if (myClient.myCharacters.Any(x => x.ID == int.Parse(Infos[1])))
                {
                    if (myClient.myInfos.myGifts.Any(x => x.myId == int.Parse(Infos[0])))
                    {
                        var myGift = myClient.myInfos.myGifts.First(e => e.myId == int.Parse(Infos[0]));
                        myClient.myCharacters.First(x => x.ID == int.Parse(Infos[1])).myInventary.AddItem(myGift.myItem, true);

                        myClient.Send("AG0");
                        Network.ServersHandler.myAuthLink.Send(string.Format("SNDG|{0}|{1}", myGift.myId, myClient.myInfos.myId));
                        myClient.myInfos.myGifts.Remove(myGift);

                    }
                    else
                        myClient.Send("AGE");
                }
                else
                    myClient.Send("AGE");
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(e.ToString());
            }
        }

        #endregion

        #region Realm

        void SendDate(string myStr)
        {
            myClient.Send(string.Format("BD{0}", Utilities.Basic.GetDofusDate()));
        }

        public void CreateGame(string myStr)
        {
            myClient.Send(string.Format("GCK|1|{0}", myClient.myPlayer.myName));
            myClient.Send("AR6bk");

            myClient.Send("cC+*#$p%i:?!");
            myClient.Send("SLo+");
            myClient.myPlayer.mySpellInventary.SendAllSpells();
            myClient.Send(string.Format("BT{0}", Utilities.Basic.GetActuelTime()));

            if (myClient.myPlayer.Life == 0)
            {
                myClient.myPlayer.UpdateStats();
                myClient.myPlayer.Life = myClient.myPlayer.MaximumLife;
            }

            myClient.myPlayer.myInventary.RefreshBonus();
            myClient.myPlayer.SendPods();
            myClient.myPlayer.SendCharStats();

            myClient.myPlayer.LoadMap();
        }

        public void ChangeChannel(string Chanel)
        {
            var Add = false;

            if (Chanel.Contains("+"))
            {
                Add = true;
                Chanel = Chanel.Replace("+", "");
            }

            else if (Chanel.Contains("-")) 
                Chanel = Chanel.Replace("-", "");
            else 
                return;

            if (Add == true)
            {
                if(!myClient.myPlayer.Channel.Contains(Chanel)) myClient.myPlayer.Channel = myClient.myPlayer.Channel + Chanel;
                myClient.Send("cC+" + Chanel);
            }
            else
            {
                myClient.myPlayer.Channel = myClient.myPlayer.Channel.Replace(Chanel, "");
                myClient.Send("cC-" + Chanel);
            }
        }

        public void ParseChatMessage(string Data)
        {
            string[] SplitData = Data.Split('|');

            var Channel = SplitData[0];
            var Message = SplitData[1];

            switch (Channel)
            {
                case "*":
                    Chat.SendGeneralMessage(myClient, Message);
                    break;
            }

            if (Channel.Length > 1 && Channel != "*")
            {
                Chat.SendPrivateMessage(myClient, Channel, Message);
            }
        }

        public void ParseConsoleMessage(string Data)
        {
            myClient.myCommander.ParseCommand(Data);
        }

        public void GameInformations(string Data)
        {
            myClient.myPlayer.GetMap().AddPlayer(myClient.myPlayer);
            myClient.Send("GDK");
            myClient.Send("fC0"); //Fight
        }

        public void GameAction(string Data)
        {
            var Pack = int.Parse(Data.Substring(0, 3));

            switch (Pack)
            {
                case 1:
                    GameMove(Data);
                    break;
            }
        }

        public void GameMove(string Data)
        {
            var Pack = Data.Substring(3);

            if (!Cells.isValidCell(myClient.myPlayer, Pack) == true)
            {
                myClient.Send("GA;0");
            }

            var Path = new Pathfinding(Pack, myClient.myPlayer.GetMap(), myClient.myPlayer.MapCell, myClient.myPlayer.Dir);
            var NewPath = Path.RemakePath();

            NewPath = Path.GetStartPath + NewPath;

            myClient.myPlayer.Dir = Path.NewDirection;
            myClient.myPlayer.State.MoveCell = Path.Destination;
            myClient.myPlayer.State.OnMove = true;

            myClient.myPlayer.GetMap().Send(string.Format("GA0;1;{0};{1}", myClient.myPlayer.ID, NewPath));
        }

        public void EndAction(string Data)
        {
            switch(Data.Substring(0,1))
            {
                case "K":

                    if (myClient.myPlayer.State.OnMove == true)
                    {
                        myClient.myPlayer.State.OnMove = false;
                        myClient.myPlayer.MapCell = myClient.myPlayer.State.MoveCell;
                        myClient.myPlayer.State.MoveCell = -1;
                        myClient.Send("BN");

                        if (myClient.myPlayer.GetMap().myTriggers.Any(x => x.myCellID == myClient.myPlayer.MapCell))
                        {
                            var m_T = myClient.myPlayer.GetMap().myTriggers.First(x => x.myCellID == myClient.myPlayer.MapCell);
                            myClient.myPlayer.TeleportNewMap(m_T.myNewMapID, m_T.myNewCellID);
                        }
                    }

                    break;

                case "E":

                    int NewCell = int.Parse(Data.Split('|')[1]);
                    myClient.myPlayer.State.OnMove = false;
                    myClient.myPlayer.MapCell = NewCell;

                    break;
            }
        }

        #region Items

        public void DeleteItem(string Data)
        {
            try
            {
                string[] AllData = Data.Split('|');
                if (int.Parse(AllData[1]) <= 0) return;
                myClient.myPlayer.myInventary.DeleteItem(int.Parse(AllData[0]), int.Parse(AllData[1]));
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot delete item from <{0}> because : {1}", myClient.myIp(), e.ToString()));
            }
        }

        public void MoveItem(string Data)
        {
            try
            {
                string[] AllData = Data.Split('|');
                myClient.myPlayer.myInventary.MoveItem(int.Parse(AllData[0]), int.Parse(AllData[1]), (AllData.Length >= 3 ? int.Parse(AllData[2]) : 1));
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot move item from <{0}> because : {1}", myClient.myIp(), e.ToString()));
            }
        }

        public void UseItem(string Data)
        {
            myClient.myPlayer.myInventary.UseItem(Data);
        }

        #endregion

        #region StatsBoosts

        public void StatsBoosts(string Data)
        {
            var Caract = int.Parse(Data);
            var Count = 0;

            switch (Caract)
            {
                case 11:

                    if (myClient.myPlayer.CharactPoint < 1) return;

                    if (myClient.myPlayer.Class == 11)
                    {
                        myClient.myPlayer.myStats.Life.Bases += 2;
                        myClient.myPlayer.Life += 2;
                    }
                    else
                    {
                        myClient.myPlayer.myStats.Life.Bases += 1;
                        myClient.myPlayer.Life += 1;
                    }

                    myClient.myPlayer.CharactPoint -= 1;
                    myClient.myPlayer.SendCharStats();

                    break;

                case 12:

                    if (myClient.myPlayer.CharactPoint < 3) return;

                    myClient.myPlayer.myStats.Wisdom.Bases += 1;
                    myClient.myPlayer.CharactPoint -= 3;
                    myClient.myPlayer.SendCharStats();

                    break;

                case 10:

                    if (myClient.myPlayer.Class == 1 | myClient.myPlayer.Class == 7 | myClient.myPlayer.Class == 2 | myClient.myPlayer.Class == 5)
                    {
                        if (myClient.myPlayer.myStats.Strenght.Bases < 51) Count = 2;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 50) Count = 3;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 150) Count = 4;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 250) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 3 | myClient.myPlayer.Class == 9)
                    {
                        if (myClient.myPlayer.myStats.Strenght.Bases < 51) Count = 1;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 50) Count = 2;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 150) Count = 3;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 250) Count = 4;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 350) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 4 | myClient.myPlayer.Class == 6 | myClient.myPlayer.Class == 8 | myClient.myPlayer.Class == 10)
                    {
                        if (myClient.myPlayer.myStats.Strenght.Bases < 101) Count = 1;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 100) Count = 2;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 200) Count = 3;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 300) Count = 4;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 400) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 11)
                    {
                        Count = 3;
                    }

                    else if (myClient.myPlayer.Class == 12)
                    {
                        if (myClient.myPlayer.myStats.Strenght.Bases < 51) Count = 1;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 50) Count = 2;
                        if (myClient.myPlayer.myStats.Strenght.Bases > 200) Count = 3;
                    }

                    if (myClient.myPlayer.CharactPoint >= Count)
                    {
                        myClient.myPlayer.myStats.Strenght.Bases += 1;
                        myClient.myPlayer.CharactPoint -= Count;
                        myClient.myPlayer.SendCharStats();
                    }
                    else
                        myClient.Send("ABE");

                    break;

                case 15:

                    if (myClient.myPlayer.Class == 1 | myClient.myPlayer.Class == 2 | myClient.myPlayer.Class == 5 | myClient.myPlayer.Class == 7 | myClient.myPlayer.Class == 10)
                    {
                        if (myClient.myPlayer.myStats.Intelligence.Bases < 101) Count = 1;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 100) Count = 2;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 200) Count = 3;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 300) Count = 4;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 400) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 3)
                    {
                        if (myClient.myPlayer.myStats.Intelligence.Bases < 21) Count = 1;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 20) Count = 2;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 60) Count = 3;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 100) Count = 4;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 140) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 4)
                    {
                        if (myClient.myPlayer.myStats.Intelligence.Bases < 51) Count = 1;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 50) Count = 2;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 150) Count = 3;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 250) Count = 4;
                    }

                    else if (myClient.myPlayer.Class == 6 | myClient.myPlayer.Class == 8)
                    {
                        if (myClient.myPlayer.myStats.Intelligence.Bases < 21) Count = 1;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 20) Count = 2;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 40) Count = 3;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 60) Count = 4;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 80) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 9)
                    {
                        if (myClient.myPlayer.myStats.Intelligence.Bases < 51) Count = 1;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 50) Count = 2;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 150) Count = 3;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 250) Count = 4;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 350) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 11)
                    {
                        Count = 3;
                    }

                    else if (myClient.myPlayer.Class == 12)
                    {
                        if (myClient.myPlayer.myStats.Intelligence.Bases < 51) Count = 1;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 50) Count = 2;
                        if (myClient.myPlayer.myStats.Intelligence.Bases > 200) Count = 3;
                    }

                    if (myClient.myPlayer.CharactPoint >= Count)
                    {
                        myClient.myPlayer.myStats.Intelligence.Bases += 1;
                        myClient.myPlayer.CharactPoint -= Count;
                        myClient.myPlayer.SendCharStats();
                    }
                    else
                        myClient.Send("ABE");

                    break;

                case 13:

                    if (myClient.myPlayer.Class == 1 | myClient.myPlayer.Class == 4 | myClient.myPlayer.Class == 5
                        | myClient.myPlayer.Class == 6 | myClient.myPlayer.Class == 7 | myClient.myPlayer.Class == 8 | myClient.myPlayer.Class == 9)
                    {
                        if (myClient.myPlayer.myStats.Luck.Bases < 21) Count = 1;
                        if (myClient.myPlayer.myStats.Luck.Bases > 20) Count = 2;
                        if (myClient.myPlayer.myStats.Luck.Bases > 40) Count = 3;
                        if (myClient.myPlayer.myStats.Luck.Bases > 60) Count = 4;
                        if (myClient.myPlayer.myStats.Luck.Bases > 80) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 2 | myClient.myPlayer.Class == 10)
                    {
                        if (myClient.myPlayer.myStats.Luck.Bases < 101) Count = 1;
                        if (myClient.myPlayer.myStats.Luck.Bases > 100) Count = 2;
                        if (myClient.myPlayer.myStats.Luck.Bases > 200) Count = 3;
                        if (myClient.myPlayer.myStats.Luck.Bases > 300) Count = 4;
                        if (myClient.myPlayer.myStats.Luck.Bases > 400) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 3)
                    {
                        if (myClient.myPlayer.myStats.Luck.Bases < 101) Count = 1;
                        if (myClient.myPlayer.myStats.Luck.Bases > 100) Count = 2;
                        if (myClient.myPlayer.myStats.Luck.Bases > 150) Count = 3;
                        if (myClient.myPlayer.myStats.Luck.Bases > 230) Count = 4;
                        if (myClient.myPlayer.myStats.Luck.Bases > 330) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 11)
                    {
                        Count = 3;
                    }

                    else if (myClient.myPlayer.Class == 12)
                    {
                        if (myClient.myPlayer.myStats.Luck.Bases < 51) Count = 1;
                        if (myClient.myPlayer.myStats.Luck.Bases > 50) Count = 2;
                        if (myClient.myPlayer.myStats.Luck.Bases > 200) Count = 3;
                    }

                    if (myClient.myPlayer.CharactPoint >= Count)
                    {
                        myClient.myPlayer.myStats.Luck.Bases += 1;
                        myClient.myPlayer.CharactPoint -= Count;
                        myClient.myPlayer.SendCharStats();
                    }
                    else
                        myClient.Send("ABE");

                    break;

                case 14:

                    if (myClient.myPlayer.Class == 1 | myClient.myPlayer.Class == 2 | myClient.myPlayer.Class == 3 | myClient.myPlayer.Class == 5
                        | myClient.myPlayer.Class == 7 | myClient.myPlayer.Class == 8 | myClient.myPlayer.Class == 10)
                    {
                        if (myClient.myPlayer.myStats.Agility.Bases < 21) Count = 1;
                        if (myClient.myPlayer.myStats.Agility.Bases > 20) Count = 2;
                        if (myClient.myPlayer.myStats.Agility.Bases > 40) Count = 3;
                        if (myClient.myPlayer.myStats.Agility.Bases > 60) Count = 4;
                        if (myClient.myPlayer.myStats.Agility.Bases > 80) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 4)
                    {
                        if (myClient.myPlayer.myStats.Agility.Bases < 101) Count = 1;
                        if (myClient.myPlayer.myStats.Agility.Bases > 100) Count = 2;
                        if (myClient.myPlayer.myStats.Agility.Bases > 200) Count = 3;
                        if (myClient.myPlayer.myStats.Agility.Bases > 300) Count = 4;
                        if (myClient.myPlayer.myStats.Agility.Bases > 400) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 6 | myClient.myPlayer.Class == 9)
                    {
                        if (myClient.myPlayer.myStats.Agility.Bases < 51) Count = 1;
                        if (myClient.myPlayer.myStats.Agility.Bases > 50) Count = 2;
                        if (myClient.myPlayer.myStats.Agility.Bases > 100) Count = 3;
                        if (myClient.myPlayer.myStats.Agility.Bases > 150) Count = 4;
                        if (myClient.myPlayer.myStats.Agility.Bases > 200) Count = 5;
                    }

                    else if (myClient.myPlayer.Class == 11)
                    {
                        Count = 3;
                    }

                    else if (myClient.myPlayer.Class == 12)
                    {
                        if (myClient.myPlayer.myStats.Agility.Bases < 51) Count = 1;
                        if (myClient.myPlayer.myStats.Agility.Bases > 50) Count = 2;
                        if (myClient.myPlayer.myStats.Agility.Bases > 200) Count = 3;
                    }

                    if (myClient.myPlayer.CharactPoint >= Count)
                    {
                        myClient.myPlayer.myStats.Agility.Bases += 1;
                        myClient.myPlayer.CharactPoint -= Count;
                        myClient.myPlayer.SendCharStats();
                    }
                    else
                        myClient.Send("ABE");

                    break;
            }
        }

        #endregion

        #endregion
    }
}
