using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace auth.Network
{
    class ServersHandler
    {
        public static Auth.AuthServer myAuthServer;
        public static Sync.SyncServer mySyncServer;

        public static void InitialiseServers()
        {
            myAuthServer = new Auth.AuthServer();
            myAuthServer.Start();

            mySyncServer = new Sync.SyncServer();
            mySyncServer.Start();
        }
    }
}
