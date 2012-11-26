using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using System.Timers;

namespace realm.Network.Authentication
{
    class AuthenticationLink : SunDofus.Network.TCPClient
    {
        Timer m_timer;
        bool  isLogged = false;
        object m_packetLocker;

        public AuthenticationLink() : base (new SilverSocket())
        {
            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);
            this.ReceivedDatas += new ReceiveDatasHandler(this.DatasArrival);
            this.ConnectFailed += new ConnectFailedHandler(this.FailedToConnect);

            m_timer = new Timer();
            m_timer.Interval = 1000;
            m_timer.Enabled = true;
            m_timer.Elapsed += new ElapsedEventHandler(this.TimeElapsed);

            m_packetLocker = new object();
        }

        public void Start()
        {
            this.ConnectTo(Utilities.Config.m_config.GetStringElement("AuthIp"), Utilities.Config.m_config.GetIntElement("AuthPort"));
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
                    case "ANAA":

                        if (!ServersHandler.adminAccount.ContainsKey(infos[1]))
                            ServersHandler.adminAccount.Add(infos[1], infos[2]);
                        else
                            ServersHandler.adminAccount[infos[1]] = infos[2];
                        break;

                    case "ANTS":

                        AuthenticationKeys.m_keys.Add(new AuthenticationKeys(_datas));
                        break;

                    case "HCS":

                        Send(string.Format("SAI|{0}|{1}|{2}", Utilities.Config.m_config.GetIntElement("ServerId"),
                            Utilities.Config.m_config.GetStringElement("ServerIp"),
                            Utilities.Config.m_config.GetIntElement("ServerPort")), true);
                        break;

                    case "HCSS":

                        isLogged = true;
                        Utilities.Loggers.m_infosLogger.Write("Connected with the @AuthenticationServer@ !");
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
