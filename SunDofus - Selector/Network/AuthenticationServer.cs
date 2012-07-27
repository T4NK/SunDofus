using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace selector.Network
{
    class AuthenticationServer : SunDofus.AbstractServer
    {
        public List<Client.SelectorClient> m_Clients;

        public AuthenticationServer()
            : base(Config.ConfigurationManager.GetString("Auth_Ip"), Config.ConfigurationManager.GetInt("Auth_Port"))
        {
            m_Clients = new List<Client.SelectorClient>();
            this.RaiseAcceptEvent += new AcceptEvent(this.AcceptClientServer);
            this.RaiseListenEvent += new OnListenEvent(this.OnListenServer);
            this.RaiseListenFailedEvent += new OnListenFailedEvent(this.OnListenFailedServer);
        }

        public void AcceptClientServer(SilverSocket Socket)
        {
            if (Socket == null) return;
            SunDofus.Logger.Infos("New inputted connection !");
            m_Clients.Add(new Client.SelectorClient(Socket));
        }

        public void OnListenServer(int m_Port)
        {
            SunDofus.Logger.Status("AuthenticationServer started on the port '" + m_Port + "' !");
        }

        public void OnListenFailedServer(Exception e)
        {
            SunDofus.Logger.Error(e);
        }

        public void RefreshAllHosts()
        {
            foreach (Client.SelectorClient Client in m_Clients)
            {
                Client.SendHosts();
            }
        }
    }
}
