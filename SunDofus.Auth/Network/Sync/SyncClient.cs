using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace auth.Network.Sync
{
    class SyncClient : SunDofus.AbstractClient
    {
        State myState;
        public Database.Models.ServerModel myServer = null;

        object PacketLocker;

        public SyncClient(SilverSocket Socket)
            : base(Socket)
        {
            this.RaiseDataArrivalEvent += new DataArrivalEvent(this.PacketsReceived);
            this.RaiseClosedEvent += new OnClosedEvent(this.Disconnected);

            myState = State.Auth;
            PacketLocker = new object();

            Send("HCS");
        }

        public void SendNewTicket(string myKey, Auth.AuthClient myClient)
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append("ANTS|");
            Builder.Append(myKey).Append("|");
            Builder.Append(myClient.myAccount.myId).Append("|");
            Builder.Append(myClient.myAccount.myPseudo).Append("|");
            Builder.Append(myClient.myAccount.myQuestion).Append("|");
            Builder.Append(myClient.myAccount.myAnswer).Append("|");
            Builder.Append(myClient.myAccount.myLevel).Append("|");
            Builder.Append(myClient.myAccount.myBaseChar).Append("|");
            Builder.Append(myClient.myAccount.mySubscriptionTime()).Append("|");
            Builder.Append(myClient.MyGifts());

            Send(Builder.ToString());
        }

        public void Send(string Message)
        {
            this.meSend(Message);
            Utilities.Loggers.InfosLogger.Write(string.Format("Sent to {0} : {1}", myIp(), Message));
        }

        void PacketsReceived(string Data)
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("Receive from sync @<{0}>@ : [{1}]", myIp(), Data));

            lock (PacketLocker)
                Parse(Data);
        }

        void Disconnected()
        {
            ChangeState(State.Disconnected);
            Utilities.Loggers.InfosLogger.Write(string.Format("New closed sync connection @<{0}>@ !", this.myIp()));

            lock (ServersHandler.mySyncServer.myClients)
                ServersHandler.mySyncServer.myClients.Remove(this);
        }

        void Parse(string Data)
        {
            try
            {
                string[] Packet = Data.Split('|');

                switch (Packet[0])
                {
                    case "SAI":
                        //Sync Account Identification
                        Authentication(int.Parse(Packet[1]), Packet[2], int.Parse(Packet[3]));
                        break;

                    case "SDAC":
                        //Sync Deleted Account Character
                        SyncAction.UpdateCharacters(int.Parse(Packet[1]), Packet[2], myServer.myID);  
                        break;

                    case "SNAC":
                        //Sync New Account Character
                        SyncAction.UpdateCharacters(int.Parse(Packet[1]), Packet[2], myServer.myID);
                        break;

                    case "SNC":
                        //Sync New Connected
                        myServer.myClients.Add(Packet[1]);
                        break;

                    case "SND":
                        //Sync New Disconnected 
                        myServer.myClients.Remove(Packet[1]);                     
                        break;

                    case "SNDG":
                        //Sync New Deleted Gift  
                        SyncAction.DeleteGift(int.Parse(Packet[1]), int.Parse(Packet[2]));
                        break;

                    case "SNLC":
                        //Sync New List Connected
                        break;

                    case "SSM":
                        //Sync Start Maintenance
                        ChangeState(State.Maintenance);
                        break;

                    case "STM":
                        //Sync Stop Maintenance
                        ChangeState(State.Connected);
                        break;
                }
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot parse sync packet : {0}", e.ToString()));
            }
        }

        void Authentication(int ServerId, string ServerIp, int ServerPort)
        {
            if (Database.Cache.ServersCache.myServers.Any(x => x.myID == ServerId && x.myIp == ServerIp && x.myPort == ServerPort && x.myState == 0))
            {
                var myServer2 = Database.Cache.ServersCache.myServers.First(x => x.myID == ServerId && x.myIp == ServerIp && x.myPort == ServerPort && x.myState == 0);

                if (!myIp().Contains(ServerIp))
                {
                    Disconnect();
                    return;
                }

                myServer = myServer2;

                Send("HCSS");
                
                ChangeState(SyncClient.State.Connected);
                Utilities.Loggers.InfosLogger.Write(string.Format("Sync @<{0}>@ authentified !", this.myIp()));
            }
            else
                Disconnect();
        }

        void ChangeState(State NewState)
        {
            this.myState = NewState;

            if (myServer == null) return;
            switch (this.myState)
            {
                case State.Auth:
                    myServer.myState = 0;
                    break;

                case State.Connected:
                    myServer.myState = 1;
                    break;

                case State.Disconnected:
                    myServer.myState = 0;
                    break;

                case State.Maintenance:
                    myServer.myState = 2;
                    break;
            }

            ServersHandler.myAuthServer.RefreshAllHosts();
        }

        enum State
        {
            Auth,
            Connected,
            Disconnected,
            Maintenance,
        }
    }
}
