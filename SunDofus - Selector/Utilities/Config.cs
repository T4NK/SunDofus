using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SunDofus;

namespace auth.Utilities
{
    class Config
    {
        public static Configuration myConfig;

        public static void LoadConfiguration()
        {
            try
            {
                myConfig = new Configuration();
                myConfig.LoadConfiguration("SunAuth.conf");
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
            if (!myConfig.ExistElement("Status_inConsole"))
                myConfig.InsertElement("Status_inConsole", "true");
            if (!myConfig.ExistElement("Status_inFile"))
                myConfig.InsertElement("Status_inFile", "false");

            //Infos
            if (!myConfig.ExistElement("Infos_inFile"))
                myConfig.InsertElement("Infos_inFile", "false");
            if (!myConfig.ExistElement("Infos_inConsole"))
                myConfig.InsertElement("Infos_inConsole", "true");

            //Errors
            if (!myConfig.ExistElement("Errors_inFile"))
                myConfig.InsertElement("Errors_inFile", "true");
            if (!myConfig.ExistElement("Errors_inConsole"))
                myConfig.InsertElement("Errors_inConsole", "true");

            //Realm & Sync
            if (!myConfig.ExistElement("Realm_Ip"))
                myConfig.InsertElement("Realm_Ip", "127.0.0.1");
            if (!myConfig.ExistElement("Realm_Port"))
                myConfig.InsertElement("Realm_Port", "485");
            if (!myConfig.ExistElement("Sync_Ip"))
                myConfig.InsertElement("Sync_Ip", "127.0.0.1");
            if (!myConfig.ExistElement("Sync_Port"))
                myConfig.InsertElement("Sync_Port", "486");

            //Client
            if (!myConfig.ExistElement("Login_Version"))
                myConfig.InsertElement("Login_Version", "1.29.1");

            //Database
            if (!myConfig.ExistElement("Database_Server"))
                myConfig.InsertElement("Database_Server", "localhost");
            if (!myConfig.ExistElement("Database_User"))
                myConfig.InsertElement("Database_User", "root");
            if (!myConfig.ExistElement("Database_Pass"))
                myConfig.InsertElement("Database_Pass", "");
            if (!myConfig.ExistElement("Database_Name"))
                myConfig.InsertElement("Database_Name", "sundofus");

            //Cache
            if (!myConfig.ExistElement("Time_Accounts_Reload"))
                myConfig.InsertElement("Time_Accounts_Reload", "60000");
            if (!myConfig.ExistElement("Time_Servers_Reload"))
                myConfig.InsertElement("Time_Servers_Reload", "60000");
            if (!myConfig.ExistElement("Time_Gifts_Reload"))
                myConfig.InsertElement("Time_Gifts_Reload", "60000");


            //Queue
            if (!myConfig.ExistElement("Max_Clients_inQueue"))
                myConfig.InsertElement("Max_Clients_inQueue", "50");
            if (!myConfig.ExistElement("Time_Queue_Reload"))
                myConfig.InsertElement("Time_Queue_Reload", "2000");

            //Subscription
            if (!myConfig.ExistElement("Max_Subscription_Time"))
                myConfig.InsertElement("Max_Subscription_Time", "31536000000");
            if (!myConfig.ExistElement("Subscription_Time"))
                myConfig.InsertElement("Subscription_Time", "true");

        }
    }
}
