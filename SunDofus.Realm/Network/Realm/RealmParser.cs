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
        public RealmClient Client;

        delegate void Packets(string s);
        Dictionary<string, Packets> m_Packets;

        public RealmParser(RealmClient m_C)
        {
            Client = m_C;
            m_Packets = new Dictionary<string, Packets>();
            RegisterPackets();
        }

        void RegisterPackets()
        {
            m_Packets["AA"] = CreateCharacter;
            m_Packets["AB"] = StatsBoosts;
            m_Packets["AD"] = DeleteCharacter;
            m_Packets["Ag"] = SendGifts;
            m_Packets["AG"] = AcceptGift;
            m_Packets["AL"] = SendCharacterList;
            m_Packets["AP"] = SendRandomName;
            m_Packets["AS"] = SelectCharacter;
            m_Packets["AT"] = ParseTicket;
            m_Packets["AV"] = AV_Packet;
            m_Packets["BA"] = ParseConsoleMessage;
            m_Packets["BD"] = SendDate;
            m_Packets["BM"] = ParseChatMessage;
            m_Packets["cC"] = ChangeChannel;
            m_Packets["GA"] = GameAction;
            m_Packets["GC"] = CreateGame;
            m_Packets["GI"] = GameInformations;
            m_Packets["GK"] = EndAction;
            m_Packets["Od"] = DeleteItem;
            m_Packets["OM"] = MoveItem;
            m_Packets["OU"] = UseItem;
        }

        public void Parse(string Data)
        {
            if (Data == "ping")
                Client.Send("pong");
            else if (Data == "qping")
                Client.Send("qpong");

            if (Data.Length < 2) return;

            string Header = Data.Substring(0, 2);

            if (!m_Packets.ContainsKey(Header))
            {
                Client.Send("BN");
                return;
            }

            m_Packets[Header](Data.Substring(2));
        }

        #region Ticket

        public void ParseTicket(string Data)
        {
            Data = Data.Replace("AT", "");
            if (Network.Authentication.AuthenticationKeys.m_Keys.Any(x => x.m_Key == Data))
            {
                Network.Authentication.AuthenticationKeys Key = Network.Authentication.AuthenticationKeys.m_Keys.First(x => x.m_Key == Data);
                Client.m_Infos = Key.m_Infos;
                Client.m_Infos.ParseCharacters();
                Client.ParseCharacters();

                Client.isAuth = true;

                Network.ServersHandler.myAuthLink.Send("NC|" + Client.m_Infos.Pseudo);
                Client.Send("ATK0");
            }
            else
                Client.Send("ATE");
        }

        #endregion
        
        #region Character

        public void SendRandomName(string test)
        {
            Client.Send("APK" + Utilities.Basic.RandomName());
        }

        public void AV_Packet(string t)
        {
            Client.Send("AV0");
        }

        public void SendCharacterList(string test)
        {
            string Pack = "ALK" + (Client.m_Infos.Subscription * 1000) +"|" + Client.m_Infos.myCharacters.Count;

            if (Client.m_Infos.myCharacters.Count != 0)
            {
                foreach (realm.Realm.Character.Character m_C in Client.m_Characters)
                {
                    Pack += "|" + m_C.PatternList();
                }
            }

            Client.Send(Pack);
        }

        public void CreateCharacter(string Packet)
        {
            try
            {
                string[] CharData = Packet.Split('|');

                if (CharData[0] != "" | CharactersManager.ExistsName(CharData[0]) == false)
                {
                    Character m_Character = new Character();
                    m_Character.ID = Database.Cache.CharactersCache.GetNewID();
                    m_Character.m_Name = CharData[0];
                    //m_Character.Level = Config.ConfigurationManager.GetInt("Start_Level");
                    //m_Character.Class = int.Parse(CharData[1]);
                    //m_Character.Sex = int.Parse(CharData[2]);
                    //m_Character.Skin = int.Parse(m_Character.Class + "" + m_Character.Sex);
                    //m_Character.Size = 100;
                    //m_Character.Color = int.Parse(CharData[3]);
                    //m_Character.Color2 = int.Parse(CharData[4]);
                    //m_Character.Color3 = int.Parse(CharData[5]);

                    //m_Character.MapID = Config.ConfigurationManager.GetInt("Start_Map");
                    //m_Character.MapCell = Config.ConfigurationManager.GetInt("Start_Cell");
                    //m_Character.Dir = Config.ConfigurationManager.GetInt("Start_Dir");

                    m_Character.CharactPoint = (m_Character.Level - 1) * 5;
                    m_Character.SpellPoint = (m_Character.Level - 1);

                    m_Character.NewCharacter = true;

                    if (m_Character.Class < 1 | m_Character.Class > 12 | m_Character.Sex < 0 | m_Character.Sex > 1)
                    {
                        Client.Send("AAE");
                        return;
                    }

                    m_Character.m_SpellInventary.LearnSpells();

                    Database.Cache.CharactersCache.CreateCharacter(m_Character);
                    CharactersManager.CharactersList.Add(m_Character);
                    Client.m_Characters.Add(m_Character);

                    Network.ServersHandler.myAuthLink.Send("NCHAR|" + Client.m_Infos.Id + "|" + Client.m_Infos.AddNewCharacterToAccount(m_Character.m_Name));

                    Client.Send("TB");
                    Client.Send("AAK");
                    SendCharacterList("");
                }
                else
                {
                    Client.Send("AAE");
                }
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(e.ToString());
            }
        }

        public void DeleteCharacter(string Packet)
        {
            int ID = int.Parse(Packet.Split('|')[0]);
            Character m_C = CharactersManager.CharactersList.First(x => x.ID == ID);
            if (Packet.Split('|')[1] != Client.m_Infos.Answer && m_C.Level >= 20)
            {
                Client.Send("ADE");
                return;
            }

            CharactersManager.CharactersList.Remove(m_C);
            Client.m_Characters.Remove(m_C);

            Network.ServersHandler.myAuthLink.Send("NCHAR|" + Client.m_Infos.Id + "|" + Client.m_Infos.RemoveCharacterToAccount(m_C.m_Name));
            Database.Cache.CharactersCache.DeleteCharacter(m_C.m_Name);

            SendCharacterList("");
        }

        public void SelectCharacter(string Packet)
        {
            Character m_C = CharactersManager.CharactersList.First(x => x.ID == int.Parse(Packet));
            if (Client.m_Characters.Contains(m_C))
            {
                Client.m_Player = m_C;
                Client.m_Player.State = new CharacterState(Client.m_Player);
                Client.m_Player.Client = Client;

                Client.m_Player.isConnected = true;

                Client.Send("ASK" + Client.m_Player.PatternSelect());
            }
            else
                Client.Send("ASE");
        }

        #endregion

        #region Gift

        public void SendGifts(string test)
        {
            Client.SendGifts();
        }

        public void AcceptGift(string ID)
        {
            try
            {
                string[] Infos = ID.Split('|');
                if (Client.m_Characters.Any(x => x.ID == int.Parse(Infos[1])))
                {
                    if (Client.m_Infos.myGifts.Any(x => x.id == int.Parse(Infos[0])))
                    {
                        Database.Models.Clients.GiftModel myGift = Client.m_Infos.myGifts.First(e => e.id == int.Parse(Infos[0]));
                        Client.m_Characters.First(x => x.ID == int.Parse(Infos[1])).m_Inventary.AddItem(myGift.item, true);

                        Client.Send("AG0");
                        Network.ServersHandler.myAuthLink.Send("DG|" + myGift.id + "|" + Client.m_Infos.Id);
                        Client.m_Infos.myGifts.Remove(myGift);

                    }
                    else
                        Client.Send("AGE");
                }
                else
                    Client.Send("AGE");
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(e.ToString());
            }
        }

        #endregion

        #region Realm

        void SendDate(string t)
        {
            Client.Send("BD" + Utilities.Basic.GetDofusDate());
        }

        public void CreateGame(string t)
        {
            Client.Send("GCK|1|" + Client.m_Player.m_Name);
            Client.Send("AR6bk");

            Client.Send("cC+*#$p%i:?!");
            Client.Send("SLo+");
            Client.m_Player.m_SpellInventary.SendAllSpells();
            Client.Send("BT" + Utilities.Basic.GetActuelTime());

            if (Client.m_Player.Life == 0)
            {
                Client.m_Player.UpdateStats();
                Client.m_Player.Life = Client.m_Player.MaximumLife;
            }

            Client.m_Player.m_Inventary.RefreshBonus();
            Client.m_Player.SendPods();
            Client.m_Player.SendCharStats();

            Client.m_Player.LoadMap();
        }

        public void ChangeChannel(string Chanel)
        {
            bool Add = false;
            if (Chanel.Contains("+"))
            {
                Add = true;
                Chanel = Chanel.Replace("+", "");
            }
            else if (Chanel.Contains("-")) 
                Chanel = Chanel.Replace("-", "");
            else return;

            if (Add == true)
            {
                if(!Client.m_Player.Channel.Contains(Chanel)) Client.m_Player.Channel = Client.m_Player.Channel + "" + Chanel;
                Client.Send("cC+" + Chanel);
            }
            else
            {
                Client.m_Player.Channel = Client.m_Player.Channel.Replace(Chanel, "");
                Client.Send("cC-" + Chanel);
            }
        }

        public void ParseChatMessage(string Data)
        {
            string[] SplitData = Data.Split('|');
            string Channel = SplitData[0];
            string Message = SplitData[1];

            switch (Channel)
            {
                case "*":
                    Chat.SendGeneralMessage(Client, Message);
                    break;
            }

            if (Channel.Length > 1 && Channel != "*")
            {
                Chat.SendPrivateMessage(Client, Channel, Message);
            }
        }

        public void ParseConsoleMessage(string Data)
        {
            Client.m_Commander.ParseCommand(Data);
        }

        public void GameInformations(string Data)
        {
            Client.m_Player.GetMap().AddPlayer(Client.m_Player);
            Client.Send("GDK");
            Client.Send("fC0"); //Fight
        }

        public void GameAction(string Data)
        {
            int Pack = int.Parse(Data.Substring(0, 3));
            switch (Pack)
            {
                case 1:
                    GameMove(Data);
                    break;
            }
        }

        public void GameMove(string Data)
        {
            string Pack = Data.Substring(3);
            if (!Cells.isValidCell(Client.m_Player, Pack) == true)
            {
                Client.Send("GA;0");
            }

            Pathfinding Path = new Pathfinding(Pack, Client.m_Player.GetMap(), Client.m_Player.MapCell, Client.m_Player.Dir);
            string NewPath = Path.RemakePath();
            NewPath = Path.GetStartPath + NewPath;

            Client.m_Player.Dir = Path.NewDirection;
            Client.m_Player.State.MoveCell = Path.Destination;
            Client.m_Player.State.OnMove = true;

            Client.m_Player.GetMap().Send("GA0;1;" + Client.m_Player.ID + ";" + NewPath);
        }

        public void EndAction(string Data)
        {
            switch(Data.Substring(0,1))
            {
                case "K":
                    if (Client.m_Player.State.OnMove == true)
                    {
                        Client.m_Player.State.OnMove = false;
                        Client.m_Player.MapCell = Client.m_Player.State.MoveCell;
                        Client.m_Player.State.MoveCell = -1;
                        Client.Send("BN");

                        if (Client.m_Player.GetMap().myTriggers.Any(x => x.CellID == Client.m_Player.MapCell))
                        {
                            Database.Models.Maps.TriggerModel m_T = Client.m_Player.GetMap().myTriggers.First(x => x.CellID == Client.m_Player.MapCell);
                            Client.m_Player.TeleportNewMap(m_T.NewMapID, m_T.NewCellID);
                        }
                    }
                    break;

                case "E":
                    int NewCell = int.Parse(Data.Split('|')[1]);
                    Client.m_Player.State.OnMove = false;
                    Client.m_Player.MapCell = NewCell;
                    break;
            }
        }

        #region Items

        public void DeleteItem(string Data)
        {
            string[] AllData = Data.Split('|');
            if (int.Parse(AllData[1]) <= 0) return;
            Client.m_Player.m_Inventary.DeleteItem(int.Parse(AllData[0]), int.Parse(AllData[1]));
        }

        public void MoveItem(string Data)
        {
            string[] AllData = Data.Split('|');
            Client.m_Player.m_Inventary.MoveItem(int.Parse(AllData[0]), int.Parse(AllData[1]), (AllData.Length >= 3 ? int.Parse(AllData[2]) : 1));
        }

        public void UseItem(string Data)
        {
            Client.m_Player.m_Inventary.UseItem(Data);
        }

        #endregion

        #region StatsBoosts

        public void StatsBoosts(string Data)
        {
            int Caract = int.Parse(Data);
            int Count = 0;

            switch (Caract)
            {
                case 11:

                    if (Client.m_Player.CharactPoint < 1) return;

                    if (Client.m_Player.Class == 11)
                    {
                        Client.m_Player.m_Stats.Life.Bases += 2;
                        Client.m_Player.Life += 2;
                    }
                    else
                    {
                        Client.m_Player.m_Stats.Life.Bases += 1;
                        Client.m_Player.Life += 1;
                    }

                    Client.m_Player.CharactPoint -= 1;
                    Client.m_Player.SendCharStats();

                    break;

                case 12:

                    if (Client.m_Player.CharactPoint < 3) return;

                    Client.m_Player.m_Stats.Wisdom.Bases += 1;
                    Client.m_Player.CharactPoint -= 3;
                    Client.m_Player.SendCharStats();

                    break;

                case 10:

                    if (Client.m_Player.Class == 1 | Client.m_Player.Class == 7 | Client.m_Player.Class == 2 | Client.m_Player.Class == 5)
                    {
                        if (Client.m_Player.m_Stats.Strenght.Bases < 51) Count = 2;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 50) Count = 3;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 150) Count = 4;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 250) Count = 5;
                    }

                    else if (Client.m_Player.Class == 3 | Client.m_Player.Class == 9)
                    {
                        if (Client.m_Player.m_Stats.Strenght.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 150) Count = 3;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 250) Count = 4;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 350) Count = 5;
                    }

                    else if (Client.m_Player.Class == 4 | Client.m_Player.Class == 6 | Client.m_Player.Class == 8 | Client.m_Player.Class == 10)
                    {
                        if (Client.m_Player.m_Stats.Strenght.Bases < 101) Count = 1;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 100) Count = 2;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 200) Count = 3;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 300) Count = 4;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 400) Count = 5;
                    }

                    else if (Client.m_Player.Class == 11)
                    {
                        Count = 3;
                    }

                    else if (Client.m_Player.Class == 12)
                    {
                        if (Client.m_Player.m_Stats.Strenght.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Strenght.Bases > 200) Count = 3;
                    }

                    if (Client.m_Player.CharactPoint >= Count)
                    {
                        Client.m_Player.m_Stats.Strenght.Bases += 1;
                        Client.m_Player.CharactPoint -= Count;
                        Client.m_Player.SendCharStats();
                    }
                    else
                        Client.Send("ABE");

                    break;

                case 15:

                    if (Client.m_Player.Class == 1 | Client.m_Player.Class == 2 | Client.m_Player.Class == 5 | Client.m_Player.Class == 7 | Client.m_Player.Class == 10)
                    {
                        if (Client.m_Player.m_Stats.Intelligence.Bases < 101) Count = 1;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 100) Count = 2;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 200) Count = 3;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 300) Count = 4;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 400) Count = 5;
                    }

                    else if (Client.m_Player.Class == 3)
                    {
                        if (Client.m_Player.m_Stats.Intelligence.Bases < 21) Count = 1;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 20) Count = 2;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 60) Count = 3;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 100) Count = 4;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 140) Count = 5;
                    }

                    else if (Client.m_Player.Class == 4)
                    {
                        if (Client.m_Player.m_Stats.Intelligence.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 150) Count = 3;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 250) Count = 4;
                    }

                    else if (Client.m_Player.Class == 6 | Client.m_Player.Class == 8)
                    {
                        if (Client.m_Player.m_Stats.Intelligence.Bases < 21) Count = 1;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 20) Count = 2;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 40) Count = 3;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 60) Count = 4;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 80) Count = 5;
                    }

                    else if (Client.m_Player.Class == 9)
                    {
                        if (Client.m_Player.m_Stats.Intelligence.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 150) Count = 3;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 250) Count = 4;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 350) Count = 5;
                    }

                    else if (Client.m_Player.Class == 11)
                    {
                        Count = 3;
                    }

                    else if (Client.m_Player.Class == 12)
                    {
                        if (Client.m_Player.m_Stats.Intelligence.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Intelligence.Bases > 200) Count = 3;
                    }

                    if (Client.m_Player.CharactPoint >= Count)
                    {
                        Client.m_Player.m_Stats.Intelligence.Bases += 1;
                        Client.m_Player.CharactPoint -= Count;
                        Client.m_Player.SendCharStats();
                    }
                    else
                        Client.Send("ABE");

                    break;

                case 13:

                    if (Client.m_Player.Class == 1 | Client.m_Player.Class == 4 | Client.m_Player.Class == 5
                        | Client.m_Player.Class == 6 | Client.m_Player.Class == 7 | Client.m_Player.Class == 8 | Client.m_Player.Class == 9)
                    {
                        if (Client.m_Player.m_Stats.Luck.Bases < 21) Count = 1;
                        if (Client.m_Player.m_Stats.Luck.Bases > 20) Count = 2;
                        if (Client.m_Player.m_Stats.Luck.Bases > 40) Count = 3;
                        if (Client.m_Player.m_Stats.Luck.Bases > 60) Count = 4;
                        if (Client.m_Player.m_Stats.Luck.Bases > 80) Count = 5;
                    }

                    else if (Client.m_Player.Class == 2 | Client.m_Player.Class == 10)
                    {
                        if (Client.m_Player.m_Stats.Luck.Bases < 101) Count = 1;
                        if (Client.m_Player.m_Stats.Luck.Bases > 100) Count = 2;
                        if (Client.m_Player.m_Stats.Luck.Bases > 200) Count = 3;
                        if (Client.m_Player.m_Stats.Luck.Bases > 300) Count = 4;
                        if (Client.m_Player.m_Stats.Luck.Bases > 400) Count = 5;
                    }

                    else if (Client.m_Player.Class == 3)
                    {
                        if (Client.m_Player.m_Stats.Luck.Bases < 101) Count = 1;
                        if (Client.m_Player.m_Stats.Luck.Bases > 100) Count = 2;
                        if (Client.m_Player.m_Stats.Luck.Bases > 150) Count = 3;
                        if (Client.m_Player.m_Stats.Luck.Bases > 230) Count = 4;
                        if (Client.m_Player.m_Stats.Luck.Bases > 330) Count = 5;
                    }

                    else if (Client.m_Player.Class == 11)
                    {
                        Count = 3;
                    }

                    else if (Client.m_Player.Class == 12)
                    {
                        if (Client.m_Player.m_Stats.Luck.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Luck.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Luck.Bases > 200) Count = 3;
                    }

                    if (Client.m_Player.CharactPoint >= Count)
                    {
                        Client.m_Player.m_Stats.Luck.Bases += 1;
                        Client.m_Player.CharactPoint -= Count;
                        Client.m_Player.SendCharStats();
                    }
                    else
                        Client.Send("ABE");

                    break;

                case 14:

                    if (Client.m_Player.Class == 1 | Client.m_Player.Class == 2 | Client.m_Player.Class == 3 | Client.m_Player.Class == 5
                        | Client.m_Player.Class == 7 | Client.m_Player.Class == 8 | Client.m_Player.Class == 10)
                    {
                        if (Client.m_Player.m_Stats.Agility.Bases < 21) Count = 1;
                        if (Client.m_Player.m_Stats.Agility.Bases > 20) Count = 2;
                        if (Client.m_Player.m_Stats.Agility.Bases > 40) Count = 3;
                        if (Client.m_Player.m_Stats.Agility.Bases > 60) Count = 4;
                        if (Client.m_Player.m_Stats.Agility.Bases > 80) Count = 5;
                    }

                    else if (Client.m_Player.Class == 4)
                    {
                        if (Client.m_Player.m_Stats.Agility.Bases < 101) Count = 1;
                        if (Client.m_Player.m_Stats.Agility.Bases > 100) Count = 2;
                        if (Client.m_Player.m_Stats.Agility.Bases > 200) Count = 3;
                        if (Client.m_Player.m_Stats.Agility.Bases > 300) Count = 4;
                        if (Client.m_Player.m_Stats.Agility.Bases > 400) Count = 5;
                    }

                    else if (Client.m_Player.Class == 6 | Client.m_Player.Class == 9)
                    {
                        if (Client.m_Player.m_Stats.Agility.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Agility.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Agility.Bases > 100) Count = 3;
                        if (Client.m_Player.m_Stats.Agility.Bases > 150) Count = 4;
                        if (Client.m_Player.m_Stats.Agility.Bases > 200) Count = 5;
                    }

                    else if (Client.m_Player.Class == 11)
                    {
                        Count = 3;
                    }

                    else if (Client.m_Player.Class == 12)
                    {
                        if (Client.m_Player.m_Stats.Agility.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Agility.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Agility.Bases > 200) Count = 3;
                    }

                    if (Client.m_Player.CharactPoint >= Count)
                    {
                        Client.m_Player.m_Stats.Agility.Bases += 1;
                        Client.m_Player.CharactPoint -= Count;
                        Client.m_Player.SendCharStats();
                    }
                    else
                        Client.Send("ABE");

                    break;
            }
        }

        #endregion

        #endregion
    }
}
