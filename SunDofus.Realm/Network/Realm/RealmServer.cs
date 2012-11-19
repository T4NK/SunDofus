using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using SunDofus.Network;

namespace realm.Network.Realm
{
    class RealmServer : AbstractServer
    {
        public List<RealmClient> myClients;

        public RealmServer()
            : base(Utilities.Config.myConfig.GetStringElement("ServerIp"), Utilities.Config.myConfig.GetIntElement("ServerPort"))
        {
            myClients = new List<RealmClient>();
            this.SocketClientAccepted += new AcceptSocketHandler(this.AcceptRealmClient);
            this.ListeningServer += new ListeningServerHandler(this.ListenRealm);
            this.ListeningServerFailed += new ListeningServerFailedHandler(this.ListenFailedRealm);
        }

        public void AcceptRealmClient(SilverSocket Socket)
        {
            if (Socket == null) return;

            Utilities.Loggers.InfosLogger.Write("New inputted @client@ connection !");
            myClients.Add(new RealmClient(Socket));
        }

        public void ListenRealm(string Remote)
        {
            Utilities.Loggers.StatusLogger.Write(string.Format("@RealmServer@ started on <{0}> !", Remote));
        }

        public void ListenFailedRealm(Exception e)
        {
            Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot start the @RealmServer@ because : {0}", e.ToString()));
        }
    }
}
