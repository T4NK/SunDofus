using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace selector.Database
{
    class SQLManager
    {
        public static MySqlConnection m_Connection;
        public static bool isRunning;

        public static void Initialise()
        {
            try
            {
                m_Connection = new MySqlConnection();
                m_Connection.ConnectionString = "server=" + Config.ConfigurationManager.GetString("Sql_Ip") + ";uid=" + Config.ConfigurationManager.GetString("Sql_User") + ";pwd='"
                    + Config.ConfigurationManager.GetString("Sql_Pass") + "';database=" + Config.ConfigurationManager.GetString("Sql_Db") + ";";
                m_Connection.Open();
                SunDofus.Logger.Status("Connected to MySQL Server !");
            }
            catch (Exception e)
            {
                SunDofus.Logger.Error(e);
            }
        }
    }
}
