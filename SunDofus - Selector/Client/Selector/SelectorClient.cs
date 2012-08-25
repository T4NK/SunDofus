using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace selector.Client
{
    class SelectorClient : SunDofus.AbstractClient
    {
        public string m_Key = "";
        public SelectorParser m_Parser;
        public State m_State;
        public Database.Data.Account m_Account;

        public SelectorClient(SilverSocket Socket) : base(Socket)
        {
            this.RaiseClosedEvent += new OnClosedEvent(this.Disconnected);
            this.RaiseDataArrivalEvent += new DataArrivalEvent(this.PacketReceived);
            m_Key = SunDofus.Basic.RandomString(32);
            m_State = State.Version;
            m_Parser = new SelectorParser(this);
            Send("HC" + m_Key);
        }

        public void Disconnected()
        {
            SunDofus.Logger.Infos("New closed connection !");
            Program.m_Auth.m_Clients.Remove(this);
        }

        public void PacketReceived(string Data)
        {
            m_Parser.Parse(Data);
        }

        public void SendInformations()
        {
            Send("Ad" + m_Account.Pseudo);
            Send("Ac" + m_Account.Communauty);
            SendHosts();
            Send("AlK" + m_Account.Level);
            Send("AQ" + m_Account.Question);
        }

        public void SendHosts()
        {
            string Packet = "AH";

            foreach (Database.Data.Server m_Server in Database.ServersManager.myServers)
            {
                Packet += m_Server.ToString();
            }

            if (Packet != "AH")
                Send(Packet.Substring(0, Packet.Length - 1));
            else
                Send("AH");
        }

        public enum State
        {
            Version,
            Account,
            Queue,
            OnList,
            None,
        }

        public void SendNewTicket(string m_Key, RealmClient m_Client)
        {
            Send("AYK" + m_Client.m_Server.Ip + ":" + m_Client.m_Server.Port + ";" + m_Key);
        }
    }
}
