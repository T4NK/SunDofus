using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace selector.Client
{
    class AbstractClient
    {
        public SilverSocket m_Socket;

        public delegate void OnClosedEvent();
        public OnClosedEvent RaiseClosedEvent;

        public delegate void DataArrivalEvent(string M);
        public DataArrivalEvent RaiseDataArrivalEvent;

        public AbstractClient(SilverSocket Socket)
        {
            m_Socket = Socket;
            m_Socket.OnSocketClosedEvent += new SilverEvents.SocketClosed(this.IsDisconnected);
            m_Socket.OnDataArrivalEvent += new SilverEvents.DataArrival(this.DataArrival);
        }

        public void DataArrival(byte[] data)
        {
            string NotParsed = Encoding.ASCII.GetString(data);
            foreach (string Packet in NotParsed.Replace("\x0a", "").Split('\x00'))
            {
                if (Packet == "") continue;
                Utils.Logger.Packets("[Received]! " + Packet);
                RaiseDataArrivalEvent(Packet);
            }
        }

        public void IsDisconnected()
        {
            RaiseClosedEvent();
        }

        public void Send(string Message)
        {
            Utils.Logger.Packets("[Sended]! " + Message);
            byte[] P = Encoding.ASCII.GetBytes(Message + "\x00");
            m_Socket.Send(P);
        }
    }
}
