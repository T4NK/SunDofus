using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.Clients
{
    class AuthClientModel
    {
        public int m_id { get; set; }
        public int m_port { get; set; }

        public string m_ip { get; set; }
    }
}
