using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace auth.Network.Auth
{
    class AuthQueue
    {
        public static Dictionary<AuthClient, int> m_clients { get; set; }
        private static Timer m_timer { get; set; }

        public static long m_confirmed { get; set; }
        private static long m_quota { get; set; }

        public static void Start()
        {
            m_clients = new Dictionary<AuthClient, int>();
            m_confirmed = 0;
            m_quota = 0;

            m_timer = new Timer();
            {
                m_timer.Interval = Utilities.Config.m_config.GetIntElement("Time_Queue_Reload");
                m_timer.Enabled = true;
                m_timer.Elapsed += new ElapsedEventHandler(RefreshQueue);
                m_timer.Start();
            }

            Utilities.Loggers.m_statusLogger.Write("@Queue@ for the servers' list started !");
        }

        public static bool MustAdd()
        {
            return (Time() <= 0 ? true : false);
        }

        public static void AddinQueue(AuthClient _client)
        {
            m_quota = (Environment.TickCount + 5000);

            Utilities.Loggers.m_infosLogger.Write(string.Format("Add @{0}@ in queue !", _client.m_account.m_pseudo));

            m_clients.Add(_client, m_clients.Count + 1);
            _client.m_waitPosition = (m_clients.Count > 1 ? m_clients.Count + 1 : 2);
        }

        private static void RefreshQueue(object sender, EventArgs e)
        {
            if (m_clients.Count <= 0)
                return;

            var count = 0;
            var rest = 0;

            foreach (var client in m_clients.Keys)
            {
                if (client.m_state != AuthClient.State.OnQueue) 
                    continue;

                if (count <= Utilities.Config.m_config.GetIntElement("Client_Per_QueueRefresh"))
                {
                    count++;
                    m_confirmed++;

                    client.m_state = AuthClient.State.OnServerList;
                    client.SendInformations();
                }
                else
                {
                    rest++;
                    client.m_waitPosition = (m_clients.Count > 1 ? m_clients.Count + 1 : 2);
                }
            }

            if (rest == 0)
            {
                m_clients.Clear();
                m_confirmed = 0;
            }

            Utilities.Loggers.m_infosLogger.Write("@Queue@ refreshed !");
        }

        private static long Time()
        {
            //TODO améliorer la gestion de la queue
            var test = (long)Math.Ceiling((double)((m_quota - Environment.TickCount) / 1000));
            //Console.WriteLine(test);
            return test;
        }
    }
}
