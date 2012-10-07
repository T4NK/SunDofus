using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus;

namespace realm.Utilities
{
    class Config
    {
        public static Configuration myConfig;

        public static void LoadConfiguration()
        {
            try
            {
                myConfig = new Configuration();
                myConfig.LoadConfiguration("SunRealm.conf");
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

            //Realm
            if (!myConfig.ExistElement("Realm_Ip"))
                myConfig.InsertElement("Realm_Ip", "127.0.0.1");
            if (!myConfig.ExistElement("Realm_Port"))
                myConfig.InsertElement("Realm_Port", "5555");

            //Database
            if (!myConfig.ExistElement("Database_Server"))
                myConfig.InsertElement("Database_Server", "localhost");
            if (!myConfig.ExistElement("Database_User"))
                myConfig.InsertElement("Database_User", "root");
            if (!myConfig.ExistElement("Database_Pass"))
                myConfig.InsertElement("Database_Pass", "");
            if (!myConfig.ExistElement("Database_Name"))
                myConfig.InsertElement("Database_Name", "suncore");
        }
    }
}
