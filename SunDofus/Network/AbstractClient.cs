using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace SunDofus
{
    public class AbstractClient
    {
        public SilverSocket mySocket;

        public delegate void OnClosedEvent();
        public OnClosedEvent RaiseClosedEvent;

        public delegate void DataArrivalEvent(string M);
        public DataArrivalEvent RaiseDataArrivalEvent;

        public AbstractClient(SilverSocket Socket)
        {
            mySocket = Socket;
            mySocket.OnSocketClosedEvent += new SilverEvents.SocketClosed(this.isDisconnected);
            mySocket.OnDataArrivalEvent += new SilverEvents.DataArrival(this.DataArrival);
        }

        void DataArrival(byte[] data)
        {
            string NotParsed = Encoding.ASCII.GetString(data);
            foreach (string Packet in NotParsed.Replace("\x0a", "").Split('\x00'))
            {
                if (Packet == "") continue;
                RaiseDataArrivalEvent(Packet);
            }
        }

        void isDisconnected()
        {
            RaiseClosedEvent();
        }

        public void Disconnect()
        {
            mySocket.CloseSocket();
        }

        public string myIp()
        {
            return mySocket.IP;
        }

        public void Send(string Message)
        {
            try
            {
                byte[] P = Encoding.ASCII.GetBytes(string.Format("{0}\x00", Message));
                mySocket.Send(P);
            }
            catch { }
        }
    }
}
