using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using MySql.Data.MySqlClient;

namespace auth.Database.Cache
{
    class ServersCache
    {
        public static List<Models.ServerModel> myServers = new List<Models.ServerModel>();
        static bool AutoStarted = false;
        static Timer AutoCache = new Timer();

        public static void ReloadCache(object sender = null, EventArgs e = null)
        {
            Utilities.Loggers.InfosLogger.Write("Reloading of @Servers' Cache@ !");

            try
            {
                lock (DatabaseHandler.myLocker)
                {
                    string Text = "SELECT * FROM servers";
                    MySqlCommand Command = new MySqlCommand(Text, DatabaseHandler.myConnection);
                    MySqlDataReader Reader = Command.ExecuteReader();

                    while (Reader.Read())
                    {
                        Models.ServerModel newServer = new Models.ServerModel();

                        newServer.myID = Reader.GetInt16("Id");
                        newServer.myIp = Reader.GetString("Ip");
                        newServer.myPort = Reader.GetInt16("Port");

                        if (!myServers.Any(x => x.myID == newServer.myID))
                            myServers.Add(newServer);
                    }

                    Reader.Close();
                }

                if (!AutoStarted == true)
                {
                    AutoStarted = true;
                    AutoCache.Interval = Utilities.Config.myConfig.GetIntElement("Time_Accounts_Reload");
                    AutoCache.Enabled = true;
                    AutoCache.Elapsed += new ElapsedEventHandler(ReloadCache);
                }
            }
            catch (Exception ex)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot reload @servers@ ({0})", ex.ToString()));
            }
        }
    }
}
