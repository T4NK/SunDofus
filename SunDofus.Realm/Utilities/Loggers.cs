using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus.Interface;

namespace realm.Utilities
{
    class Loggers
    {
        public static Logger m_statusLogger { get; set; }
        public static Logger m_infosLogger { get; set; }
        public static Logger m_errorsLogger { get; set; }

        public static void InitialiseLoggers()
        {
            m_statusLogger = new Logger("Status", Basic.m_locker, ConsoleColor.Green);
            {
                if (Config.m_config.GetBoolElement("Status_inConsole") == true)
                    m_statusLogger.StartConsoleLogger();
                if (Config.m_config.GetBoolElement("Status_inFile") == true)
                    m_statusLogger.StartFileLogger();
            }

            m_infosLogger = new Logger("Infos", Basic.m_locker, ConsoleColor.Magenta);
            {
                if (Config.m_config.GetBoolElement("Infos_inFile") == true)
                    m_infosLogger.StartFileLogger();

                if (Config.m_config.GetBoolElement("Infos_inConsole") == true)
                    m_infosLogger.StartConsoleLogger();
            }

            m_errorsLogger = new Logger("Errors", Basic.m_locker, ConsoleColor.Yellow);
            {
                if (Config.m_config.GetBoolElement("Errors_inFile") == true)
                    m_errorsLogger.StartFileLogger();

                if (Config.m_config.GetBoolElement("Errors_inConsole") == true)
                    m_errorsLogger.StartConsoleLogger();
            }

            m_statusLogger.Write("@Loggers@ loaded and started !");
        }
    }
}
