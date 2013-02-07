using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Network
{
    class ServersHandler
    {
        public static Auth.AuthServer AuthServer;
        public static Sync.SyncServer SyncServer;

        public static void InitialiseServers()
        {
            AuthServer = new Auth.AuthServer();
            AuthServer.Start();

            SyncServer = new Sync.SyncServer();
            SyncServer.Start();
        }
    }
}
