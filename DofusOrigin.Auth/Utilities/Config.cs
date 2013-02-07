using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DofusOrigin.Settings;

namespace DofusOrigin.Utilities
{
    class Config
    {
        private static Configuration _config;

        public static Configuration GetConfig
        {
            get
            {
                return _config;
            }
        }

        public static void LoadConfiguration()
        {
            if (!File.Exists("DofusOrigin.conf"))
                throw new Exception("Configuration file doesn't exist !");

            _config = new Configuration();
            {
                _config.LoadConfiguration("DofusOrigin.conf");
                AddParameters();
            }
        }

        private static void AddParameters()
        {
            //Status
            _config.InsertElement("Status_inConsole", "true");
            _config.InsertElement("Status_inFile", "false");

            //Infos
            _config.InsertElement("Infos_inFile", "false");
            _config.InsertElement("Infos_inConsole", "true");

            //Errors
            _config.InsertElement("Errors_inFile", "true");
            _config.InsertElement("Errors_inConsole", "true");

            //Auth & Sync
            _config.InsertElement("Auth_Ip", "127.0.0.1");
            _config.InsertElement("Auth_Port", "485");
            _config.InsertElement("Sync_Ip", "127.0.0.1");
            _config.InsertElement("Sync_Port", "486");

            //Client
            _config.InsertElement("Login_Version", "1.29.1");

            //Database
            _config.InsertElement("Database_Server", "localhost");
            _config.InsertElement("Database_User", "root");
            _config.InsertElement("Database_Pass", "");
            _config.InsertElement("Database_Name", "dofusorigin_auth");

            //Cache
            _config.InsertElement("Time_Accounts_Reload", "60000");
            _config.InsertElement("Time_Servers_Reload", "60000");
            _config.InsertElement("Time_Gifts_Reload", "60000");


            //Queue
            _config.InsertElement("Max_Clients_inQueue", "50");
            _config.InsertElement("Client_Per_QueueRefresh", "10");
            _config.InsertElement("Time_Queue_Reload", "5000");

            //Subscription
            _config.InsertElement("Max_Subscription_Time", "31536000000");
            _config.InsertElement("Subscription_Time", "true");
        }
    }
}
