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
        private static List<Models.ServerModel> _servers = new List<Models.ServerModel>();

        public static List<Models.ServerModel> Cache
        {
            get
            {
                return _servers;
            }
        }

        private static bool _started = false;
        private static Timer _cache = new Timer();

        public static void ReloadCache(object sender = null, EventArgs e = null)
        {
            Utilities.Loggers.InfosLogger.Write("Reloading of @Servers' Cache@ ...");

            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM dyn_realms";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);
                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var server = new Models.ServerModel();
                    {
                        server.ID = sqlReader.GetInt16("Id");
                        server.IP = sqlReader.GetString("Ip");
                        server.Port = sqlReader.GetInt16("Port");
                    }

                    if (!_servers.Any(x => x.ID == server.ID))
                        _servers.Add(server);
                }

                sqlReader.Close();
            }

            if (!_started == true)
            {
                _started = true;
                _cache.Interval = Utilities.Config.GetConfig.GetIntElement("Time_Accounts_Reload");
                _cache.Enabled = true;
                _cache.Elapsed += new ElapsedEventHandler(ReloadCache);
            }
        }
    }
}
