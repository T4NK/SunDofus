using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus.Network.Auth;
using SunDofus.Network.Sync;

namespace SunDofus.Network
{
    class ServersHandler
    {
        public static AuthServer AuthServer;
        public static SyncServer SyncServer;

        public static void InitialiseServers()
        {
            AuthServer = new AuthServer();
            AuthServer.Start();

            SyncServer = new SyncServer();
            SyncServer.Start();
        }
    }
}
