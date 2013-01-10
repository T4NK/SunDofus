using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Cache
{
    class ServersCache
    {
        public static List<Models.ServerModel> m_servers = new List<Models.ServerModel>();

        private static bool m_started = false;
        private static Timer m_cache = new Timer();

        public static void ReloadCache(object sender = null, EventArgs e = null)
        {
            Utilities.Loggers.m_infosLogger.Write("Reloading of @Servers' Cache@ ...");

            lock (DatabaseHandler.m_locker)
            {
                string sqlText = "SELECT * FROM dyn_realms";
                MySqlCommand sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);
                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var server = new Models.ServerModel();
                    {
                        server.m_id = sqlReader.GetInt16("Id");
                        server.m_ip = sqlReader.GetString("Ip");
                        server.m_port = sqlReader.GetInt16("Port");
                    }

                    if (!m_servers.Any(x => x.m_id == server.m_id))
                        m_servers.Add(server);
                }

                sqlReader.Close();
            }

            if (!m_started == true)
            {
                m_started = true;
                m_cache.Interval = Utilities.Config.m_config.GetIntElement("Time_Accounts_Reload");
                m_cache.Enabled = true;
                m_cache.Elapsed += new ElapsedEventHandler(ReloadCache);
            }
        }
    }
}
