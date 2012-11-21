using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace auth.Network.Auth
{
    class AuthServer : SunDofus.Network.AbstractServer
    {
        public List<AuthClient> m_clients { get; set; }

        public AuthServer()
            : base(Utilities.Config.m_config.GetStringElement("Auth_Ip"), Utilities.Config.m_config.GetIntElement("Auth_Port"))
        {
            m_clients = new List<AuthClient>();

            this.SocketClientAccepted += new AcceptSocketHandler(this.OnAcceptedClient);
            this.ListeningServer += new ListeningServerHandler(this.OnListeningServer);
            this.ListeningServerFailed += new ListeningServerFailedHandler(this.OnListeningFailedServer);

            AuthQueue.Start();
        }

        public void RefreshAllHosts()
        {
            foreach (var client in m_clients)
                client.RefreshHosts();
        }

        void OnAcceptedClient(SilverSocket _socket)
        {
            if (_socket == null) 
                return;

            Utilities.Loggers.m_infosLogger.Write(string.Format("New inputted realm connection @<{0}>@ !", _socket.IP));

            lock (m_clients)
                m_clients.Add(new AuthClient(_socket));
        }

        void OnListeningServer(string _remote)
        {
            Utilities.Loggers.m_statusLogger.Write(string.Format("@RealmServer@ starded on <{0}> !", _remote));
        }

        void OnListeningFailedServer(Exception _exception)
        {
            Utilities.Loggers.m_errorsLogger.Write(string.Format("@RealmServer@ can't start : {0}", _exception.ToString()));
        }
    }
}
