using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Interface;

namespace DofusOrigin.Utilities
{
    class Loggers
    {
        public static Logger StatusLogger;
        public static Logger InfosLogger;
        public static Logger ErrorsLogger;

        public static void InitialiseLoggers()
        {
            StatusLogger = new Logger("Status", Basic.ConsoleLocker, ConsoleColor.Green);
            {
                if (Config.GetConfig.GetBoolElement("Status_inConsole") == true)
                    StatusLogger.StartConsoleLogger();
                if (Config.GetConfig.GetBoolElement("Status_inFile") == true)
                    StatusLogger.StartFileLogger();
            }

            InfosLogger = new Logger("Infos", Basic.ConsoleLocker, ConsoleColor.Magenta);
            {
                if (Config.GetConfig.GetBoolElement("Infos_inFile") == true)
                    InfosLogger.StartFileLogger();

                if (Config.GetConfig.GetBoolElement("Infos_inConsole") == true)
                    InfosLogger.StartConsoleLogger();
            }

            ErrorsLogger = new Logger("Errors", Basic.ConsoleLocker, ConsoleColor.Yellow);
            {
                if (Config.GetConfig.GetBoolElement("Errors_inFile") == true)
                    ErrorsLogger.StartFileLogger();

                if (Config.GetConfig.GetBoolElement("Errors_inConsole") == true)
                    ErrorsLogger.StartConsoleLogger();
            }

            StatusLogger.Write("@Loggers@ loaded and started !");
        }
    }
}
