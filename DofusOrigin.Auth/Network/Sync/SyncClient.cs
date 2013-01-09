using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using auth.Database.Models;

namespace auth.Network.Sync
{
    class SyncClient : DofusOrigin.Network.TCPClient
    {
        private State m_state { get; set; }
        public ServerModel m_server { get; set; }

        object m_packetLocker;

        public SyncClient(SilverSocket _socket)
            : base(_socket)
        {
            this.ReceivedDatas += new ReceiveDatasHandler(this.PacketsReceived);
            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);

            m_state = State.OnAuthentication;
            m_server = null;
            m_packetLocker = new object();

            Send("HCS");
        }

        public void SendTicket(string _key, Auth.AuthClient _client)
        {
            var Builder = new StringBuilder();
            {
                Builder.Append("ANTS|");
                Builder.Append(_key).Append("|");
                Builder.Append(_client.m_account.m_id).Append("|");
                Builder.Append(_client.m_account.m_pseudo).Append("|");
                Builder.Append(_client.m_account.m_question).Append("|");
                Builder.Append(_client.m_account.m_answer).Append("|");
                Builder.Append(_client.m_account.m_level).Append("|");
                Builder.Append(string.Join(",", _client.m_account.m_characters[m_server.m_id].ToArray())).Append("|");
                Builder.Append(_client.m_account.GetSubscriptionTime()).Append("|");
                Builder.Append(string.Join("+", Database.Cache.GiftsCache.m_gifts.Where(x => x.m_target == _client.m_account.m_id)));
            }

            Send(Builder.ToString());
        }

        public void Send(string _message)
        {
            this.SendDatas(_message);
            Utilities.Loggers.m_infosLogger.Write(string.Format("Sent to {0} : {1}", myIp(), _message));
        }

        private void PacketsReceived(string _datas)
        {
            Utilities.Loggers.m_infosLogger.Write(string.Format("Receive from sync @<{0}>@ : [{1}]", myIp(), _datas));

            lock (m_packetLocker)
                Parse(_datas);
        }

        private void Disconnected()
        {
            ChangeState(State.OnDisconnected);
            Utilities.Loggers.m_infosLogger.Write(string.Format("New closed sync connection @<{0}>@ !", this.myIp()));

            lock (ServersHandler.m_syncServer.m_clients)
                ServersHandler.m_syncServer.m_clients.Remove(this);
        }

        private void Parse(string _datas)
        {
            try
            {
                var packet = _datas.Split('|');

                switch (packet[0])
                {
                    case "SAI":
                        //Sync Account Identification
                        Authentication(int.Parse(packet[1]), packet[2], int.Parse(packet[3]));
                        break;

                    case "SDAC":
                        //Sync Deleted Account Character
                        SyncAction.UpdateCharacters(int.Parse(packet[1]), packet[2], m_server.m_id, false);  
                        break;

                    case "SNAC":
                        //Sync New Account Character
                        SyncAction.UpdateCharacters(int.Parse(packet[1]), packet[2], m_server.m_id);
                        break;

                    case "SNC":
                        //Sync New Connected
                        if(!m_server.m_clients.Contains(packet[1]))
                            m_server.m_clients.Add(packet[1]);
                        break;

                    case "SND":
                        //Sync New Disconnected 
                        if (m_server.m_clients.Contains(packet[1]))
                            m_server.m_clients.Remove(packet[1]);                     
                        break;

                    case "SNDG":
                        //Sync New Deleted Gift  
                        SyncAction.DeleteGift(int.Parse(packet[1]), int.Parse(packet[2]));
                        break;

                    case "SNLC":
                        //Sync New List Connected
                        ParseListConnected(_datas);
                        break;

                    case "SSM":
                        //Sync Start Maintenance
                        ChangeState(State.OnMaintenance);
                        break;

                    case "STM":
                        //Sync Stop Maintenance
                        ChangeState(State.OnConnected);
                        break;
                }
            }
            catch (Exception e)
            {
                Utilities.Loggers.m_errorsLogger.Write(string.Format("Cannot parse sync packet : {0}", e.ToString()));
            }
        }

        private void Authentication(int _serverId, string _serverIp, int _serverPort)
        {
            if (Database.Cache.ServersCache.m_servers.Any(x => x.m_id == _serverId && x.m_ip == _serverIp && x.m_port == _serverPort && x.m_state == 0))
            {
                var requieredServer = Database.Cache.ServersCache.m_servers.First(x => x.m_id == _serverId && x.m_ip == _serverIp && x.m_port == _serverPort && x.m_state == 0);

                if (!myIp().Contains(_serverIp))
                {
                    Disconnect();
                    return;
                }

                m_server = requieredServer;

                Send("HCSS");                
                ChangeState(SyncClient.State.OnConnected);

                Utilities.Loggers.m_infosLogger.Write(string.Format("Sync @<{0}>@ authentified !", this.myIp()));
            }
            else
                Disconnect();
        }

        private void ChangeState(State _state)
        {
            this.m_state = _state;

            if (m_server == null) return;
            switch (this.m_state)
            {
                case State.OnAuthentication:
                    m_server.m_state = 0;
                    break;

                case State.OnConnected:
                    m_server.m_state = 1;
                    break;

                case State.OnDisconnected:
                    m_server.m_state = 0;
                    break;

                case State.OnMaintenance:
                    m_server.m_state = 2;
                    break;
            }

            ServersHandler.m_authServer.m_clients.ForEach(x => x.RefreshHosts());
        }

        private void ParseListConnected(string _datas)
        {
            var packet = _datas.Substring(5).Split('|');

            foreach (var pseudo in packet)
            {
                if (!m_server.m_clients.Contains(pseudo))
                    m_server.m_clients.Add(pseudo);
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
