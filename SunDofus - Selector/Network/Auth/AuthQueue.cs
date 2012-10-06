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
            myRefreshTimer.Interval = Utilities.Config.myConfig.GetIntElement("Time_Queue_Reload");
            myRefreshTimer.Enabled = true;
            myRefreshTimer.Elapsed += new ElapsedEventHandler(RefreshQueue);
            myRefreshTimer.Start();

            Utilities.Loggers.StatusLogger.Write("@Queue@ for the servers' list started !");
        }

        public static void AddinQueue(AuthClient myClient)
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("Add @{0}@ in queue !", myClient.myAccount.myPseudo));
            myClients.Add(myClient, myClients.Count + 1);
            myClient.WaitPosition = (myClients.Count > 1 ? myClients.Count + 1 : 2);
        }

        static void RefreshQueue(object sender, EventArgs e)
        {
            if (myClients.Count <= 0)
                return;

            var Count = 0;
            var Rest = 0;

            foreach (AuthClient myClient in myClients.Keys)
            {
                if (myClient.myState != AuthClient.State.Queue) return;
                if (Count <= Utilities.Config.myConfig.GetIntElement("Max_Clients_inQueue"))
                {
                    Count++;
                    Confirmed++;

                    myClient.myState = AuthClient.State.OnList;
                    myClient.SendInformations();
                }
                else
                {
                    Rest++;
                    myClient.WaitPosition = (myClients.Count > 1 ? myClients.Count + 1 : 2);
                }
            }

            if (Rest == 0)
            {
                myClients.Clear();
            }

            Utilities.Loggers.InfosLogger.Write("@Queue@ refreshed !");
        }
    }
}
