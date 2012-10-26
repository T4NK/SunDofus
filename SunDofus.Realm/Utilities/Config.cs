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
                myConfig.InsertElement("Database_Name", "sundofus");

            //AuthServer & RealmServer
            if (!myConfig.ExistElement("AuthIp"))
                myConfig.InsertElement("AuthIp", "127.0.0.1");
            if (!myConfig.ExistElement("AuthPort"))
                myConfig.InsertElement("AuthPort", "486");
            if (!myConfig.ExistElement("ServerId"))
                myConfig.InsertElement("ServerId", "6");
            if (!myConfig.ExistElement("ServerIp"))
                myConfig.InsertElement("ServerIp", "127.0.0.1");
            if (!myConfig.ExistElement("ServerPort"))
                myConfig.InsertElement("ServerPort", "5555");

            //ChatSpam
            if (!myConfig.ExistElement("AntiSpamTrade"))
                myConfig.InsertElement("AntiSpamTrade", "60000");
            if (!myConfig.ExistElement("AntiSpamRecruitment"))
                myConfig.InsertElement("AntiSpamRecruitment", "60000");

            #region Start

            if (!myConfig.ExistElement("StartLevel"))
                myConfig.InsertElement("StartLevel", "1");

            if (!myConfig.ExistElement("StartMap_Feca"))
                myConfig.InsertElement("StartMap_Feca", "10300");
            if (!myConfig.ExistElement("StartCell_Feca"))
                myConfig.InsertElement("StartCell_Feca", "337");
            if (!myConfig.ExistElement("StartDir_Feca"))
                myConfig.InsertElement("StartDir_Feca", "3");

            if (!myConfig.ExistElement("StartMap_Osa"))
                myConfig.InsertElement("StartMap_Osa", "10258");
            if (!myConfig.ExistElement("StartCell_Osa"))
                myConfig.InsertElement("StartCell_Osa", "210");
            if (!myConfig.ExistElement("StartDir_Osa"))
                myConfig.InsertElement("StartDir_Osa", "3");

            if (!myConfig.ExistElement("StartMap_Enu"))
                myConfig.InsertElement("StartMap_Enu", "10299");
            if (!myConfig.ExistElement("StartCell_Enu"))
                myConfig.InsertElement("StartCell_Enu", "300");
            if (!myConfig.ExistElement("StartDir_Enu"))
                myConfig.InsertElement("StartDir_Enu", "3");

            if (!myConfig.ExistElement("StartMap_Sram"))
                myConfig.InsertElement("StartMap_Sram", "10285");
            if (!myConfig.ExistElement("StartCell_Sram"))
                myConfig.InsertElement("StartCell_Sram", "263");
            if (!myConfig.ExistElement("StartDir_Sram"))
                myConfig.InsertElement("StartDir_Sram", "3");

            if (!myConfig.ExistElement("StartMap_Xel"))
                myConfig.InsertElement("StartMap_Xel", "10298");
            if (!myConfig.ExistElement("StartCell_Xel"))
                myConfig.InsertElement("StartCell_Xel", "301");
            if (!myConfig.ExistElement("StartDir_Xel"))
                myConfig.InsertElement("StartDir_Xel", "3");

            if (!myConfig.ExistElement("StartMap_Eca"))
                myConfig.InsertElement("StartMap_Eca", "10276");
            if (!myConfig.ExistElement("StartCell_Eca"))
                myConfig.InsertElement("StartCell_Eca", "296");
            if (!myConfig.ExistElement("StartDir_Eca"))
                myConfig.InsertElement("StartDir_Eca", "3");

            if (!myConfig.ExistElement("StartMap_Eni"))
                myConfig.InsertElement("StartMap_Eni", "10283");
            if (!myConfig.ExistElement("StartCell_Eni"))
                myConfig.InsertElement("StartCell_Eni", "299");
            if (!myConfig.ExistElement("StartDir_Eni"))
                myConfig.InsertElement("StartDir_Eni", "3");

            if (!myConfig.ExistElement("StartMap_Iop"))
                myConfig.InsertElement("StartMap_Iop", "10294");
            if (!myConfig.ExistElement("StartCell_Iop"))
                myConfig.InsertElement("StartCell_Iop", "263");
            if (!myConfig.ExistElement("StartDir_Iop"))
                myConfig.InsertElement("StartDir_Iop", "3");

            if (!myConfig.ExistElement("StartMap_Cra"))
                myConfig.InsertElement("StartMap_Cra", "10292");
            if (!myConfig.ExistElement("StartCell_Cra"))
                myConfig.InsertElement("StartCell_Cra", "299");
            if (!myConfig.ExistElement("StartDir_Cra"))
                myConfig.InsertElement("StartDir_Cra", "3");

            if (!myConfig.ExistElement("StartMap_Sadi"))
                myConfig.InsertElement("StartMap_Sadi", "10279");
            if (!myConfig.ExistElement("StartCell_Sadi"))
                myConfig.InsertElement("StartCell_Sadi", "269");
            if (!myConfig.ExistElement("StartDir_Sadi"))
                myConfig.InsertElement("StartDir_Sadi", "3");

            if (!myConfig.ExistElement("StartMap_Sacri"))
                myConfig.InsertElement("StartMap_Sacri", "10296");
            if (!myConfig.ExistElement("StartCell_Sacri"))
                myConfig.InsertElement("StartCell_Sacri", "244");
            if (!myConfig.ExistElement("StartDir_Sacri"))
                myConfig.InsertElement("StartDir_Sacri", "3");

            if (!myConfig.ExistElement("StartMap_Panda"))
                myConfig.InsertElement("StartMap_Panda", "10289");
            if (!myConfig.ExistElement("StartCell_Panda"))
                myConfig.InsertElement("StartCell_Panda", "264");
            if (!myConfig.ExistElement("StartDir_Panda"))
                myConfig.InsertElement("StartDir_Panda", "3"); 

            #endregion         
        }
    }
}
