using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace auth.Network
{
    class ServersHandler
    {
        public static Auth.AuthServer m_authServer { get; set; }
        public static Sync.SyncServer m_syncServer { get; set; }

        public static void InitialiseServers()
        {
            m_authServer = new Auth.AuthServer();
            m_authServer.Start();

            m_syncServer = new Sync.SyncServer();
            m_syncServer.Start();
        }
    }
}
