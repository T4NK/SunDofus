using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus.Utilities;
using SunDofus.Entities;
using SunDofus.Entities.Requests;
using SunDofus.Network;

namespace SunDofus
{
    class Program
    {
        static void Main(string[] args)
        {
            Basic.Uptime = Environment.TickCount;
            Console.Title = "SunDofus ";

            Config.LoadConfiguration();
            Loggers.InitializeLoggers();

            if (Config.GetBoolElement("Realm"))
            {
                try
                {
                    ServersHandler.InitialiseServers();
                    DatabaseProvider.InitializeConnection();

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
