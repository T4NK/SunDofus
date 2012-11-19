using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace auth.Network.Auth
{
    class AuthClient : SunDofus.Network.AbstractClient
    {
        public string m_key = "";
        public State m_state;
        public Database.Models.AccountModel m_account;
        public int m_waitPosition = 0;

        object m_packetLocker;

        public AuthClient(SilverSocket Socket) : base(Socket)
        {
            m_packetLocker = new object();

            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);
            this.ReceivedDatas += new ReceiveDatasHandler(this.PacketReceived);

            m_key = Utilities.Basic.RandomString(32);
            m_state = State.Version;

            Send(string.Format("HC{0}", m_key));
        }

        public void Send(string Message)
        {
            this.SendDatas(Message);
            Utilities.Loggers.m_infosLogger.Write(string.Format("Sent to @<{0}>@ : {1}", myIp(), Message));
        }

        public void SendInformations()
        {
            Send(string.Format("Ad{0}",m_account.myPseudo));
            Send(string.Format("Ac{0}", m_account.myCommunauty));
            SendHosts();
            Send(string.Format("AlK{0}", m_account.myLevel));
            Send(string.Format("AQ{0}", m_account.myQuestion));
        }

        public void SendHosts()
        {
            var Packet = "AH";

            foreach (var m_Server in Database.Cache.ServersCache.myServers)
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
            if (Database.Cache.GiftsCache.myGifts.Any(x => x.myTarget == this.m_account.myId))
            {
                var Packet = "";

                foreach (var myGift in Database.Cache.GiftsCache.myGifts.Where(x => x.myTarget == this.m_account.myId))
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
            Utilities.Loggers.m_infosLogger.Write(string.Format("New closed client connection @<{0}>@ !", this.myIp()));

            lock (ServersHandler.m_authServer.myClients)
                ServersHandler.m_authServer.myClients.Remove(this);
        }

        void PacketReceived(string Data)
        {
            Utilities.Loggers.m_infosLogger.Write(string.Format("Receive from client @<{0}>@ : [{1}]", this.myIp(), Data));

            lock (m_packetLocker)
                Parse(Data);
        }

        void Parse(string Data)
        {
            try
            {
                switch (m_state)
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
                Utilities.Loggers.m_errorsLogger.Write(string.Format("Can't parse packet from @<{0}>@ : {1}", myIp(), e.ToString()));
            }
        }

        void CheckVersion(string Data)
        {
            if (Data.Contains(Utilities.Config.m_config.GetStringElement("Login_Version")))
            {
                Utilities.Loggers.m_infosLogger.Write(string.Format("Client @<{0}>@ has good dofus-version !", myIp()));
                m_state = State.Account;
            }
            else
            {
                Utilities.Loggers.m_errorsLogger.Write(string.Format("Client @<{0}>@ has false dofus-version !", myIp()));
                m_state = State.None;
                this.Send(string.Format("AlEv{0}", Utilities.Config.m_config.GetStringElement("Login_Version")));
            }
        }

        void CheckAccount(string Data)
        {
            if (!Data.Contains("#1")) return;
            string[] Infos = Data.Split('#');

            var Username = Infos[0];
            var Password = Infos[1];

            if (Database.Cache.AccountsCache.myAccounts.Any(x => x.myUsername == Username && Utilities.Basic.Encrypt(x.myPassword, m_key) == Password))
            {
                m_account = Database.Cache.AccountsCache.myAccounts.First
                    (x => x.myUsername == Username && Utilities.Basic.Encrypt(x.myPassword, m_key) == Password);

                Utilities.Loggers.m_infosLogger.Write(string.Format("Client @{0}@ authentified !", m_account.myPseudo));

                SendInformations();

                if (AuthQueue.myClients.Count == 0)
                {
                    SendInformations();
                    m_state = State.OnList;
                }
                else if (AuthQueue.myClients.Count >= Utilities.Config.m_config.GetIntElement("Max_Clients_inQueue"))
                {
                    Send("M00\0");
                    this.Disconnect();
                    m_state = State.None;
                }
                else
                {
                    AuthQueue.AddinQueue(this);
                    Send(string.Format("Af{0}|{1}|0|1", (m_waitPosition - AuthQueue.Confirmed), (AuthQueue.myClients.Count >= 2 ? AuthQueue.myClients.Count : 2)));
                    m_state = State.Queue;
                }
            }
            else
            {
                Send("AlEx");
                m_state = State.None;
            }
        }

        void CheckQueue()
        {
            if(m_state == State.Queue)
                Send(string.Format("Aq{0}|{1}|0|1", (m_waitPosition - AuthQueue.Confirmed), (AuthQueue.myClients.Count >= 2 ? AuthQueue.myClients.Count : 2)));
        }

        void CheckServerPacket(string Data)
        {
            switch (Data.Substring(1, 1))
            {
                case "x":

                    var lPacket = "AxK" + m_account.mySubscriptionTime();

                    foreach (var myServer in Database.Cache.ServersCache.myServers)
                    {
                        if (!m_account.myCharacters.ContainsKey(myServer.myID))
                            m_account.myCharacters.Add(myServer.myID, new List<string>());

                        lPacket += string.Format("|{0},{1}", myServer.myID, m_account.myCharacters[myServer.myID].Count);
                    }

                    Send(lPacket);

                    break;

                case "X":

                    var ID = int.Parse(Data.Replace("AX", ""));

                    if (ServersHandler.m_syncServer.myClients.Any(x => x.myServer.myID == ID))
                    {
                        var myServer = ServersHandler.m_syncServer.myClients.First(x => x.myServer.myID == ID);
                        var myKey = Utilities.Basic.RandomString(16);

                        myServer.SendNewTicket(myKey, this);
                        SendNewTicket(myKey, myServer);
                    }

                    break;

                case "F":

                    var pPacket = "AF";

                    foreach (var myServer in Database.Cache.ServersCache.myServers)
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
