using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace selector
{
    class Program
    {
        public static Network.AuthenticationServer m_Auth;
        public static Network.RealmServer m_Realm;

        static void Main(string[] args)
        {
            Console.Title = "SunDofus - RealmSelector | Shaak [c]  2012";

            Config.ConfigurationManager.IniConfig();
            SunDofus.Logger.Debug = Config.ConfigurationManager.GetBool("Debug");

            Database.SQLManager.Initialise();
            Database.ServersManager.ReloadCache(new object(), new EventArgs());
            Database.GiftsManager.ReloadCache(new object(), new EventArgs());
            Database.AccountsManager.ReloadCache(new object(), new EventArgs());

            m_Auth = new Network.AuthenticationServer();
            m_Auth.Start();

            m_Realm = new Network.RealmServer();
            m_Realm.Start();

            Client.SelectorQueue.StartInstance();

            Console.ReadLine();
        }
    }
}
