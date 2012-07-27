using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace selector.Database.Data
{
    class Server
    {
        public int ID = 0;
        public string Ip = "";
        public int Port = 0;
        public int Connected = 0;

        public List<string> Clients = new List<string>();

        public override string ToString()
        {
            return this.ID + ";" + Connected + ";" + (75 * this.ID) + ";1|";
        }

        public static List<Server> ListOfServers = new List<Server>();

        public static void LoadServer()
        {
            try
            {
                string SQLText = "SELECT * FROM servers";
                MySqlCommand SQLCommand = new MySqlCommand(SQLText, SQLManager.m_Connection);
                MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

                while (SQLReader.Read())
                {
                    Server NewServer = new Server();
                    NewServer.ID = SQLReader.GetInt16("Id");
                    NewServer.Ip = SQLReader.GetString("Ip");
                    NewServer.Port = SQLReader.GetInt16("Port");
                    ListOfServers.Add(NewServer);
                }

                SQLReader.Close();

                SunDofus.Logger.Status("'" + ListOfServers.Count + "' servers loaded !");
            }
            catch (Exception e)
            {
                SunDofus.Logger.Error(e);
            }
        }
    }
}
