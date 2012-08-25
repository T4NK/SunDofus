using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using MySql.Data.MySqlClient;

namespace selector.Database
{
    class ServersManager
    {
        public static List<Data.Server> myServers = new List<Data.Server>();
        static bool AutoReload = false;
        static Timer AutoTimer = new Timer();

        public static void ReloadCache(object sender, EventArgs e)
        {
            if (SQLManager.isRunning == true) return;
            try
            {
                SQLManager.isRunning = true;
                string Text = "SELECT * FROM servers";
                MySqlCommand Command = new MySqlCommand(Text, SQLManager.m_Connection);
                MySqlDataReader Reader = Command.ExecuteReader();

                while (Reader.Read())
                {
                    Data.Server newServer = new Data.Server();

                    newServer.ID = Reader.GetInt16("Id");
                    newServer.Ip = Reader.GetString("Ip");
                    newServer.Port = Reader.GetInt16("Port");

                    if (myServers.Count(x => x.ID == newServer.ID) == 0)
                    {
                        myServers.Add(newServer);
                    }
                }

                Reader.Close();
                SQLManager.isRunning = false;

                SunDofus.Logger.Infos("'" + myServers.Count + "' servers reloaded !");

                if (!AutoReload == true)
                {
                    AutoReload = true;
                    AutoTimer.Interval = Config.ConfigurationManager.GetInt("TimeReloadServers");
                    AutoTimer.Enabled = true;
                    AutoTimer.Elapsed += new ElapsedEventHandler(ReloadCache);
                }
            }
            catch (Exception ex)
            {
                SunDofus.Logger.Error(ex);
            }
        }
    }
}
