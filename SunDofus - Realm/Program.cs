using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm
{
    class Program
    {
        public static int m_ServerID = -1;

        public static Network.SelectorLink m_RealmLink;
        public static Network.AuthenticationServer m_AuthServer;

        static void Main(string[] args)
        {
            Config.ConfigurationManager.IniConfig();
            Console.Title = "SunDofus - Realm ~ " + m_ServerID + " | Nicolas Petit [c]  2012";
            Database.SQLManager.Initialise();

            Database.Data.MapSql.LoadMaps();
            Database.Data.CharacterSql.LoadCharacters();

            m_AuthServer = new Network.AuthenticationServer();
            m_AuthServer.Start();

            m_RealmLink = new Network.SelectorLink();
            m_RealmLink.Start();

            Console.ReadLine();
        }
    }
}
