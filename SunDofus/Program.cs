using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus.Utilities;
using SunDofus.Auth.Entities;
using SunDofus.Auth.Entities.Requests;
using SunDofus.Network;

namespace SunDofus
{
    class Program
    {
        static void Main(string[] args)
        {
            Basic.Uptime = Environment.TickCount;
            Console.Title = "SunDofus";

            Config.LoadConfiguration();
            Loggers.InitializeLoggers();

            if (Config.GetBoolElement("Realm"))
            {
                try
                {
                    ServersHandler.InitialiseServers();
                    Auth.Entities.DatabaseProvider.InitializeConnection();

                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                }
            }

            if (Config.GetBoolElement("World"))
            {
                try
                {
                    Console.Title = string.Format("{0} | Server '{1}'", Console.Title, Config.GetIntElement("ServerID"));
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                }
            }

            Loggers.InfosLogger.Write(string.Format("Started in '{0}'ms !", Basic.Uptime));
            Console.ReadLine();
        }
    }
}
