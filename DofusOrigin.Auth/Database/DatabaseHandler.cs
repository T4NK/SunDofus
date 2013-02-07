﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database
{
    class DatabaseHandler
    {
        public static MySqlConnection Connection;
        public static object ConnectionLocker;

        public static void InitializeConnection()
        {
            Connection = new MySqlConnection();
            ConnectionLocker = new object();

            Connection.ConnectionString = string.Format("server={0};uid={1};pwd='{2}';database={3}",
                    Utilities.Config.GetConfig.GetStringElement("Database_Server"),
                    Utilities.Config.GetConfig.GetStringElement("Database_User"),
                    Utilities.Config.GetConfig.GetStringElement("Database_Pass"),
                    Utilities.Config.GetConfig.GetStringElement("Database_Name"));

            lock (ConnectionLocker)
                Connection.Open();

            Utilities.Loggers.StatusLogger.Write("Connected to the @database@ !");
        }
    }
}
