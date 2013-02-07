﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using DofusOrigin.Database.Models;

namespace DofusOrigin.Network.Sync
{
    class SyncClient : DofusOrigin.Network.TCPClient
    {
        public ServerModel Server;

        private State _state;
        private object _packetLocker;

        public SyncClient(SilverSocket socket)
            : base(socket)
        {
            this.ReceivedDatas += new ReceiveDatasHandler(this.PacketsReceived);
            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);

            _state = State.OnAuthentication;
            _packetLocker = new object();

            Server = null;

            Send("HCS");
        }

        public void SendTicket(string key, Auth.AuthClient client)
        {
            var Builder = new StringBuilder();
            {
                Builder.Append("ANTS|");
                Builder.Append(key).Append("|");
                Builder.Append(client.Account.ID).Append("|");
                Builder.Append(client.Account.Pseudo).Append("|");
                Builder.Append(client.Account.Question).Append("|");
                Builder.Append(client.Account.Answer).Append("|");
                Builder.Append(client.Account.Level).Append("|");
                Builder.Append(string.Join(",", client.Account.Characters[Server.ID].ToArray())).Append("|");
                Builder.Append(client.Account.SubscriptionTime()).Append("|");
                Builder.Append(string.Join("+", Database.Cache.GiftsCache.Cache.Where(x => x.Target == client.Account.ID)));
            }

            Send(Builder.ToString());
        }

        public void Send(string message)
        {
            this.SendDatas(message);
            Utilities.Loggers.InfosLogger.Write(string.Format("Sent to {0} : {1}", myIp(), message));
        }

        private void PacketsReceived(string datas)
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("Receive from sync @<{0}>@ : [{1}]", myIp(), datas));

            lock (_packetLocker)
                Parse(datas);
        }

        private void Disconnected()
        {
            ChangeState(State.OnDisconnected);
            Utilities.Loggers.InfosLogger.Write(string.Format("New closed sync connection @<{0}>@ !", this.myIp()));

            lock (ServersHandler.SyncServer.GetClients)
                ServersHandler.SyncServer.GetClients.Remove(this);
        }

        private void Parse(string datas)
        {
            try
            {
                var packet = datas.Split('|');

                switch (packet[0])
                {
                    case "SAI":

                        Authentication(int.Parse(packet[1]), packet[2], int.Parse(packet[3]));
                        break;

                    case "SDAC":

                        SyncAction.UpdateCharacters(int.Parse(packet[1]), packet[2], Server.ID, false);  
                        break;

                    case "SNAC":

                        SyncAction.UpdateCharacters(int.Parse(packet[1]), packet[2], Server.ID);
                        break;

                    case "SNC":

                        if(!Server.GetClients.Contains(packet[1]))
                            Server.GetClients.Add(packet[1]);
                        break;

                    case "SND":

                        if (Server.GetClients.Contains(packet[1]))
                            Server.GetClients.Remove(packet[1]);                     
                        break;

                    case "SNDG":

                        SyncAction.DeleteGift(int.Parse(packet[1]), int.Parse(packet[2]));
                        break;

                    case "SNLC":

                        ParseListConnected(datas);
                        break;

                    case "SSM":

                        ChangeState(State.OnMaintenance);
                        break;

                    case "STM":

                        ChangeState(State.OnConnected);
                        break;
                }
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot parse sync packet : {0}", e.ToString()));
            }
        }

        private void Authentication(int serverId, string serverIp, int serverPort)
        {
            if (Database.Cache.ServersCache.Cache.Any(x => x.ID == serverId && x.IP == serverIp && x.Port == serverPort && x.State == 0))
            {
                var requieredServer = Database.Cache.ServersCache.Cache.First(x => x.ID == serverId && x.IP == serverIp && x.Port == serverPort && x.State == 0);

                if (!myIp().Contains(serverIp))
                {
                    Disconnect();
                    return;
                }

                Server = requieredServer;

                Send("HCSS");                
                ChangeState(SyncClient.State.OnConnected);

                Utilities.Loggers.InfosLogger.Write(string.Format("Sync @<{0}>@ authentified !", this.myIp()));
            }
            else
                Disconnect();
        }

        private void ChangeState(State state)
        {
            this._state = state;

            if (Server == null) 
                return;

            switch (this._state)
            {
                case State.OnAuthentication:
                    Server.State = 0;
                    break;

                case State.OnConnected:
                    Server.State = 1;
                    break;

                case State.OnDisconnected:
                    Server.State = 0;
                    break;

                case State.OnMaintenance:
                    Server.State = 2;
                    break;
            }

            ServersHandler.AuthServer.GetClients.ForEach(x => x.RefreshHosts());
        }

        private void ParseListConnected(string _datas)
        {
            var packet = _datas.Substring(5).Split('|');

            foreach (var pseudo in packet)
            {
                if (!Server.GetClients.Contains(pseudo))
                    Server.GetClients.Add(pseudo);
            }
        }

        private enum State
        {
            OnAuthentication,
            OnConnected,
            OnDisconnected,
            OnMaintenance,
        }
    }
}
