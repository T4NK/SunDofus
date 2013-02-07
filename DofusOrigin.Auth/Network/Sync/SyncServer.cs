using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace DofusOrigin.Network.Sync
{
    class SyncServer : DofusOrigin.Network.TCPServer
    {
        private List<SyncClient> _clients;

        public List<SyncClient> GetClients
        {
            get
            {
                return _clients;
            }
            set
            {
                _clients = value;
            }
        }

        public SyncServer()
            : base(Utilities.Config.GetConfig.GetStringElement("Sync_Ip"), Utilities.Config.GetConfig.GetIntElement("Sync_Port"))
        {
            _clients = new List<SyncClient>();

            this.SocketClientAccepted += new AcceptSocketHandler(this.OnAcceptedClient);
            this.ListeningServer += new ListeningServerHandler(this.OnListeningServer);
            this.ListeningServerFailed += new ListeningServerFailedHandler(this.OnListeningFailedServer);
        }

        private void OnAcceptedClient(SilverSocket socket)
        {
            if (socket == null) 
                return;

            Utilities.Loggers.InfosLogger.Write(string.Format("New inputted sync connection @<{0}>@ !", socket.IP));

            lock (GetClients)
                GetClients.Add(new SyncClient(socket));
        }

        private void OnListeningServer(string remote)
        {
            Utilities.Loggers.StatusLogger.Write(string.Format("@SyncServer@ starded on <{0}> !", remote));
        }

        private void OnListeningFailedServer(Exception exception)
        {
            Utilities.Loggers.ErrorsLogger.Write(string.Format("@SyncServer@ can't start : {0}", exception.ToString()));
        }
    }
}
