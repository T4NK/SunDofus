using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace auth.Network.Sync
{
    class SyncServer : SunDofus.Network.AbstractServer
    {
        public List<SyncClient> myClients;

        public SyncServer()
            : base(Utilities.Config.m_config.GetStringElement("Sync_Ip"), Utilities.Config.m_config.GetIntElement("Sync_Port"))
        {
            myClients = new List<SyncClient>();
            this.SocketClientAccepted += new AcceptSocketHandler(this.AcceptRealmClient);
            this.ListeningServer += new ListeningServerHandler(this.OnListenRealm);
            this.ListeningServerFailed += new ListeningServerFailedHandler(this.OnListenFailedRealm);
        }

        public void AcceptRealmClient(SilverSocket newSocket)
        {
            if (newSocket == null) return;
            Utilities.Loggers.m_infosLogger.Write(string.Format("New inputted sync connection @<{0}>@ !", newSocket.IP));

            lock (myClients)
                myClients.Add(new SyncClient(newSocket));
        }

        public void OnListenRealm(string Remote)
        {
            Utilities.Loggers.m_statusLogger.Write(string.Format("@SyncServer@ starded on <{0}> !", Remote));
        }

        public void OnListenFailedRealm(Exception e)
        {
            Utilities.Loggers.m_errorsLogger.Write(string.Format("@SyncServer@ can't start : {0}", e.ToString()));
        }
    }
}
