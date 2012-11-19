using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus.Settings;

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
            myConfig.InsertElement("Status_inConsole", "true");
            myConfig.InsertElement("Status_inFile", "false");

            //Infos
            myConfig.InsertElement("Infos_inFile", "false");
            myConfig.InsertElement("Infos_inConsole", "true");

            //Errors
            myConfig.InsertElement("Errors_inFile", "true");
            myConfig.InsertElement("Errors_inConsole", "true");

            //Database
            myConfig.InsertElement("Database_Server", "localhost");
            myConfig.InsertElement("Database_User", "root");
            myConfig.InsertElement("Database_Pass", "");
            myConfig.InsertElement("Database_Name", "sundofus");

            //AuthServer & RealmServer & EditorServer
            myConfig.InsertElement("AuthIp", "127.0.0.1");
            myConfig.InsertElement("AuthPort", "486");
            myConfig.InsertElement("ServerId", "6");
            myConfig.InsertElement("ServerIp", "127.0.0.1");
            myConfig.InsertElement("ServerPort", "5555");
            myConfig.InsertElement("EditorIp", "127.0.0.1");
            myConfig.InsertElement("EditorPort", "487");

            //ChatSpam
            myConfig.InsertElement("AntiSpamTrade", "60000");
            myConfig.InsertElement("AntiSpamRecruitment", "60000");

        #region Start

            myConfig.InsertElement("StartLevel", "1");

            myConfig.InsertElement("StartMap_Feca", "10300");
            myConfig.InsertElement("StartCell_Feca", "337");
            myConfig.InsertElement("StartDir_Feca", "3");

            myConfig.InsertElement("StartMap_Osa", "10258");
            myConfig.InsertElement("StartCell_Osa", "210");
            myConfig.InsertElement("StartDir_Osa", "3");

            myConfig.InsertElement("StartMap_Enu", "10299");
            myConfig.InsertElement("StartCell_Enu", "300");
            myConfig.InsertElement("StartDir_Enu", "3");

            myConfig.InsertElement("StartMap_Sram", "10285");
            myConfig.InsertElement("StartCell_Sram", "263");
            myConfig.InsertElement("StartDir_Sram", "3");

            myConfig.InsertElement("StartMap_Xel", "10298");
            myConfig.InsertElement("StartCell_Xel", "301");
            myConfig.InsertElement("StartDir_Xel", "3");

            myConfig.InsertElement("StartMap_Eca", "10276");
            myConfig.InsertElement("StartCell_Eca", "296");
            myConfig.InsertElement("StartDir_Eca", "3");

            myConfig.InsertElement("StartMap_Eni", "10283");
            myConfig.InsertElement("StartCell_Eni", "299");
            myConfig.InsertElement("StartDir_Eni", "3");

            myConfig.InsertElement("StartMap_Iop", "10294");
            myConfig.InsertElement("StartCell_Iop", "263");
            myConfig.InsertElement("StartDir_Iop", "3");

            myConfig.InsertElement("StartMap_Cra", "10292");
            myConfig.InsertElement("StartCell_Cra", "299");
            myConfig.InsertElement("StartDir_Cra", "3");

            myConfig.InsertElement("StartMap_Sadi", "10279");
            myConfig.InsertElement("StartCell_Sadi", "269");
            myConfig.InsertElement("StartDir_Sadi", "3");

            myConfig.InsertElement("StartMap_Sacri", "10296");
            myConfig.InsertElement("StartCell_Sacri", "244");
            myConfig.InsertElement("StartDir_Sacri", "3");

            myConfig.InsertElement("StartMap_Panda", "10289");
            myConfig.InsertElement("StartCell_Panda", "264");
            myConfig.InsertElement("StartDir_Panda", "3"); 

            #endregion         
        }
    }
}
