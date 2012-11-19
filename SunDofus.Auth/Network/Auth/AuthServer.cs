using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace auth.Network.Auth
{
    class AuthServer : SunDofus.Network.AbstractServer
    {
        public List<AuthClient> myClients;

        public AuthServer()
            : base(Utilities.Config.m_config.GetStringElement("Auth_Ip"), Utilities.Config.m_config.GetIntElement("Auth_Port"))
        {
            myClients = new List<AuthClient>();
            this.SocketClientAccepted += new AcceptSocketHandler(this.AcceptClientServer);
            this.ListeningServer += new ListeningServerHandler(this.OnListenServer);
            this.ListeningServerFailed += new ListeningServerFailedHandler(this.OnListenFailedServer);

            AuthQueue.Start();
        }

        public void AcceptClientServer(SilverSocket newSocket)
        {
            if (newSocket == null) return;
            Utilities.Loggers.m_infosLogger.Write(string.Format("New inputted realm connection @<{0}>@ !", newSocket.IP));

            lock (myClients)
                myClients.Add(new AuthClient(newSocket));
        }

        public void OnListenServer(string Remote)
        {
            Utilities.Loggers.m_statusLogger.Write(string.Format("@RealmServer@ starded on <{0}> !", Remote));
        }

        public void OnListenFailedServer(Exception e)
        {
            Utilities.Loggers.m_errorsLogger.Write(string.Format("@RealmServer@ can't start : {0}", e.ToString()));
        }

        public void RefreshAllHosts()
        {
            foreach (var Client in myClients)
                Client.SendHosts();
        }
    }
}
