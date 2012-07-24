using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using System.Timers;

namespace realm.Network
{
    class SelectorLink
    {
        SilverSocket m_Socket;
        Timer m_Timer;
        bool m_RunningTimer = false;
        SelectorParser m_Parser;

        public SelectorLink()
        {
            m_Socket = new SilverSocket();

            m_Socket.OnConnected += new SilverEvents.Connected(this.Connected);
            m_Socket.OnFailedToConnect += new SilverEvents.FailedToConnect(this.FailedToConnect);
            m_Socket.OnDataArrivalEvent += new SilverEvents.DataArrival(this.DataArrival);
            m_Socket.OnSocketClosedEvent += new SilverEvents.SocketClosed(this.Disconnected);

            m_Timer = new Timer();
            m_Timer.Interval = 10000;
            m_Timer.Enabled = true;

            m_Timer.Elapsed += new ElapsedEventHandler(this.ElapsedVoid);

            m_Parser = new SelectorParser(this);
        }

        public void Start()
        {
            m_Socket.ConnectTo(Config.ConfigurationManager.GetString("Selector_Ip"), Config.ConfigurationManager.GetInt("Selector_Port"));
        }

        public void ElapsedVoid(object sender, EventArgs e)
        {
            Start();
        }

        public void FailedToConnect(Exception e)
        {
            if (m_RunningTimer == false)
            {
                m_Timer.Start();
                m_RunningTimer = true;
            }
        }

        public void Connected()
        {
            m_Timer.Stop();
            m_RunningTimer = false;
            Utils.Logger.Status("Connected with the selector <" + Config.ConfigurationManager.GetString("Selector_Ip") + "," + Config.ConfigurationManager.GetInt("Selector_Port") + "> !");

            Send("Auth|" + Program.m_ServerID + "|" + Config.ConfigurationManager.GetString("Server_Ip") + "|" + Config.ConfigurationManager.GetString("Server_Port"));
        }

        public void DataArrival(byte[] data)
        {
            string NotParsed = Encoding.ASCII.GetString(data);
            foreach (string Packet in NotParsed.Replace("\x0a", "").Split('\x00'))
            {
                if (Packet == "") continue;
                Utils.Logger.Packets("[Received]! " + Packet);
                m_Parser.Parse(Packet);
            }
        }

        public void Disconnected()
        {
            m_Timer.Start();
            m_RunningTimer = true;
            Utils.Logger.Infos("Connection with the selector closed !");
        }

        public void Send(string Message)
        {
            Utils.Logger.Packets("[Sended]! " + Message);
            byte[] P = Encoding.ASCII.GetBytes(Message + "\x00");
            m_Socket.Send(P);
        }
    }
}
