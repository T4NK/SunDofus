using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace selector
{
    class Program
    {
        public static Network.AuthentificationServer m_Auth;
        public static Network.RealmServer m_Realm;

        static void Main(string[] args)
        {
            Console.Title = "SunDofus - RealmSelector | Nicolas Petit [c]  2012";

            Config.ConfigurationManager.LoadConfiguration();
            Database.SQLManager.Initialise();
            Database.Data.Server.LoadServer();

            m_Auth = new Network.AuthentificationServer();
            m_Auth.Start();

            m_Realm = new Network.RealmServer();
            m_Realm.Start();

            Console.ReadLine();
        }
    }
}
