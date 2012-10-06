using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus;

namespace auth.Utilities
{
    class Loggers
    {
        public static Logger StatusLogger;
        public static Logger InfosLogger;
        public static Logger ErrorsLogger;

        public static void InitialiseLoggers()
        {
            StatusLogger = new Logger("Status", ConsoleColor.Green);
        
            if (Config.myConfig.GetBoolElement("Status_inConsole") == true)
                StatusLogger.StartConsoleLogger();
            if (Config.myConfig.GetBoolElement("Status_inFile") == true)
                StatusLogger.StartFileLogger();

            InfosLogger = new Logger("Infos", ConsoleColor.Magenta);

            if (Config.myConfig.GetBoolElement("Infos_inFile") == true)
                InfosLogger.StartFileLogger();

            if (Config.myConfig.GetBoolElement("Infos_inConsole") == true)
                InfosLogger.StartConsoleLogger();

            ErrorsLogger = new Logger("Errors", ConsoleColor.Yellow);

            if (Config.myConfig.GetBoolElement("Errors_inFile") == true)
                ErrorsLogger.StartFileLogger();

            if (Config.myConfig.GetBoolElement("Errors_inConsole") == true)
                ErrorsLogger.StartConsoleLogger();

            StatusLogger.Write("@Loggers@ started !");
        }
    }
}
