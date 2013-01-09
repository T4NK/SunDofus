using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using auth.Utilities;
using auth.Database;
using auth.Database.Cache;
using auth.Network;

namespace auth
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "DofusOrigin.Auth | Shaak [c]";

            try
            {
                Config.LoadConfiguration();
                Loggers.InitialiseLoggers();

                DatabaseHandler.InitialiseConnection();

                GiftsCache.ReloadCache();
                AccountsCache.ReloadCache();
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
