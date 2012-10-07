using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace auth.Network.Auth
{
    class AuthServer : SunDofus.AbstractServer
    {
        public List<AuthClient> myClients;

        public AuthServer()
            : base(Utilities.Config.myConfig.GetStringElement("Auth_Ip"), Utilities.Config.myConfig.GetIntElement("Auth_Port"))
        {
            myClients = new List<AuthClient>();
            this.RaiseAcceptEvent += new AcceptEvent(this.AcceptClientServer);
            this.RaiseListenEvent += new OnListenEvent(this.OnListenServer);
            this.RaiseListenFailedEvent += new OnListenFailedEvent(this.OnListenFailedServer);

            AuthQueue.Start();
        }

        public void AcceptClientServer(SilverSocket newSocket)
        {
            if (newSocket == null) return;
            Utilities.Loggers.InfosLogger.Write(string.Format("New inputted realm connection @<{0}>@ !", newSocket.IP));

            lock (myClients)
                myClients.Add(new AuthClient(newSocket));
        }

        public void OnListenServer(string Remote)
        {
            Utilities.Loggers.StatusLogger.Write(string.Format("@RealmServer@ starded on <{0}> !", Remote));
        }

        public void OnListenFailedServer(Exception e)
        {
            Utilities.Loggers.ErrorsLogger.Write(string.Format("@RealmServer@ can't start : {0}", e.ToString()));
        }

        public void RefreshAllHosts()
        {
            foreach (AuthClient Client in myClients)
            {
                Client.SendHosts();
            }
        }
    }
}
