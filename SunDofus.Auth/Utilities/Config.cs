using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SunDofus.Settings;

namespace auth.Utilities
{
    class Config
    {
        public static Configuration m_config { get; set; }

        public static void LoadConfiguration()
        {
            try
            {
                m_config = new Configuration();
                m_config.LoadConfiguration("SunAuth.conf");
                AddDefaultsParameters();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static void AddDefaultsParameters()
        {
            //Status
            m_config.InsertElement("Status_inConsole", "true");
            m_config.InsertElement("Status_inFile", "false");

            //Infos
            m_config.InsertElement("Infos_inFile", "false");
            m_config.InsertElement("Infos_inConsole", "true");

            //Errors
            m_config.InsertElement("Errors_inFile", "true");
            m_config.InsertElement("Errors_inConsole", "true");

            //Auth & Sync
            m_config.InsertElement("Auth_Ip", "127.0.0.1");
            m_config.InsertElement("Auth_Port", "485");
            m_config.InsertElement("Sync_Ip", "127.0.0.1");
            m_config.InsertElement("Sync_Port", "486");

            //Client
            m_config.InsertElement("Login_Version", "1.29.1");

            //Database
            m_config.InsertElement("Database_Server", "localhost");
            m_config.InsertElement("Database_User", "root");
            m_config.InsertElement("Database_Pass", "");
            m_config.InsertElement("Database_Name", "sundofus");

            //Cache
            m_config.InsertElement("Time_Accounts_Reload", "60000");
            m_config.InsertElement("Time_Servers_Reload", "60000");
            m_config.InsertElement("Time_Gifts_Reload", "60000");


            //Queue
            m_config.InsertElement("Max_Clients_inQueue", "50");
            m_config.InsertElement("Time_Queue_Reload", "2000");

            //Subscription
            m_config.InsertElement("Max_Subscription_Time", "31536000000");
            m_config.InsertElement("Subscription_Time", "true");

        }
    }
}
