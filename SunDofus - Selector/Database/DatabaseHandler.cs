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
        public static object myConnectionLocker;

        public static void InitialiseConnection()
        {
            myConnection = new MySqlConnection();
            myConnectionLocker = new object();

            try
            {
                myConnection.ConnectionString = "server=" + Utilities.Config.myConfig.GetStringElement("Database_Server") +
                        ";uid=" + Utilities.Config.myConfig.GetStringElement("Database_User") +
                        ";pwd='" + Utilities.Config.myConfig.GetStringElement("Database_Pass") +
                        "';database=" + Utilities.Config.myConfig.GetStringElement("Database_Name");

                myConnection.Open();

                Utilities.Loggers.StatusLogger.Write("Connected to the @database@ !");
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Can't connect to the @database@ ({0})",e.ToString()));
            }
        }
    }
}
