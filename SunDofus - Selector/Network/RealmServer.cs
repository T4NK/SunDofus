using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace selector.Network
{
    class RealmServer : AbstractServer
    {
        public List<Client.RealmClient> m_Clients;

        public RealmServer()
            : base(Config.ConfigurationManager.GetString("Realm_Ip"), Config.ConfigurationManager.GetInt("Realm_Port"))
        {
            m_Clients = new List<Client.RealmClient>();
            this.RaiseAcceptEvent += new AcceptEvent(this.AcceptRealmClient);
            this.RaiseListenEvent += new OnListenEvent(this.OnListenRealm);
            this.RaiseListenFailedEvent += new OnListenFailedEvent(this.OnListenFailedRealm);
        }

        public void AcceptRealmClient(SilverSocket Socket)
        {
            if (Socket == null) return;
            Utils.Logger.Infos("New inputed server connection !");
            m_Clients.Add(new Client.RealmClient(Socket));
        }

        public void OnListenRealm(int m_Port)
        {
            Utils.Logger.Status("RealmServer started on the port '" + m_Port + "' !");
        }

        public void OnListenFailedRealm(Exception e)
        {
            Utils.Logger.Error(e);
        }
    }
}
