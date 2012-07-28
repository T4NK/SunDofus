using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.Character;
using realm.Realm.Map;

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
            long MemberTime = 60 * 60 * 24 * 365;
            string Pack = "ALK" + (MemberTime * 1000) + "|" + Client.m_Infos.CharactersNames.Count;

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
                    m_Character.Name = CharData[0];
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

                    CharactersManager.ListOfCharacters.Add(m_Character);
                    Client.m_Characters.Add(m_Character);

                    Program.m_RealmLink.Send("NCHAR|" + Client.m_Infos.Id + "|" + Client.m_Infos.AddNewCharacterToAccount(m_Character.Name));
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
            Character m_C = CharactersManager.ListOfCharacters.First(x => x.ID == ID);
            if (Packet.Split('|')[1] != Client.m_Infos.Answer && m_C.Level > 19)
            {
                Client.Send("ADE");
                return;
            }

            CharactersManager.ListOfCharacters.Remove(m_C);
            Client.m_Characters.Remove(m_C);

            Program.m_RealmLink.Send("NCHAR|" + Client.m_Infos.Id + "|" + Client.m_Infos.RemoveCharacterToAccount(m_C.Name));
            Database.Data.CharacterSql.DeleteCharacter(m_C.Name);

            SendCharacterList("");
        }

        public void SelectCharacter(string Packet)
        {
            Character m_C = CharactersManager.ListOfCharacters.First(x => x.ID == int.Parse(Packet));
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
            Client.Send("eL-1|"); // Emote
            Client.Send("GCK|1|" + Client.m_Player.Name);
            Client.Send("AR6bk");

            if (Client.m_Player.State.Created == false)
            {
                Client.m_Player.State.Created = true;
                Client.Send("cC+*#$p%i:?!");
                Client.Send("SLo+");
                Client.Send("BT" + SunDofus.Basic.GetActuelTime());
            }
            else
            {

            }

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
                    Client.m_Player.GetMap().Send("cMK|" + Client.m_Player.ID + "|" + Client.m_Player.Name + "|" + Message);
                    break;
            }

            if (Channel.Length > 1 && Channel != "*")
            {
                Character m_C = CharactersManager.ListOfCharacters.First(x => x.Name == Channel);
                if (m_C.isConnected == true)
                {
                    m_C.Client.Send("cMKF|" + Client.m_Player.ID + "|" + Client.m_Player.Name + "|" + Message);
                    Client.Send("cMKT|" + Client.m_Player.ID + "|" + m_C.Name + "|" + Message);
                }
                else
                {
                    Client.Send("cMEf" + Channel);
                }
            }
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

                        if (Client.m_Player.GetMap().ContainsTrigger(Client.m_Player.MapCell) == true)
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

        #endregion
    }
}
