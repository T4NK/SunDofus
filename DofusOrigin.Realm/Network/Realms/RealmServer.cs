using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using DofusOrigin.Network;

namespace realm.Network.Realms
{
    class RealmServer : TCPServer
    {
        public List<RealmClient> m_clients;
        public List<string> m_pseudoClients;

        public RealmServer()
            : base(Utilities.Config.m_config.GetStringElement("ServerIp"), Utilities.Config.m_config.GetIntElement("ServerPort"))
        {
            m_clients = new List<RealmClient>();
            m_pseudoClients = new List<string>();

            this.SocketClientAccepted += new AcceptSocketHandler(this.OnAcceptedClient);
            this.ListeningServer += new ListeningServerHandler(this.OnListeningServer);
            this.ListeningServerFailed += new ListeningServerFailedHandler(this.OnListeningFailedServer);
        }

        public void OnAcceptedClient(SilverSocket _socket)
        {
            if (_socket == null) 
                return;

            Utilities.Loggers.m_infosLogger.Write("New inputted @client@ connection !");
            m_clients.Add(new RealmClient(_socket));
        }

        public void OnListeningServer(string _remote)
        {
            Utilities.Loggers.m_statusLogger.Write(string.Format("@RealmServer@ started on <{0}> !", _remote));
        }

        public void OnListeningFailedServer(Exception _exception)
        {
            Utilities.Loggers.m_errorsLogger.Write(string.Format("Cannot start the @RealmServer@ because : {0}", _exception.ToString()));
        }
    }
}
