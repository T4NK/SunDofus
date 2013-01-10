using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Models
{
    class ServerModel
    {
        public int m_id { get; set; }
        public int m_port { get; set; }
        public int m_state { get; set; }

        public string m_ip { get; set; }
        public List<string> m_clients { get; set; }

        public ServerModel()
        {
            m_clients = new List<string>();
        }

        public override string ToString()
        {
            return this.m_id + ";" + m_state + ";" + (75 * this.m_id) + ";1";
        }
    }
}
