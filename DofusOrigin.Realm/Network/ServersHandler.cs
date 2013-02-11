using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Network
{
    class ServersHandler
    {
        public static Realm.RealmServer RealmServer;
        public static Authentication.AuthenticationsLinks AuthLinks;

        public static void InitialiseServers()
        {
            RealmServer = new Realm.RealmServer();
            RealmServer.Start();

            AuthLinks = new Authentication.AuthenticationsLinks();
            AuthLinks.Start();
        }
    }
}
