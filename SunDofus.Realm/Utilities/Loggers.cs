using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus;

namespace realm.Utilities
{
    class Loggers
    {
        public static Logger StatusLogger;
        public static Logger InfosLogger;
        public static Logger ErrorsLogger;

        public static void InitialiseLoggers()
        {
            StatusLogger = new Logger("Status",Basic._Locker, ConsoleColor.Green);

            if (Config.myConfig.GetBoolElement("Status_inConsole") == true)
                StatusLogger.StartConsoleLogger();
            if (Config.myConfig.GetBoolElement("Status_inFile") == true)
                StatusLogger.StartFileLogger();

            InfosLogger = new Logger("Infos", Basic._Locker, ConsoleColor.Magenta);

            if (Config.myConfig.GetBoolElement("Infos_inFile") == true)
                InfosLogger.StartFileLogger();

            if (Config.myConfig.GetBoolElement("Infos_inConsole") == true)
                InfosLogger.StartConsoleLogger();

            ErrorsLogger = new Logger("Errors", Basic._Locker, ConsoleColor.Yellow);

            if (Config.myConfig.GetBoolElement("Errors_inFile") == true)
                ErrorsLogger.StartFileLogger();

            if (Config.myConfig.GetBoolElement("Errors_inConsole") == true)
                ErrorsLogger.StartConsoleLogger();

            StatusLogger.Write("@Loggers@ started !");
        }
    }
}
