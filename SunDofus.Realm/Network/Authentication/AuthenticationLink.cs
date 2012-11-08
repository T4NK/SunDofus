using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using System.Timers;

namespace realm.Network.Authentication
{
    class AuthenticationLink : SunDofus.AbstractClient
    {
        Timer myTimer;
        bool  isLogged = false;
        object myPacketLocker;

        public AuthenticationLink() : base (new SilverSocket())
        {
            this.RaiseClosedEvent += new OnClosedEvent(this.Disconnected);
            this.RaiseDataArrivalEvent += new DataArrivalEvent(this.DataArrival);
            this.RaiseFailedConnectEvent += new FailedConnectEvent(this.FailedToConnect);

            myTimer = new Timer();
            myTimer.Interval = 1000;
            myTimer.Enabled = true;
            myTimer.Elapsed += new ElapsedEventHandler(this.ElapsedVoid);

            myPacketLocker = new object();
        }

        public void Start()
        {
            this.ConnectTo(Utilities.Config.myConfig.GetStringElement("AuthIp"), Utilities.Config.myConfig.GetIntElement("AuthPort"));
        }

        public void Send(string Message, bool Force = false)
        {
            if (isLogged == false && Force == false)
                return;

            Utilities.Loggers.InfosLogger.Write(string.Format("Sent to {0} : {1}", myIp(), Message));
            this.meSend(Message);
        }

        void ElapsedVoid(object sender, EventArgs e)
        {
            if (this.isConnected == false)
                Start();
            else
                myTimer.Stop();
        }

        void FailedToConnect(Exception e)
        {
            Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot connect to @AuthServer@ because {0}", e.ToString()));
        }

        void DataArrival(string datas)
        {
            lock (myPacketLocker)
                ParsePacket(datas);
        }

        void Disconnected()
        {
            Utilities.Loggers.StatusLogger.Write("Connection with the @AuthServer@ closed !");
            myTimer.Start();
        }

        void ParsePacket(string Data)
        {
            string[] Infos = Data.Split('|');

            try
            {
                switch (Infos[0])
                {
                    case "ANAA":

                        if (!ServersHandler.adminAccount.ContainsKey(Infos[1]))
                            ServersHandler.adminAccount.Add(Infos[1], Infos[2]);
                        else
                            ServersHandler.adminAccount[Infos[1]] = Infos[2];
                        break;

                    case "ANTS":

                        AuthenticationKeys.myKeys.Add(new AuthenticationKeys(Data));
                        break;

                    case "HCS":

                        Send(string.Format("SAI|{0}|{1}|{2}", Utilities.Config.myConfig.GetIntElement("ServerId"),
                            Utilities.Config.myConfig.GetStringElement("ServerIp"),
                            Utilities.Config.myConfig.GetIntElement("ServerPort")), true);
                        break;

                    case "HCSS":

                        isLogged = true;
                        Utilities.Loggers.InfosLogger.Write("Connected with the @AuthenticationServer@ !");
                        break;
                }
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot parse @AuthServer's packet@ ({0}) because : {1}", Data, e.ToString()));
            }
        }
    }
}
