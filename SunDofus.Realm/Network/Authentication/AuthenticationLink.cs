using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using System.Timers;

namespace realm.Network.Authentication
{
    class AuthenticationLink
    {
        SilverSocket mySocket;
        Timer myTimer;
        bool  isConnected = false, isLogged = false;
        object myPacketLocker;

        public AuthenticationLink()
        {
            mySocket = new SilverSocket();
            mySocket.OnConnected += new SilverEvents.Connected(this.Connected);
            mySocket.OnFailedToConnect += new SilverEvents.FailedToConnect(this.FailedToConnect);
            mySocket.OnDataArrivalEvent += new SilverEvents.DataArrival(this.DataArrival);
            mySocket.OnSocketClosedEvent += new SilverEvents.SocketClosed(this.Disconnected);

            myTimer = new Timer();
            myTimer.Interval = 1000;
            myTimer.Enabled = true;
            myTimer.Elapsed += new ElapsedEventHandler(this.ElapsedVoid);

            myPacketLocker = new object();
        }

        public void Start()
        {
            mySocket.ConnectTo(Utilities.Config.myConfig.GetStringElement("AuthIp"), Utilities.Config.myConfig.GetIntElement("AuthPort"));
        }

        public void Send(string Message, bool Force = false)
        {
            if (isLogged == false && Force == false) return;

            try
            {
                byte[] P = Encoding.ASCII.GetBytes(string.Format("{0}\x00", Message));
                mySocket.Send(P);
            }
            catch { }
        }

        void ElapsedVoid(object sender, EventArgs e)
        {
            if (isConnected == false)
                Start();
            else
                myTimer.Stop();
        }

        void FailedToConnect(Exception e)
        {
            Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot connect to @AuthServer@ because {0}", e.ToString()));
        }

        void Connected()
        {
            isConnected = true;

            Utilities.Loggers.StatusLogger.Write(string.Format("Connected with the @AuthServer@ <{0}:{1}> !",
                Utilities.Config.myConfig.GetStringElement("AuthIp"), Utilities.Config.myConfig.GetIntElement("AuthPort")));
        }

        void DataArrival(byte[] data)
        {
            var NotParsed = Encoding.ASCII.GetString(data);
            foreach (var Packet in NotParsed.Replace("\x0a", "").Split('\x00'))
            {
                if (Packet == "") continue;

                lock(myPacketLocker)
                    ParsePacket(Packet);
            }
        }

        void Disconnected()
        {
            isConnected = false;
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
