using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace auth.Database
{
    class DatabaseHandler
    {
        public static MySqlConnection m_connection;
        public static object m_locker;

        public static void InitialiseConnection()
        {
            m_connection = new MySqlConnection();
            m_locker = new object();

            m_connection.ConnectionString = string.Format("server={0};uid={1};pwd='{2}';database={3}",
                    Utilities.Config.m_config.GetStringElement("Database_Server"),
                    Utilities.Config.m_config.GetStringElement("Database_User"),
                    Utilities.Config.m_config.GetStringElement("Database_Pass"),
                    Utilities.Config.m_config.GetStringElement("Database_Name"));

            lock (m_locker)
                m_connection.Open();

            Utilities.Loggers.m_statusLogger.Write("Connected to the @database@ !");
        }
    }
}
