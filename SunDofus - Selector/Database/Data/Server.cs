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
    }
}
