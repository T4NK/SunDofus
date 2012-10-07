using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Network
{
    class ServersHandler
    {
        public static Realm.RealmServer myRealmServer;

        public static void InitialiseServers()
        {
            myRealmServer = new Realm.RealmServer();
            myRealmServer.Start();
        }
    }
}
