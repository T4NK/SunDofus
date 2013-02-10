using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database
{
    class DatabaseHandler
    {
        public static MySqlConnection m_connection;
        public static object m_locker;

        public static void InitialiseConnection()
        {
            m_connection = new MySqlConnection();
            m_locker = new object();

            try
            {
                m_connection.ConnectionString = string.Format("server={0};uid={1};pwd='{2}';database={3}", 
                    Utilities.Config.GetConfig.GetStringElement("Database_Server"), 
                    Utilities.Config.GetConfig.GetStringElement("Database_User"), 
                    Utilities.Config.GetConfig.GetStringElement("Database_Pass"), 
                    Utilities.Config.GetConfig.GetStringElement("Database_Name"));

                lock (m_locker)
                    m_connection.Open();

                Utilities.Loggers.StatusLogger.Write("Connected to the @database@ !");
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Can't connect to the @database@ : {0}", e.ToString()));
            }
        }
    }
}
