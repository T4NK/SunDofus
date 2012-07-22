using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace selector.Network
{
    class AuthentificationServer : AbstractServer
    {
        public List<Client.SelectorClient> m_Clients;

        public AuthentificationServer()
            : base(Config.ConfigurationManager.GetString("Ip_Auth"), Config.ConfigurationManager.GetInt("Port_Auth"))
        {
            m_Clients = new List<Client.SelectorClient>();
            this.RaiseAcceptEvent += new AcceptEvent(this.AcceptClientServer);
            this.RaiseListenEvent += new OnListenEvent(this.OnListenServer);
            this.RaiseListenFailedEvent += new OnListenFailedEvent(this.OnListenFailedServer);
        }

        public void AcceptClientServer(SilverSocket Socket)
        {
            if (Socket == null) return;
            Utils.Logger.Infos("New inputed connection !");
            m_Clients.Add(new Client.SelectorClient(Socket));
        }

        public void OnListenServer(int m_Port)
        {
            Utils.Logger.Status("AuthentificationServer started on the port '" + m_Port + "' !");
        }

        public void OnListenFailedServer(Exception e)
        {
            Utils.Logger.Error(e);
        }
    }
}
