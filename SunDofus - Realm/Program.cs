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
        public static Network.AuthentificationServer m_AuthServer;

        static void Main(string[] args)
        {
            Config.ConfigurationManager.LoadConfiguration();
            Console.Title = "SunDofus - Realm ~ " + m_ServerID + " | Nicolas Petit [c]  2012";
            Database.SQLManager.Initialise();
            Database.Data.CharacterSql.LoadCharacters();

            m_RealmLink = new Network.SelectorLink();
            m_RealmLink.Start();

            m_AuthServer = new Network.AuthentificationServer();
            m_AuthServer.Start();

            Console.ReadLine();
        }
    }
}
