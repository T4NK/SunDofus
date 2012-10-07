using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using SunDofus;

namespace realm.Network.Realm
{
    class RealmServer : AbstractServer
    {
        public List<RealmClient> m_Clients;

        public RealmServer()
            : base(Utilities.Config.myConfig.GetStringElement("ServerIp"), Utilities.Config.myConfig.GetIntElement("ServerPort"))
        {
            m_Clients = new List<RealmClient>();
            this.RaiseAcceptEvent += new AcceptEvent(this.AcceptRealmClient);
            this.RaiseListenEvent += new OnListenEvent(this.ListenRealm);
            this.RaiseListenFailedEvent += new OnListenFailedEvent(this.ListenFailedRealm);
        }

        public void AcceptRealmClient(SilverSocket Socket)
        {
            if (Socket == null) return;
            Utilities.Loggers.InfosLogger.Write("New inputted client connection !");
            m_Clients.Add(new RealmClient(Socket));
        }

        public void ListenRealm(string Remote)
        {
            Utilities.Loggers.StatusLogger.Write(string.Format("RealmServer started on <{0}> !", Remote));
        }

        public void ListenFailedRealm(Exception e)
        {
            Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot start the RealmServer because : {0}", e.ToString()));
        }
    }
}
