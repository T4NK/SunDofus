using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (!System.IO.File.Exists("DofusOrigin.conf"))
                throw new Exception("Configuration file doesn't exist !");

            _config = new Configuration();
            {
                _config.LoadConfiguration("DofusOrigin.conf");
                AddDefaultsParameters();
            }
        }

        private static void AddDefaultsParameters()
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

            //Database
            _config.InsertElement("Database_Server", "localhost");
            _config.InsertElement("Database_User", "root");
            _config.InsertElement("Database_Pass", "");
            _config.InsertElement("Database_Name", "dofusorigin_realm");

            //AuthServer & RealmServer
            _config.InsertElement("AuthIp", "127.0.0.1");
            _config.InsertElement("AuthPort", "486");
            _config.InsertElement("ServerId", "6");
            _config.InsertElement("ServerIp", "127.0.0.1");
            _config.InsertElement("ServerPort", "5555");
            _config.InsertElement("ServerCom", "0");

            //ChatSpam
            _config.InsertElement("AntiSpamTrade", "60000");
            _config.InsertElement("AntiSpamRecruitment", "60000");

            //Cache
            _config.InsertElement("TimeReloadAuths", "60000");

            _config.InsertElement("MustMonstersMove", "false");
            _config.InsertElement("MustNPCsMove", "false");

        #region Start

            _config.InsertElement("StartLevel", "1");
            _config.InsertElement("StartKamas", "0");

            _config.InsertElement("StartMap_Feca", "10300");
            _config.InsertElement("StartCell_Feca", "337");
            _config.InsertElement("StartDir_Feca", "3");

            _config.InsertElement("StartMap_Osa", "10258");
            _config.InsertElement("StartCell_Osa", "210");
            _config.InsertElement("StartDir_Osa", "3");

            _config.InsertElement("StartMap_Enu", "10299");
            _config.InsertElement("StartCell_Enu", "300");
            _config.InsertElement("StartDir_Enu", "3");

            _config.InsertElement("StartMap_Sram", "10285");
            _config.InsertElement("StartCell_Sram", "263");
            _config.InsertElement("StartDir_Sram", "3");

            _config.InsertElement("StartMap_Xel", "10298");
            _config.InsertElement("StartCell_Xel", "301");
            _config.InsertElement("StartDir_Xel", "3");

            _config.InsertElement("StartMap_Eca", "10276");
            _config.InsertElement("StartCell_Eca", "296");
            _config.InsertElement("StartDir_Eca", "3");

            _config.InsertElement("StartMap_Eni", "10283");
            _config.InsertElement("StartCell_Eni", "299");
            _config.InsertElement("StartDir_Eni", "3");

            _config.InsertElement("StartMap_Iop", "10294");
            _config.InsertElement("StartCell_Iop", "263");
            _config.InsertElement("StartDir_Iop", "3");

            _config.InsertElement("StartMap_Cra", "10292");
            _config.InsertElement("StartCell_Cra", "299");
            _config.InsertElement("StartDir_Cra", "3");

            _config.InsertElement("StartMap_Sadi", "10279");
            _config.InsertElement("StartCell_Sadi", "269");
            _config.InsertElement("StartDir_Sadi", "3");

            _config.InsertElement("StartMap_Sacri", "10296");
            _config.InsertElement("StartCell_Sacri", "244");
            _config.InsertElement("StartDir_Sacri", "3");

            _config.InsertElement("StartMap_Panda", "10289");
            _config.InsertElement("StartCell_Panda", "264");
            _config.InsertElement("StartDir_Panda", "3"); 

            #endregion         
        }
    }
}
