using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Models
{
    class ServerModel
    {
        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        private int _port;

        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        private int _state;

        public int State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        private string _IP;

        public string IP
        {
            get
            {
                return _IP;
            }
            set
            {
                _IP = value;
            }
        }

        private List<string> _clients;

        public List<string> GetClients
        {
            get 
            {
                return _clients;
            }
            set 
            {
                _clients = value;
            }
        }

        public ServerModel()
        {
            _clients = new List<string>();
        }

        public override string ToString()
        {
            return this._ID + ";" + _state + ";" + (75 * this._ID) + ";1";
        }
    }
}
