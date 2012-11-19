using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace auth.Network.Auth
{
    class AuthQueue
    {
        public static Dictionary<AuthClient, int> myClients;
        public static Timer myRefreshTimer;
        public static int Confirmed = 0;

        public static void Start()
        {
            myClients = new Dictionary<AuthClient, int>();

            myRefreshTimer = new Timer();
            myRefreshTimer.Interval = Utilities.Config.m_config.GetIntElement("Time_Queue_Reload");
            myRefreshTimer.Enabled = true;
            myRefreshTimer.Elapsed += new ElapsedEventHandler(RefreshQueue);
            myRefreshTimer.Start();

            Utilities.Loggers.m_statusLogger.Write("@Queue@ for the servers' list started !");
        }

        public static void AddinQueue(AuthClient myClient)
        {
            Utilities.Loggers.m_infosLogger.Write(string.Format("Add @{0}@ in queue !", myClient.m_account.myPseudo));
            myClients.Add(myClient, myClients.Count + 1);
            myClient.m_waitPosition = (myClients.Count > 1 ? myClients.Count + 1 : 2);
        }

        static void RefreshQueue(object sender, EventArgs e)
        {
            if (myClients.Count <= 0)
                return;

            var Count = 0;
            var Rest = 0;

            foreach (AuthClient myClient in myClients.Keys)
            {
                if (myClient.m_state != AuthClient.State.Queue) 
                    return;

                if (Count <= Utilities.Config.m_config.GetIntElement("Max_Clients_inQueue"))
                {
                    Count++;
                    Confirmed++;

                    myClient.m_state = AuthClient.State.OnList;
                    myClient.SendInformations();
                }

                else
                {
                    Rest++;
                    myClient.m_waitPosition = (myClients.Count > 1 ? myClients.Count + 1 : 2);
                }
            }

            if (Rest == 0)
                myClients.Clear();

            Utilities.Loggers.m_infosLogger.Write("@Queue@ refreshed !");
        }
    }
}
