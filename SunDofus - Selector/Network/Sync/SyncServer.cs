using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace auth.Network.Sync
{
    class SyncServer : SunDofus.AbstractServer
    {
        public List<SyncClient> myClients;

        public SyncServer()
            : base(Utilities.Config.myConfig.GetStringElement("Sync_Ip"), Utilities.Config.myConfig.GetIntElement("Sync_Port"))
        {
            myClients = new List<SyncClient>();
            this.RaiseAcceptEvent += new AcceptEvent(this.AcceptRealmClient);
            this.RaiseListenEvent += new OnListenEvent(this.OnListenRealm);
            this.RaiseListenFailedEvent += new OnListenFailedEvent(this.OnListenFailedRealm);
        }

        public void AcceptRealmClient(SilverSocket newSocket)
        {
            if (newSocket == null) return;
            Utilities.Loggers.InfosLogger.Write(string.Format("New inputted sync connection @<{0}>@ !", newSocket.IP));

            lock (myClients)
                myClients.Add(new SyncClient(newSocket));
        }

        public void OnListenRealm(string Remote)
        {
            Utilities.Loggers.StatusLogger.Write(string.Format("@SyncServer@ starded on <{0}> !", Remote));
        }

        public void OnListenFailedRealm(Exception e)
        {
            Utilities.Loggers.ErrorsLogger.Write(string.Format("@SyncServer@ can't start : {0}", e.ToString()));
        }
    }
}
