using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Cache
{
    class AuthsCache
    {
        public static List<Models.Clients.AuthClientModel> m_auths = new List<Models.Clients.AuthClientModel>();

        static Timer m_timer { get; set; }
        static bool is_started = false;

        public static void ReloadAuths(object sender = null, EventArgs e = null)
        {
            lock (DatabaseHandler.m_locker)
            {
                string sqlText = "SELECT * FROM dyn_auths";
                MySqlCommand sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);
                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var server = new Models.Clients.AuthClientModel();

                    server.m_id = sqlReader.GetInt16("Id");
                    server.m_ip = sqlReader.GetString("Ip");
                    server.m_port = sqlReader.GetInt16("Port");

                    if (!m_auths.Any(x => x.m_id == server.m_id))
                        m_auths.Add(server);
                }

                sqlReader.Close();
            }

            if (!is_started)
            {
                m_timer = new Timer();
                m_timer.Interval = Utilities.Config.GetConfig.GetIntElement("TimeReloadAuths");
                m_timer.Enabled = true;
                m_timer.Elapsed += new ElapsedEventHandler(ReloadAuths);
            }
            else
                Network.ServersHandler.m_authLinks.Update(m_auths);
        }
    }
}
