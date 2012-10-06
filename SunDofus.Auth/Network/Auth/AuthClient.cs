using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace auth.Network.Auth
{
    class AuthClient : SunDofus.AbstractClient
    {
        public string myKey = "";
        public State myState;
        public Database.Models.AccountModel myAccount;
        public int WaitPosition = 0;

        object PacketLocker;

        public AuthClient(SilverSocket Socket) : base(Socket)
        {
            PacketLocker = new object();

            this.RaiseClosedEvent += new OnClosedEvent(this.Disconnected);
            this.RaiseDataArrivalEvent += new DataArrivalEvent(this.PacketReceived);

            myKey = Utilities.Basic.RandomString(32);
            myState = State.Version;

            Send(string.Format("HC{0}", myKey));
        }

        public void SendInformations()
        {
            Send(string.Format("Ad{0}",myAccount.myPseudo));
            Send(string.Format("Ac{0}", myAccount.myCommunauty));
            SendHosts();
            Send(string.Format("AlK{0}", myAccount.myLevel));
            Send(string.Format("AQ{0}", myAccount.myQuestion));
        }

        public void SendHosts()
        {
            var Packet = "AH";

            foreach (Database.Models.ServerModel m_Server in Database.Cache.ServersCache.myServers)
            {
                Packet += m_Server.ToString();
            }

            if (Packet != "AH")
                Send(Packet.Substring(0, Packet.Length - 1));
            else
                Send("AH");
        }

        public void SendNewTicket(string myKey, Sync.SyncClient myClient)
        {
            Send(string.Format("AYK{0}:{1};{2}", myClient.myServer.myIp, myClient.myServer.myPort, myKey));
        }

        public string MyGifts()
        {
            if (Database.Cache.GiftsCache.myGifts.Any(x => x.myTarget == this.myAccount.myId))
            {
                var Packet = "";

                foreach (Database.Models.GiftModel myGift in Database.Cache.GiftsCache.myGifts.Where(x => x.myTarget == this.myAccount.myId))
                    Packet += string.Format("{0}~{1}~{2}~{3}+", myGift.myId, myGift.myTitle, myGift.myMessage, myGift.myItemID);

                return Packet.Substring(0, Packet.Length - 1); ;
            }

            return "";
        }

        public enum State
        {
            Version,
            Account,
            Queue,
            OnList,
            None,
        }

        void Disconnected()
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("New closed client connection @<{0}>@ !", this.myIp()));

            lock (ServersHandler.myAuthServer.myClients)
                ServersHandler.myAuthServer.myClients.Remove(this);
        }

        void PacketReceived(string Data)
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("Receive from client @<{0}>@ : [{1}]", this.myIp(), Data));

            lock (PacketLocker)
                Parse(Data);
        }

        void Parse(string Data)
        {
            try
            {
                switch (myState)
                {
                    case State.Version:
                        CheckVersion(Data);
                        break;

                    case State.Account:
                        CheckAccount(Data);
                        break;

                    case State.Queue:
                        CheckQueue();
                        break;

                    case State.OnList:
                        CheckServerPacket(Data);
                        break;

                    case State.None:
                        return;
                }
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Can't parse packet from @<{0}>@ : {1}", myIp(), e.ToString()));
            }
        }

        void CheckVersion(string Data)
        {
            if (Data.Contains(Utilities.Config.myConfig.GetStringElement("Login_Version")))
            {
                Utilities.Loggers.InfosLogger.Write(string.Format("Client @<{0}>@ has good dofus-version !", myIp()));
                myState = State.Account;
            }
            else
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Client @<{0}>@ has false dofus-version !", myIp()));
                myState = State.None;
                this.Send(string.Format("AlEv{0}", Utilities.Config.myConfig.GetStringElement("Login_Version")));
            }
        }

        void CheckAccount(string Data)
        {
            if (!Data.Contains("#1")) return;
            string[] Infos = Data.Split('#');

            var Username = Infos[0];
            var Password = Infos[1];

            if (Database.Cache.AccountsCache.myAccounts.Any(x => x.myUsername == Username && Utilities.Basic.Encrypt(x.myPassword, myKey) == Password))
            {
                myAccount = Database.Cache.AccountsCache.myAccounts.First
                    (x => x.myUsername == Username && Utilities.Basic.Encrypt(x.myPassword, myKey) == Password);

                Utilities.Loggers.InfosLogger.Write(string.Format("Client @{0}@ authentified !", myAccount.myPseudo));

                SendInformations();

                if (AuthQueue.myClients.Count == 0)
                {
                    SendInformations();
                    myState = State.OnList;
                }
                else if (AuthQueue.myClients.Count >= Utilities.Config.myConfig.GetIntElement("Max_Clients_inQueue"))
                {
                    Send("M00\0");
                    this.Disconnect();
                    myState = State.None;
                }
                else
                {
                    AuthQueue.AddinQueue(this);
                    Send(string.Format("Af{0}|{1}|0|1", (WaitPosition - AuthQueue.Confirmed), (AuthQueue.myClients.Count >= 2 ? AuthQueue.myClients.Count : 2)));
                    myState = State.Queue;
                }
            }
            else
            {
                Send("AlEx");
                myState = State.None;
            }
        }

        void CheckQueue()
        {
            if(myState == State.Queue)
                Send(string.Format("Aq{0}|{1}|0|1", (WaitPosition - AuthQueue.Confirmed), (AuthQueue.myClients.Count >= 2 ? AuthQueue.myClients.Count : 2)));
        }

        void CheckServerPacket(string Data)
        {
            switch (Data.Substring(1, 1))
            {
                case "x":

                    string lPacket = "AxK" + myAccount.mySubscriptionTime();

                    foreach (Database.Models.ServerModel myServer in Database.Cache.ServersCache.myServers)
                    {
                        if (!myAccount.myCharacters.ContainsKey(myServer.myID))
                            myAccount.myCharacters.Add(myServer.myID, new List<string>());

                        lPacket += string.Format("|{0},{1}", myServer.myID, myAccount.myCharacters[myServer.myID].Count);
                    }

                    Send(lPacket);

                    break;

                case "X":

                    int ID = int.Parse(Data.Replace("AX", ""));

                    if (ServersHandler.mySyncServer.myClients.Any(x => x.myServer.myID == ID))
                    {
                        Sync.SyncClient myServer = ServersHandler.mySyncServer.myClients.First(x => x.myServer.myID == ID);

                        string myKey = Utilities.Basic.RandomString(16);
                        myServer.SendNewTicket(myKey, this);
                        SendNewTicket(myKey, myServer);
                    }

                    break;

                case "F":

                    string pPacket = "AF";

                    foreach (Database.Models.ServerModel myServer in Database.Cache.ServersCache.myServers)
                    {
                        if (myServer.myClients.Contains(Data.Replace("AF", "")))
                            pPacket += string.Format("{0};", myServer.myID);
                    }

                    if (pPacket == "AF")
                        Send("AF");
                    else
                        Send(pPacket.Substring(0, pPacket.Length - 1));

                    break;
            }
        }
    }
}
