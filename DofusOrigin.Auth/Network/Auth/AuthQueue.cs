using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace DofusOrigin.Network.Auth
{
    class AuthQueue
    {
        public static Dictionary<AuthClient, int> m_clients { get; set; }
        public static List<AuthClient> toDelete { get; set; }
        private static Timer m_timer { get; set; }

        public static void Start()
        {
            m_clients = new Dictionary<AuthClient, int>();
            toDelete = new List<AuthClient>();

            m_timer = new Timer();
            {
                m_timer.Interval = Utilities.Config.m_config.GetIntElement("Time_Queue_Reload");
                m_timer.Enabled = true;
                m_timer.Elapsed += new ElapsedEventHandler(RefreshQueue);
                m_timer.Start();
            }

            Utilities.Loggers.m_statusLogger.Write("@Queue@ for the servers' list started !");
        }

        public static void AddinQueue(AuthClient _client)
        {
            Utilities.Loggers.m_infosLogger.Write(string.Format("Add @{0}@ in queue !", _client.myIp()));

            m_clients.Add(_client, m_clients.Count + 1);
            _client.m_waitPosition = (m_clients.Count > 1 ? m_clients.Count + 1 : 2);
        }

        private static void RefreshQueue(object sender, EventArgs e)
        {
            if (m_clients.Count <= 0)
                return;

            var count = 0;

            foreach (var client in m_clients.Keys)
            {
                if (count < Utilities.Config.m_config.GetIntElement("Client_Per_QueueRefresh"))
                {
                    count++;

                    client.CheckAccount();
                    toDelete.Add(client);
                }
            }

            toDelete.ForEach(x => m_clients.Remove(x));
            toDelete.Clear();

            foreach (var client in m_clients.Keys)
                client.m_waitPosition = (m_clients.Count > 1 ? m_clients.Count + 1 : 2);

            Utilities.Loggers.m_infosLogger.Write("@Queue@ refreshed !");
        }
    }
}
