using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Settings;

namespace DofusOrigin.Utilities
{
    class Config
    {
        public static Configuration m_config { get; set; }

        public static void LoadConfiguration()
        {
            try
            {
                m_config = new Configuration();
                {
                    m_config.LoadConfiguration("DofusOrigin.conf");
                    AddDefaultsParameters();
                }
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

            //Database
            m_config.InsertElement("Database_Server", "localhost");
            m_config.InsertElement("Database_User", "root");
            m_config.InsertElement("Database_Pass", "");
            m_config.InsertElement("Database_Name", "dofusorigin_realm");

            //AuthServer & RealmServer
            m_config.InsertElement("AuthIp", "127.0.0.1");
            m_config.InsertElement("AuthPort", "486");
            m_config.InsertElement("ServerId", "6");
            m_config.InsertElement("ServerIp", "127.0.0.1");
            m_config.InsertElement("ServerPort", "5555");
            m_config.InsertElement("ServerCom", "0");

            //ChatSpam
            m_config.InsertElement("AntiSpamTrade", "60000");
            m_config.InsertElement("AntiSpamRecruitment", "60000");

            //Cache
            m_config.InsertElement("TimeReloadAuths", "60000");

            m_config.InsertElement("MustMonstersMove", "false");
            m_config.InsertElement("MustNPCsMove", "false");

        #region Start

            m_config.InsertElement("StartLevel", "1");
            m_config.InsertElement("StartKamas", "0");

            m_config.InsertElement("StartMap_Feca", "10300");
            m_config.InsertElement("StartCell_Feca", "337");
            m_config.InsertElement("StartDir_Feca", "3");

            m_config.InsertElement("StartMap_Osa", "10258");
            m_config.InsertElement("StartCell_Osa", "210");
            m_config.InsertElement("StartDir_Osa", "3");

            m_config.InsertElement("StartMap_Enu", "10299");
            m_config.InsertElement("StartCell_Enu", "300");
            m_config.InsertElement("StartDir_Enu", "3");

            m_config.InsertElement("StartMap_Sram", "10285");
            m_config.InsertElement("StartCell_Sram", "263");
            m_config.InsertElement("StartDir_Sram", "3");

            m_config.InsertElement("StartMap_Xel", "10298");
            m_config.InsertElement("StartCell_Xel", "301");
            m_config.InsertElement("StartDir_Xel", "3");

            m_config.InsertElement("StartMap_Eca", "10276");
            m_config.InsertElement("StartCell_Eca", "296");
            m_config.InsertElement("StartDir_Eca", "3");

            m_config.InsertElement("StartMap_Eni", "10283");
            m_config.InsertElement("StartCell_Eni", "299");
            m_config.InsertElement("StartDir_Eni", "3");

            m_config.InsertElement("StartMap_Iop", "10294");
            m_config.InsertElement("StartCell_Iop", "263");
            m_config.InsertElement("StartDir_Iop", "3");

            m_config.InsertElement("StartMap_Cra", "10292");
            m_config.InsertElement("StartCell_Cra", "299");
            m_config.InsertElement("StartDir_Cra", "3");

            m_config.InsertElement("StartMap_Sadi", "10279");
            m_config.InsertElement("StartCell_Sadi", "269");
            m_config.InsertElement("StartDir_Sadi", "3");

            m_config.InsertElement("StartMap_Sacri", "10296");
            m_config.InsertElement("StartCell_Sacri", "244");
            m_config.InsertElement("StartDir_Sacri", "3");

            m_config.InsertElement("StartMap_Panda", "10289");
            m_config.InsertElement("StartCell_Panda", "264");
            m_config.InsertElement("StartDir_Panda", "3"); 

            #endregion         
        }
    }
}
