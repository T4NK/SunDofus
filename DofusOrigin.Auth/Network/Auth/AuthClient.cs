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
        public string m_key { get; set; }
        public int m_waitPosition { get; set; }

        public AccountModel m_account { get; set; }

        private object m_packetLocker { get; set; }
        private string m_actualInfos { get; set; }
        private int nbPacket { get; set; }

        public AuthClient(SilverSocket _socket) : base(_socket)
        {
            m_packetLocker = new object();

            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);
            this.ReceivedDatas += new ReceiveDatasHandler(this.PacketReceived);

            m_key = Utilities.Basic.RandomString(32);
            nbPacket = 0;

            Send(string.Format("HC{0}", m_key));
        }

        public void Send(string _message)
        {
            this.SendDatas(_message);
            Utilities.Loggers.m_infosLogger.Write(string.Format("Sent to @<{0}>@ : {1}", myIp(), _message));
        }

        public void SendInformations()
        {
            Send(string.Format("Ad{0}",m_account.m_pseudo));
            Send(string.Format("Ac{0}", m_account.m_communauty));
            RefreshHosts();
            Send(string.Format("AlK{0}", m_account.m_level));
            Send(string.Format("AQ{0}", m_account.m_question));
        }

        public void RefreshHosts()
        {
            var packet = string.Format("AH{0}",
                string.Join("|", Database.Cache.ServersCache.m_servers));

            Send(packet);
        }

        private void Disconnected()
        {
            Utilities.Loggers.m_infosLogger.Write(string.Format("New closed client connection @<{0}>@ !", this.myIp()));

            lock (ServersHandler.m_authServer.m_clients)
                ServersHandler.m_authServer.m_clients.Remove(this);
        }

        private void PacketReceived(string _datas)
        {
            Utilities.Loggers.m_infosLogger.Write(string.Format("Receive from client @<{0}>@ : [{1}]", this.myIp(), _datas));
            nbPacket++;

            lock (m_packetLocker)
                Parse(_datas);
        }

        private void Parse(string _datas)
        {
            try
            {
                if (nbPacket == 1 && _datas.Length > 2)
                {
                    if (_datas.Contains(Utilities.Config.m_config.GetStringElement("Login_Version")))
                    {
                        if (AuthQueue.m_clients.Count >= Utilities.Config.m_config.GetIntElement("Max_Clients_inQueue"))
                        {
                            Send("M00\0");
                            this.Disconnect();
                        }
                        else
                            AuthQueue.AddinQueue(this);
                    }
                    else
                    {
                        Utilities.Loggers.m_errorsLogger.Write(string.Format("Client @<{0}>@ has false dofus-version !", myIp()));
                        this.Send(string.Format("AlEv{0}", Utilities.Config.m_config.GetStringElement("Login_Version")));
                    }
                }
                else if (nbPacket == 2 && _datas.Length > 2)
                    m_actualInfos = _datas;
                else
                {
                    if (_datas.Substring(0, 1) != "A")
                        return;

                    var packet = "";

                    switch (_datas.Substring(1, 1))
                    {
                        case "f":

                            Send(string.Format("Af{0}|{1}|0|1", (m_waitPosition), (AuthQueue.m_clients.Count >= 2 ? AuthQueue.m_clients.Count : 2)));
                            return;

                        case "F":

                            packet = string.Format("AF{0}", Database.Cache.ServersCache.m_servers.First(x => x.m_clients.Contains(_datas.Substring(2))));
                            Send(packet);
                            return;

                        case "x":

                            packet = string.Format("AxK{0}", m_account.GetSubscriptionTime());

                            foreach (var server in Database.Cache.ServersCache.m_servers)
                            {
                                if (!m_account.m_characters.ContainsKey(server.m_id))
                                    m_account.m_characters.Add(server.m_id, new List<string>());

                                packet += string.Format("|{0},{1}", server.m_id, m_account.m_characters[server.m_id].Count);
                            }

                            Send(packet);
                            return;

                        case "X":

                            var id = int.Parse(_datas.Substring(2));

                            if (ServersHandler.m_syncServer.m_clients.Any(x => x.m_server.m_id == id))
                            {
                                var server = ServersHandler.m_syncServer.m_clients.First(x => x.m_server.m_id == id);
                                var key = Utilities.Basic.RandomString(16);

                                server.SendTicket(key, this);
                        
                                packet = string.Format("AYK{0}:{1};{2}", server.m_server.m_ip, server.m_server.m_port, key);
                                Send(packet);
                                return;
                            }

                            Send("BN");
                            return;
                    }
                }
            }
            catch (Exception e)
            {
                Utilities.Loggers.m_errorsLogger.Write(string.Format("Can't parse packet from @<{0}>@ : {1}", myIp(), e.ToString()));
            }
        }

        public void CheckAccount()
        {
            if (!m_actualInfos.Contains("#1"))
                return;

            var infos = m_actualInfos.Split('#');
            var username = infos[0];
            var password = infos[1];

            if (Database.Cache.AccountsCache.m_accounts.Any(x => x.m_username == username && Utilities.Basic.Encrypt(x.m_password, m_key) == password))
            {
                m_account = Database.Cache.AccountsCache.m_accounts.First
                    (x => x.m_username == username && Utilities.Basic.Encrypt(x.m_password, m_key) == password);

                Utilities.Loggers.m_infosLogger.Write(string.Format("Client @{0}@ authentified !", m_account.m_pseudo));

                SendInformations();
            }
            else
            {
                Send("AlEx");
            }
        }
    }
}
