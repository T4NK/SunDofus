using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace auth.Network.Sync
{
    class SyncServer : DofusOrigin.Network.TCPServer
    {
        public List<SyncClient> m_clients { get; set; }

        public SyncServer()
            : base(Utilities.Config.m_config.GetStringElement("Sync_Ip"), Utilities.Config.m_config.GetIntElement("Sync_Port"))
        {
            m_clients = new List<SyncClient>();

            this.SocketClientAccepted += new AcceptSocketHandler(this.OnAcceptedClient);
            this.ListeningServer += new ListeningServerHandler(this.OnListeningServer);
            this.ListeningServerFailed += new ListeningServerFailedHandler(this.OnListeningFailedServer);
        }

        private void OnAcceptedClient(SilverSocket _socket)
        {
            if (_socket == null) return;
            Utilities.Loggers.m_infosLogger.Write(string.Format("New inputted sync connection @<{0}>@ !", _socket.IP));

            lock (m_clients)
                m_clients.Add(new SyncClient(_socket));
        }

        private void OnListeningServer(string _remote)
        {
            Utilities.Loggers.m_statusLogger.Write(string.Format("@SyncServer@ starded on <{0}> !", _remote));
        }

        private void OnListeningFailedServer(Exception _exception)
        {
            Utilities.Loggers.m_errorsLogger.Write(string.Format("@SyncServer@ can't start : {0}", _exception.ToString()));
        }
    }
}
