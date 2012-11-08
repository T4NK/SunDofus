using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Network
{
    class ServersHandler
    {
        public static Realm.RealmServer myRealmServer;
        public static Authentication.AuthenticationLink myAuthLink;
        public static Editors.EditorServer myEditorServer;

        public static Dictionary<string, string> adminAccount;

        public static void InitialiseServers()
        {
            adminAccount = new Dictionary<string, string>();

            myRealmServer = new Realm.RealmServer();
            myRealmServer.Start();

            myAuthLink = new Authentication.AuthenticationLink();
            myAuthLink.Start();

            myEditorServer = new Editors.EditorServer();
            myEditorServer.Start();
        }
    }
}
