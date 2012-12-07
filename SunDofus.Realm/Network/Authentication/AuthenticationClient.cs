using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using System.Timers;

namespace realm.Network.Authentication
{
    class AuthenticationClient : SunDofus.Network.TCPClient
    {
        public AuthenticationClient(Database.Models.Clients.AuthClientModel _model)
            : base(new SilverSocket())
        {
            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);
            this.ReceivedDatas += new ReceiveDatasHandler(this.DatasArrival);
            this.ConnectFailed += new ConnectFailedHandler(this.FailedToConnect);

            m_timer = new Timer();
            m_timer.Interval = 1000;
            m_timer.Enabled = true;
            m_timer.Elapsed += new ElapsedEventHandler(this.TimeElapsed);

            m_packetLocker = new object();
            m_model = _model;
        }

        public Database.Models.Clients.AuthClientModel m_model { get; set; }
        Timer m_timer { get; set; }
        bool  isLogged = false;
        object m_packetLocker { get; set; }

        public void Start()
        {
            this.ConnectTo(m_model.m_ip, m_model.m_port);
        }

        public void Send(string _message, bool _force = false)
        {
            if (isLogged == false && _force == false)
                return;

            Utilities.Loggers.m_infosLogger.Write(string.Format("Sent to {0} : {1}", myIp(), _message));
            this.SendDatas(_message);
        }

        void TimeElapsed(object sender, EventArgs e)
        {
            if (this.isConnected == false)
                Start();
            else
                m_timer.Stop();
        }

        void FailedToConnect(Exception _exception)
        {
            Utilities.Loggers.m_errorsLogger.Write(string.Format("Cannot connect to @AuthServer@ because {0}", _exception.ToString()));
        }

        void DatasArrival(string _datas)
        {
            lock (m_packetLocker)
                ParsePacket(_datas);
        }

        void Disconnected()
        {
            Utilities.Loggers.m_statusLogger.Write("Connection with the @AuthServer@ closed !");
            m_timer.Start();
        }

        void ParsePacket(string _datas)
        {
            var infos = _datas.Split('|');

            try
            {
                switch (infos[0])
                {
                    case "ANTS":

                        AuthenticationsKeys.m_keys.Add(new AuthenticationsKeys(_datas));
                        break;

                    case "HCS":

                        Send(string.Format("SAI|{0}|{1}|{2}", Utilities.Config.m_config.GetIntElement("ServerId"),
                            Utilities.Config.m_config.GetStringElement("ServerIp"),
                            Utilities.Config.m_config.GetIntElement("ServerPort")), true);
                        break;

                    case "HCSS":

                        isLogged = true;
                        Utilities.Loggers.m_infosLogger.Write("Connected with the @AuthenticationServer@ !");

                        if (ServersHandler.m_realmServer.m_pseudoclients.Count > 0)
                        {
                            Send(string.Format("SNLC|{0}", string.Join("|", ServersHandler.m_realmServer.m_pseudoclients)));
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Utilities.Loggers.m_errorsLogger.Write(string.Format("Cannot parse @AuthServer's packet@ ({0}) because : {1}", _datas, e.ToString()));
            }
        }
    }
}
