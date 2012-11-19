using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus.Interface;

namespace auth.Utilities
{
    class Loggers
    {
        public static Logger m_statusLogger;
        public static Logger m_infosLogger;
        public static Logger m_errorsLogger;

        public static void InitialiseLoggers()
        {
            m_statusLogger = new Logger("Status", Basic.m_locker, ConsoleColor.Green);
        
            if (Config.m_config.GetBoolElement("Status_inConsole"))
                m_statusLogger.StartConsoleLogger();
            if (Config.m_config.GetBoolElement("Status_inFile"))
                m_statusLogger.StartFileLogger();

            m_infosLogger = new Logger("Infos", Basic.m_locker, ConsoleColor.Magenta);

            if (Config.m_config.GetBoolElement("Infos_inFile"))
                m_infosLogger.StartFileLogger();

            if (Config.m_config.GetBoolElement("Infos_inConsole"))
                m_infosLogger.StartConsoleLogger();

            m_errorsLogger = new Logger("Errors", Basic.m_locker, ConsoleColor.Yellow);

            if (Config.m_config.GetBoolElement("Errors_inFile"))
                m_errorsLogger.StartFileLogger();

            if (Config.m_config.GetBoolElement("Errors_inConsole"))
                m_errorsLogger.StartConsoleLogger();

            m_statusLogger.Write("@Loggers@ loaded and started !");
        }
    }
}
