using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace DofusOrigin.Network.Auth
{
    class AuthQueue
    {
        private static Dictionary<AuthClient, int> _clients;

        public static Dictionary<AuthClient, int> GetClients
        {
            get
            {
                return _clients;
            }
        }

        private static List<AuthClient> MustDelete;
        private static Timer _reloadTimer;

        public static void Start()
        {
            _clients = new Dictionary<AuthClient, int>();
            MustDelete = new List<AuthClient>();

            _reloadTimer = new Timer();
            {
                _reloadTimer.Interval = Utilities.Config.GetConfig.GetIntElement("Time_Queue_Reload");
                _reloadTimer.Enabled = true;
                _reloadTimer.Elapsed += new ElapsedEventHandler(RefreshQueue);
                _reloadTimer.Start();
            }

            Utilities.Loggers.StatusLogger.Write("@Queue@ for the servers' list started !");
        }

        public static void AddinQueue(AuthClient client)
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("Add @{0}@ in queue !", client.myIp()));

            _clients.Add(client, _clients.Count + 1);
            client.WaitPosition = (_clients.Count > 1 ? _clients.Count + 1 : 2);
        }

        private static void RefreshQueue(object sender, EventArgs e)
        {
            if (_clients.Count <= 0)
                return;

            var count = 0;

            foreach (var client in _clients.Keys.OrderBy(x => x.WaitPosition))
            {
                if (count < Utilities.Config.GetConfig.GetIntElement("Client_Per_QueueRefresh"))
                {
                    count++;

                    client.CheckAccount();
                    MustDelete.Add(client);
                }
            }

            MustDelete.ForEach(x => _clients.Remove(x));
            MustDelete.Clear();

            foreach (var client in _clients.Keys)
                client.WaitPosition = (_clients.Count > 1 ? _clients.Count + 1 : 2);

            Utilities.Loggers.InfosLogger.Write("@Queue@ refreshed !");
        }
    }
}
