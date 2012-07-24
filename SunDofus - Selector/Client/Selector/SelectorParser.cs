using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace selector.Client
{
    class SelectorParser
    {
        public SelectorClient Client;

        public SelectorParser(SelectorClient m_C)
        {
            Client = m_C;
        }

        public void Parse(string Data)
        {
            switch (Client.m_State)
            {
                case SelectorClient.State.Version:
                    Version(Data);
                    break;

                case SelectorClient.State.Account:
                    Account(Data);
                    break;

                case SelectorClient.State.Queue:
                    //Juste une grosse flemme(FR)
                    break;

                case SelectorClient.State.OnList:
                    Server(Data);
                    break;
            }
        }

        public void Version(string Packet)
        {
            if (Packet.Contains(Config.ConfigurationManager.GetString("Auth_Version")))
            {
                Client.m_State = SelectorClient.State.Account;
            }
            else
            {
                Client.Send("AlEv" + Config.ConfigurationManager.GetString("Auth_Version"));
                Client.m_State = SelectorClient.State.None;
            }
        }

        public void Account(string Packet)
        {
            if (!Packet.Contains("#1")) return;
            string[] Infos = Packet.Split('#');
            string Username = Infos[0].Replace(Config.ConfigurationManager.GetString("Auth_Version"), "");
            string Password = Infos[1];

            Database.Data.Account AccountRequested = new Database.Data.Account(Username);
            if (Password == Utils.Basic.Encrypt(AccountRequested.Password, Client.m_Key))
            {
                Client.m_Account = AccountRequested;
                Utils.Logger.Infos("Client '" + AccountRequested.Pseudo + "' authentified !");
                Client.m_State = SelectorClient.State.OnList;
                Client.SendInformations();
            }
            else
            {
                Client.Send("AlEx");
                Client.m_State = SelectorClient.State.None;
            }
        }

        public void Server(string Packet)
        {
            if (Packet.StartsWith("Ax"))
            {
                long MemberTime = 60 * 60 * 24 * 365;
                string Pack = "AxK" + (MemberTime * 1000);

                foreach (Database.Data.Server m_Server in Database.Data.Server.ListOfServers)
                {
                    if (Client.m_Account.Characters.ContainsKey(m_Server.ID))
                    {
                        Pack += "|" + m_Server.ID + "," + Client.m_Account.Characters[m_Server.ID].Count;
                    }
                    else
                    {
                        Pack += "|" + m_Server.ID + ",0";
                    }
                }
                Client.Send(Pack);
            }
            else if (Packet.StartsWith("AX"))
            {
                int ID = int.Parse(Packet.Replace("AX", ""));

                foreach (RealmClient m_Server in Program.m_Realm.m_Clients)
                {
                    if (m_Server.m_Server.ID == ID)
                    {
                        string m_Key = Utils.Basic.RandomString(16);
                        m_Server.SendNewTicket(m_Key, Client);
                        Client.SendNewTicket(m_Key, m_Server);
                    }
                }
            }
            else if (Packet.StartsWith("AF"))
            {
                string PacketPseudo = "AF";

                foreach (Client.RealmClient m_Server in Program.m_Realm.m_Clients)
                {
                    if(m_Server.m_Server.Clients.Contains(Packet.Replace("AF", "")))
                    {
                        PacketPseudo += m_Server.m_Server.ID + ";";
                    }
                }

                if (PacketPseudo == "AF")
                    Client.Send("AF");
                else
                    Client.Send(PacketPseudo.Substring(0, PacketPseudo.Length - 1));
                
            }
        }
    }
}
