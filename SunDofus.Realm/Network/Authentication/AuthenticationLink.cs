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
        SilverSocket m_Socket;
        Timer m_Timer;
        bool  isConnected = false, isLogged = false;
        object PacketLocker;

        public AuthenticationLink()
        {
            m_Socket = new SilverSocket();
            m_Socket.OnConnected += new SilverEvents.Connected(this.Connected);
            m_Socket.OnFailedToConnect += new SilverEvents.FailedToConnect(this.FailedToConnect);
            m_Socket.OnDataArrivalEvent += new SilverEvents.DataArrival(this.DataArrival);
            m_Socket.OnSocketClosedEvent += new SilverEvents.SocketClosed(this.Disconnected);

            m_Timer = new Timer();
            m_Timer.Interval = 1000;
            m_Timer.Enabled = true;
            m_Timer.Elapsed += new ElapsedEventHandler(this.ElapsedVoid);

            PacketLocker = new object();
        }

        public void Start()
        {
            m_Socket.ConnectTo(Utilities.Config.myConfig.GetStringElement("AuthIp"), Utilities.Config.myConfig.GetIntElement("AuthPort"));
        }

        public void Send(string Message, bool Force = false)
        {
            if (isLogged == false && Force == false) return;
            try
            {
                byte[] P = Encoding.ASCII.GetBytes(Message + "\x00");
                m_Socket.Send(P);
            }
            catch { }
        }

        void ElapsedVoid(object sender, EventArgs e)
        {
            if (isConnected == false)
                Start();
            else
                m_Timer.Stop();
        }

        void FailedToConnect(Exception e)
        {
            Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot connect to AuthServer because {0}", e.ToString()));
        }

        void Connected()
        {
            isConnected = true;
            Utilities.Loggers.StatusLogger.Write(string.Format("Connected with the AuthServer <{0}:{1}> !",
                Utilities.Config.myConfig.GetStringElement("AuthIp"), Utilities.Config.myConfig.GetIntElement("AuthPort")));
        }

        void DataArrival(byte[] data)
        {
            string NotParsed = Encoding.ASCII.GetString(data);
            foreach (string Packet in NotParsed.Replace("\x0a", "").Split('\x00'))
            {
                if (Packet == "") continue;

                lock(PacketLocker)
                    ParsePacket(Packet);
            }
        }

        void Disconnected()
        {
            m_Timer.Start();
            Utilities.Loggers.InfosLogger.Write("Connection with the selector closed !");
        }

        void ParsePacket(string Data)
        {
            string[] Infos = Data.Split('|');

            try
            {
                switch (Infos[0])
                {
                    case "ANTS":

                        AuthenticationKeys.m_Keys.Add(new AuthenticationKeys(Data));
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
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot parse AuthServer's packet ({0}) because : {1}", Data, e.ToString()));
            }
        }
    }
}
