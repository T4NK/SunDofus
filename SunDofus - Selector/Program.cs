using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace selector
{
    class Program
    {
        public static Network.AuthentificationServer m_Auth;

        static void Main(string[] args)
        {
            Console.Title = "SunDofus - RealmSelector | Np-Develop [c]  2012";

            Config.ConfigurationManager.LoadConfiguration();
            Database.SQLManager.Initialise();
            Database.Data.Server.LoadServer();

            m_Auth = new Network.AuthentificationServer();
            m_Auth.Start();

            Console.ReadLine();
        }
    }
}
