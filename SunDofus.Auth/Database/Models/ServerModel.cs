using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace auth.Database.Models
{
    class ServerModel
    {
        public int myID = 0, myPort = 0, myState = 0;
        public string myIp = "";

        public List<string> myClients = new List<string>();

        public override string ToString()
        {
            return this.myID + ";" + myState + ";" + (75 * this.myID) + ";1|";
        }
    }
}
