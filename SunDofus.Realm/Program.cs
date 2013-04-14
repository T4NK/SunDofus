using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus.Utilities;
using SunDofus.Database;
using SunDofus.Database.Cache;
using SunDofus.Network;

namespace SunDofus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SunDofus";

            try
            {
                Config.LoadConfiguration();
                Loggers.InitializeLoggers();

                DatabaseHandler.InitializeConnection();

                AccountsCache.ResetConnectedValue();
                GiftsCache.ReloadCache();
                ServersCache.ReloadCache();

                ServersHandler.InitialiseServers();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.ReadLine();
        }
    }
}
