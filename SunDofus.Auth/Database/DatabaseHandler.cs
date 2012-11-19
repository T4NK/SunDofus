using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace auth.Database
{
    class DatabaseHandler
    {
        public static MySqlConnection myConnection;
        public static object myLocker;

        public static void InitialiseConnection()
        {
            myConnection = new MySqlConnection();
            myLocker = new object();

            try
            {
                myConnection.ConnectionString = string.Format("server={0};uid={1};pwd='{2}';database={3}",
                    Utilities.Config.m_config.GetStringElement("Database_Server"),
                    Utilities.Config.m_config.GetStringElement("Database_User"),
                    Utilities.Config.m_config.GetStringElement("Database_Pass"),
                    Utilities.Config.m_config.GetStringElement("Database_Name"));

                lock (myLocker)
                    myConnection.Open();

                Utilities.Loggers.m_statusLogger.Write("Connected to the @database@ !");
            }
            catch (Exception e)
            {
                Utilities.Loggers.m_errorsLogger.Write(string.Format("Can't connect to the @database@ ({0})",e.ToString()));
            }
        }
    }
}
