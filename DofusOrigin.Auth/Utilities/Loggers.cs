using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Interface;

namespace DofusOrigin.Utilities
{
    class Loggers
    {
        public static Logger m_statusLogger { get; set; }
        public static Logger m_infosLogger { get; set; }
        public static Logger m_errorsLogger { get; set; }

        public static void InitialiseLoggers()
        {
            m_statusLogger = new Logger("Status", Basic.m_consoleLocker, ConsoleColor.Green);
            {
                if (Config.m_config.GetBoolElement("Status_inConsole"))
                    m_statusLogger.StartConsoleLogger();

                if (Config.m_config.GetBoolElement("Status_inFile"))
                    m_statusLogger.StartFileLogger();
            }

            m_infosLogger = new Logger("Infos", Basic.m_consoleLocker, ConsoleColor.Magenta);
            {
                if (Config.m_config.GetBoolElement("Infos_inFile"))
                    m_infosLogger.StartFileLogger();

                if (Config.m_config.GetBoolElement("Infos_inConsole"))
                    m_infosLogger.StartConsoleLogger();
            }

            m_errorsLogger = new Logger("Errors", Basic.m_consoleLocker, ConsoleColor.Yellow);
            {
                if (Config.m_config.GetBoolElement("Errors_inFile"))
                    m_errorsLogger.StartFileLogger();

                if (Config.m_config.GetBoolElement("Errors_inConsole"))
                    m_errorsLogger.StartConsoleLogger();
            }

            m_statusLogger.Write("@Loggers@ loaded and started !");
        }
    }
}
