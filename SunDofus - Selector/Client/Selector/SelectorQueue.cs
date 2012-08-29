using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace selector.Client
{
    class SelectorQueue
    {
        public static Dictionary<int,SelectorClient> myClients;
        public static long Confirmed = 0;
        public static Timer RefreshTimer;

        public static void StartInstance()
        {
            myClients = new Dictionary<int, SelectorClient>();
            RefreshTimer = new Timer();
            RefreshTimer.Interval = Config.ConfigurationManager.GetInt("TimeReloadQueue");
            RefreshTimer.Elapsed += new ElapsedEventHandler(RefreshQueue);
            RefreshTimer.Start();
        }

        public static void AddToQueue(SelectorClient client)
        {
            myClients.Add(myClients.Count + 1, client);
            client.WaitPosition = (myClients.Count > 1 ? myClients.Count + 1 : 2);
        }

        public static void RefreshQueue(object sender, EventArgs e)
        {
            int Count = 0;
            int Rest = 0;
            foreach (int i in myClients.Keys)
            {
                SelectorClient client = myClients[i];
                if (client.m_State != SelectorClient.State.Queue) continue;
                if (Count <= Config.ConfigurationManager.GetInt("MaxClientsForQueue"))
                {
                    Confirmed += 1;
                    Count += 1;

                    client.m_State = SelectorClient.State.OnList;
                    client.SendInformations();
                }
                else
                {
                    Rest += 1;
                    client.WaitPosition = (myClients.Count > 1 ? myClients.Count + 1 : 2);
                    RefreshQueue(client);
                }
            }

            if (Rest == 0)
            {
                myClients.Clear();
                Confirmed = 0;
            }
        }

        public static void RefreshQueue(SelectorClient client)
        {
            client.Send("Af" + (client.WaitPosition - Confirmed) + "|"
                + (myClients.Count >= 2 ? myClients.Count : 2) + "|0|1");
        }
    }
}
