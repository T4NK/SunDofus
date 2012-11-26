using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Network
{
    class ServersHandler
    {
        public static Realms.RealmServer m_realmServer;
        public static Authentication.AuthenticationLink m_authLink;
        public static Editors.EditorServer m_editorServer;

        public static Dictionary<string, string> adminAccount;

        public static void InitialiseServers()
        {
            adminAccount = new Dictionary<string, string>();

            m_realmServer = new Realms.RealmServer();
            m_realmServer.Start();

            m_authLink = new Authentication.AuthenticationLink();
            m_authLink.Start();

            m_editorServer = new Editors.EditorServer();
            m_editorServer.Start();
        }
    }
}
