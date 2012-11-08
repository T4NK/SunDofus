using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace SunDofus
{
    public class AbstractClient
    {
        SilverSocket mySocket;
        public bool isConnected = false;

        public delegate void OnClosedEvent();
        public OnClosedEvent RaiseClosedEvent;

        public delegate void DataArrivalEvent(string M);
        public DataArrivalEvent RaiseDataArrivalEvent;

        public delegate void FailedConnectEvent(Exception e);
        public FailedConnectEvent RaiseFailedConnectEvent;

        public AbstractClient(SilverSocket Socket)
        {
            mySocket = Socket;
            mySocket.OnConnected += new SilverEvents.Connected(this.Connected);
            mySocket.OnSocketClosedEvent += new SilverEvents.SocketClosed(this.isDisconnected);
            mySocket.OnDataArrivalEvent += new SilverEvents.DataArrival(this.DataArrival);
            mySocket.OnFailedToConnect += new SilverEvents.FailedToConnect(this.FailedToConnect);
        }

        public void ConnectTo(string Ip, int Port)
        {
            mySocket.ConnectTo(Ip, Port);
        }

        public string myIp()
        {
            return mySocket.IP;
        }

        protected void meSend(string Message)
        {
            try
            {
                byte[] P = Encoding.ASCII.GetBytes(string.Format("{0}\x00", Message));
                mySocket.Send(P);
            }
            catch { }
        }

        void Connected()
        {
            isConnected = true;
        }

        void FailedToConnect(Exception e)
        {
            RaiseFailedConnectEvent(e);
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
            isConnected = false;
            RaiseClosedEvent();
        }

        public void Disconnect()
        {
            mySocket.CloseSocket();
        }
    }
}
