using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.Character;
using realm.Realm.Map;
using realm.Realm.Character.Stats;
using realm.Realm;
using realm.Realm.World;

namespace realm.Client
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
            m_Packets["AL"] = SendCharacterList;
            m_Packets["AP"] = SendRandomName;
            m_Packets["AS"] = SelectCharacter;
            m_Packets["AT"] = ParseTicket;
            m_Packets["AV"] = AV_Packet;
            m_Packets["BD"] = SendDate;
            m_Packets["BM"] = ParseChatMessage;
            m_Packets["cC"] = ChangeChannel;
            m_Packets["GA"] = GameAction;
            m_Packets["GC"] = CreateGame;
            m_Packets["GI"] = GameInformations;
            m_Packets["GK"] = EndAction;
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
            foreach (Network.SelectorKeys Key in Network.SelectorKeys.m_Keys)
            {
                if (Key.m_Key == Data)
                {
                    Client.m_Infos = Key.m_Infos;
                    Client.m_Infos.ParseCharacters();
                    Client.ParseCharacters();

                    Client.isAuth = true;

                    Program.m_RealmLink.Send("NC|" + Client.m_Infos.Pseudo);
                    Client.Send("ATK0");
                }
            }
        }

        #endregion
        
        #region Character

        public void SendRandomName(string test)
        {
            Client.Send("APK" + SunDofus.Basic.RandomName());
        }

        public void AV_Packet(string t)
        {
            Client.Send("AV0");
        }

        public void SendCharacterList(string test)
        {
            string Pack = "ALK" + Client.m_Infos.Subscription +"|" + Client.m_Infos.CharactersNames.Count;

            if (Client.m_Infos.CharactersNames.Count != 0)
            {
                foreach (Realm.Character.Character m_C in Client.m_Characters)
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
                    m_Character.ID = Database.Data.CharacterSql.GetNewID();
                    m_Character.m_Name = CharData[0];
                    m_Character.Level = 1;
                    m_Character.Class = int.Parse(CharData[1]);
                    m_Character.Sex = int.Parse(CharData[2]);
                    m_Character.Skin = int.Parse(m_Character.Class + "" + m_Character.Sex);
                    m_Character.Size = 100;
                    m_Character.Color = int.Parse(CharData[3]);
                    m_Character.Color2 = int.Parse(CharData[4]);
                    m_Character.Color3 = int.Parse(CharData[5]);

                    m_Character.MapID = 10111;
                    m_Character.MapCell = 255;
                    m_Character.Dir = 3;

                    m_Character.NewCharacter = true;

                    if (m_Character.Class < 1 | m_Character.Class > 12 | m_Character.Sex < 0 | m_Character.Sex > 1)
                    {
                        Client.Send("AAE");
                        return;
                    }

                    CharactersManager.CharactersList.Add(m_Character);
                    Client.m_Characters.Add(m_Character);

                    Program.m_RealmLink.Send("NCHAR|" + Client.m_Infos.Id + "|" + Client.m_Infos.AddNewCharacterToAccount(m_Character.m_Name));
                    Database.Data.CharacterSql.CreateCharacter(m_Character);

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
                SunDofus.Logger.Error(e);
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

            Program.m_RealmLink.Send("NCHAR|" + Client.m_Infos.Id + "|" + Client.m_Infos.RemoveCharacterToAccount(m_C.m_Name));
            Database.Data.CharacterSql.DeleteCharacter(m_C.m_Name);

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

        #region Realm

        void SendDate(string t)
        {
            Client.Send("BD" + SunDofus.Basic.GetDofusDate());
        }

        public void CreateGame(string t)
        {
            Client.Send("GCK|1|" + Client.m_Player.m_Name);
            Client.Send("AR6bk");

            Client.Send("cC+*#$p%i:?!");
            Client.Send("SLo+");
            Client.Send("BT" + SunDofus.Basic.GetActuelTime());

            Client.m_Player.SendCharStats();
            Client.m_Player.SendPods();

            Client.m_Player.LoadMap();
        }

        public void ChangeChannel(string Chanel)
        {
            bool Add = false;
            if (Chanel.Contains("+")) Add = true;
            else if (Chanel.Contains("-")) Add = false;
            else return;

            if (Add == true)
            {
                if (Client.m_Player.Channel.Contains(Chanel))
                {
                    Client.Send("cC" + Client.m_Player.Channel);
                    return;
                }
                Client.m_Player.Channel = Client.m_Player.Channel + "" + Chanel;
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
            string[] AllData = Data.Split(' ');
            Client.m_Commander.ParseCommand(AllData[0], AllData[1]);
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

                        if (Client.m_Player.GetMap().m_Triggers.Count(x => x.CellID == Client.m_Player.MapCell) > 0)
                        {
                            Trigger m_T = Client.m_Player.GetMap().m_Triggers.First(x => x.CellID == Client.m_Player.MapCell);
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
                        Client.m_Player.m_Stats.Vitalite.Bases += 2;
                        Client.m_Player.Life += 2;
                    }
                    else
                    {
                        Client.m_Player.m_Stats.Vitalite.Bases += 1;
                        Client.m_Player.Life += 1;
                    }

                    Client.m_Player.CharactPoint -= 1;
                    Client.m_Player.SendCharStats();

                    break;

                case 12:

                    if (Client.m_Player.CharactPoint < 3) return;

                    Client.m_Player.m_Stats.Sagesse.Bases += 1;
                    Client.m_Player.CharactPoint -= 3;
                    Client.m_Player.SendCharStats();

                    break;

                case 10:

                    if (Client.m_Player.Class == 1 | Client.m_Player.Class == 7 | Client.m_Player.Class == 2 | Client.m_Player.Class == 5)
                    {
                        if (Client.m_Player.m_Stats.Force.Bases < 51) Count = 2;
                        if (Client.m_Player.m_Stats.Force.Bases > 50) Count = 3;
                        if (Client.m_Player.m_Stats.Force.Bases > 150) Count = 4;
                        if (Client.m_Player.m_Stats.Force.Bases > 250) Count = 5;
                    }

                    else if (Client.m_Player.Class == 3 | Client.m_Player.Class == 9)
                    {
                        if (Client.m_Player.m_Stats.Force.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Force.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Force.Bases > 150) Count = 3;
                        if (Client.m_Player.m_Stats.Force.Bases > 250) Count = 4;
                        if (Client.m_Player.m_Stats.Force.Bases > 350) Count = 5;
                    }

                    else if (Client.m_Player.Class == 4 | Client.m_Player.Class == 6 | Client.m_Player.Class == 8 | Client.m_Player.Class == 10)
                    {
                        if (Client.m_Player.m_Stats.Force.Bases < 101) Count = 1;
                        if (Client.m_Player.m_Stats.Force.Bases > 100) Count = 2;
                        if (Client.m_Player.m_Stats.Force.Bases > 200) Count = 3;
                        if (Client.m_Player.m_Stats.Force.Bases > 300) Count = 4;
                        if (Client.m_Player.m_Stats.Force.Bases > 400) Count = 5;
                    }

                    else if (Client.m_Player.Class == 11)
                    {
                        Count = 3;
                    }

                    else if (Client.m_Player.Class == 12)
                    {
                        if (Client.m_Player.m_Stats.Force.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Force.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Force.Bases > 200) Count = 3;
                    }

                    if (Client.m_Player.CharactPoint >= Count)
                    {
                        Client.m_Player.m_Stats.Force.Bases += 1;
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
                    }
                    else
                        Client.Send("ABE");

                    break;

                case 13:

                    if (Client.m_Player.Class == 1 | Client.m_Player.Class == 4 | Client.m_Player.Class == 5
                        | Client.m_Player.Class == 6 | Client.m_Player.Class == 7 | Client.m_Player.Class == 8 | Client.m_Player.Class == 9)
                    {
                        if (Client.m_Player.m_Stats.Chance.Bases < 21) Count = 1;
                        if (Client.m_Player.m_Stats.Chance.Bases > 20) Count = 2;
                        if (Client.m_Player.m_Stats.Chance.Bases > 40) Count = 3;
                        if (Client.m_Player.m_Stats.Chance.Bases > 60) Count = 4;
                        if (Client.m_Player.m_Stats.Chance.Bases > 80) Count = 5;
                    }

                    else if (Client.m_Player.Class == 2 | Client.m_Player.Class == 10)
                    {
                        if (Client.m_Player.m_Stats.Chance.Bases < 101) Count = 1;
                        if (Client.m_Player.m_Stats.Chance.Bases > 100) Count = 2;
                        if (Client.m_Player.m_Stats.Chance.Bases > 200) Count = 3;
                        if (Client.m_Player.m_Stats.Chance.Bases > 300) Count = 4;
                        if (Client.m_Player.m_Stats.Chance.Bases > 400) Count = 5;
                    }

                    else if (Client.m_Player.Class == 3)
                    {
                        if (Client.m_Player.m_Stats.Chance.Bases < 101) Count = 1;
                        if (Client.m_Player.m_Stats.Chance.Bases > 100) Count = 2;
                        if (Client.m_Player.m_Stats.Chance.Bases > 150) Count = 3;
                        if (Client.m_Player.m_Stats.Chance.Bases > 230) Count = 4;
                        if (Client.m_Player.m_Stats.Chance.Bases > 330) Count = 5;
                    }

                    else if (Client.m_Player.Class == 11)
                    {
                        Count = 3;
                    }

                    else if (Client.m_Player.Class == 12)
                    {
                        if (Client.m_Player.m_Stats.Chance.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Chance.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Chance.Bases > 200) Count = 3;
                    }

                    if (Client.m_Player.CharactPoint >= Count)
                    {
                        Client.m_Player.m_Stats.Chance.Bases += 1;
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
                        if (Client.m_Player.m_Stats.Agilite.Bases < 21) Count = 1;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 20) Count = 2;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 40) Count = 3;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 60) Count = 4;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 80) Count = 5;
                    }

                    else if (Client.m_Player.Class == 4)
                    {
                        if (Client.m_Player.m_Stats.Agilite.Bases < 101) Count = 1;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 100) Count = 2;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 200) Count = 3;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 300) Count = 4;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 400) Count = 5;
                    }

                    else if (Client.m_Player.Class == 6 | Client.m_Player.Class == 9)
                    {
                        if (Client.m_Player.m_Stats.Agilite.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 100) Count = 3;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 150) Count = 4;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 200) Count = 5;
                    }

                    else if (Client.m_Player.Class == 11)
                    {
                        Count = 3;
                    }

                    else if (Client.m_Player.Class == 12)
                    {
                        if (Client.m_Player.m_Stats.Agilite.Bases < 51) Count = 1;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 50) Count = 2;
                        if (Client.m_Player.m_Stats.Agilite.Bases > 200) Count = 3;
                    }

                    if (Client.m_Player.CharactPoint >= Count)
                    {
                        Client.m_Player.m_Stats.Agilite.Bases += 1;
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
