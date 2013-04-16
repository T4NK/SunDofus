using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using MySql.Data.MySqlClient;

namespace SunDofus.Database.Cache
{
    class AuthsCache
    {
        public static List<Models.Clients.AuthClientModel> AuthsList = new List<Models.Clients.AuthClientModel>();

        private static Timer Timer;
        private static bool isStarted = false;

        public static void ReloadAuths(object sender = null, EventArgs e = null)
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM dyn_auths";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var server = new Models.Clients.AuthClientModel();

                    server.ID = sqlReader.GetInt16("Id");
                    server.IP = sqlReader.GetString("Ip");
                    server.Port = sqlReader.GetInt16("Port");

                    lock (AuthsList)
                    {
                        if (!AuthsList.Any(x => x.ID == server.ID))
                            AuthsList.Add(server);
                    }
                }

                sqlReader.Close();
            }

            if (!isStarted)
            {
                Timer = new Timer();
                Timer.Interval = Utilities.Config.GetConfig.GetIntElement("TimeReloadAuths");
                Timer.Enabled = true;
                Timer.Elapsed += new ElapsedEventHandler(ReloadAuths);
            }
            else
                Network.ServersHandler.AuthLinks.Update(AuthsList);
        }
    }
}
