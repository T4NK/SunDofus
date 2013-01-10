using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Utilities;
using DofusOrigin.Database;
using DofusOrigin.Database.Cache;
using DofusOrigin.Network;

namespace DofusOrigin
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
