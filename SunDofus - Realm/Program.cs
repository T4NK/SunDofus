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

        static void Main(string[] args)
        {
            Config.ConfigurationManager.LoadConfiguration();
            Console.Title = "SunDofus - Realm ~ " + m_ServerID + " | Nicolas Petit [c]  2012";

            m_RealmLink = new Network.SelectorLink();
            m_RealmLink.Start();

            Console.ReadLine();
        }
    }
}
