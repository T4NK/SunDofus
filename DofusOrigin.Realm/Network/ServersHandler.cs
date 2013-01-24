using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Network
{
    class ServersHandler
    {
        public static Realm.RealmServer m_realmServer;
        public static Authentication.AuthenticationsLinks m_authLinks;

        public static void InitialiseServers()
        {
            m_realmServer = new Realm.RealmServer();
            m_realmServer.Start();

            m_authLinks = new Authentication.AuthenticationsLinks();
            m_authLinks.Start();
        }
    }
}
