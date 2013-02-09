using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using DofusOrigin.Database.Models;

namespace DofusOrigin.Network.Auth
{
    class AuthClient : DofusOrigin.Network.TCPClient
    {
        public int _waitPosition;

        public int WaitPosition
        {
            get
            {
                return _waitPosition;
            }
            set
            {
                _waitPosition = value;
            }
        }

        public AccountModel Account;
        private AccountState State;

        private object _packetLocker;
        private string _actualInfos;
        private string _key;

        public AuthClient(SilverSocket socket) : base(socket)
        {
            _packetLocker = new object();

            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);
            this.ReceivedDatas += new ReceiveDatasHandler(this.PacketReceived);

            _key = Utilities.Basic.RandomString(32);

            Send(string.Format("HC{0}", _key));
        }

        public void Send(string message)
        {
            lock(_packetLocker)
                this.SendDatas(message);

            Utilities.Loggers.InfosLogger.Write(string.Format("Sent to @<{0}>@ : {1}", myIp(), message));
        }

        public void SendInformations()
        {
            Send(string.Format("Ad{0}",Account.Pseudo));
            Send(string.Format("Ac{0}", Account.Communauty));
            RefreshHosts();
            Send(string.Format("AlK{0}", Account.Level));
            Send(string.Format("AQ{0}", Account.Question));
        }

        public void RefreshHosts()
        {
            lock (Database.Cache.ServersCache.Cache)
            {
                var packet = string.Format("AH{0}",
                    string.Join("|", Database.Cache.ServersCache.Cache));

                Send(packet);
            }
        }

        public void CheckAccount()
        {
            if (!_actualInfos.Contains("#1"))
                return;

            var infos = _actualInfos.Split('#');
            var username = infos[0];
            var password = infos[1];

            var requestedAccount = Database.Cache.AccountsCache.LoadAccount(username);

            if (requestedAccount != null && Utilities.Basic.Encrypt(requestedAccount.Password, _key) == password)
            {
                Account = requestedAccount;

                Utilities.Loggers.InfosLogger.Write(string.Format("Client @{0}@ authentified !", Account.Pseudo));
                State = AccountState.OnServersList;

                SendInformations();
            }
            else
            {
                Send("AlEx");
            }
        }

        private void Disconnected()
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("New closed client connection @<{0}>@ !", this.myIp()));

            lock (ServersHandler.AuthServer.GetClients)
                ServersHandler.AuthServer.GetClients.Remove(this);
        }

        private void PacketReceived(string datas)
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("Receive from client @<{0}>@ : [{1}]", this.myIp(), datas));

            lock (_packetLocker)
                Parse(datas);
        }

        private void Parse(string datas)
        {
            if (datas.Contains("#1"))
            {
                _actualInfos = datas;
                return;
            }

            switch (State)
            {
                case AccountState.OnCheckingVersion:

                    if (datas.Contains(Utilities.Config.GetConfig.GetStringElement("Login_Version")))
                    {
                        if (AuthQueue.GetClients.Count >= Utilities.Config.GetConfig.GetIntElement("Max_Clients_inQueue"))
                        {
                            Send("M00\0");
                            this.Disconnect();
                        }
                        else
                        {
                            State = AccountState.OnCheckingQueue;
                            AuthQueue.AddinQueue(this);
                        }
                    }
                    else
                    {
                        Utilities.Loggers.ErrorsLogger.Write(string.Format("Client @<{0}>@ has false dofus-version !", myIp()));
                        this.Send(string.Format("AlEv{0}", Utilities.Config.GetConfig.GetStringElement("Login_Version")));
                    }

                    return;

                case AccountState.OnCheckingQueue:

                    lock(AuthQueue.GetClients)
                        Send(string.Format("Af{0}|{1}|0|2", (WaitPosition), (AuthQueue.GetClients.Count > 2 ? AuthQueue.GetClients.Count : 3)));

                    return;

                case AccountState.OnServersList:

                    ParseListPacket(datas);

                    return;
            }
        }

        private void ParseListPacket(string initialPacket)
        {
            if (initialPacket.Substring(0, 1) != "A")
                return;

            var packet = "";

            switch (initialPacket[1])
            {
                case 'F':

                    lock (Database.Cache.ServersCache.Cache)
                    {
                        if (Database.Cache.ServersCache.Cache.Any(x => x.GetClients.Contains(initialPacket.Substring(2))))
                        {
                            packet = string.Format("AF{0}", Database.Cache.ServersCache.Cache.First(x => x.GetClients.Contains(initialPacket.Substring(2))).ID);
                            Send(packet);
                        }
                        Send("AF");
                    }

                    return;

                case 'x':

                    lock (Database.Cache.ServersCache.Cache)
                    {
                        packet = string.Format("AxK{0}", Account.SubscriptionTime());

                        foreach (var server in Database.Cache.ServersCache.Cache)
                        {
                            if (!Account.Characters.ContainsKey(server.ID))
                                Account.Characters.Add(server.ID, new List<string>());

                            packet += string.Format("|{0},{1}", server.ID, Account.Characters[server.ID].Count);
                        }

                        Send(packet);
                    }

                    return;

                case 'X':

                    lock (Database.Cache.ServersCache.Cache)
                    {
                        var id = 0;
                        try
                        {
                            id = int.Parse(initialPacket.Substring(2));
                        }
                        catch { return; }

                        if (ServersHandler.SyncServer.GetClients.Any(x => x.Server.ID == id))
                        {
                            var server = ServersHandler.SyncServer.GetClients.First(x => x.Server.ID == id);
                            var key = Utilities.Basic.RandomString(16);

                            server.SendTicket(key, this);

                            packet = string.Format("AYK{0}:{1};{2}", server.Server.IP, server.Server.Port, key);
                            Send(packet);
                            return;
                        }

                        Send("BN");
                    }

                    return;
            }
        }

        private enum AccountState
        {
            OnCheckingVersion,
            OnCheckingQueue,
            OnServersList,
        }
    }
}
