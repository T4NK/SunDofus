using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace selector.Network
{
    class AbstractServer
    {
        SilverServer m_Server;
        int m_Port;

        public delegate void AcceptEvent(SilverSocket Socket);
        public AcceptEvent RaiseAcceptEvent;

        public delegate void OnListenEvent(int Port);
        public OnListenEvent RaiseListenEvent;

        public delegate void OnListenFailedEvent(Exception e);
        public OnListenFailedEvent RaiseListenFailedEvent;

        public AbstractServer(string ip, int port)
        {
            m_Port = port;

            m_Server = new SilverServer(ip, port);
            m_Server.OnAcceptSocketEvent += new SilverEvents.AcceptSocket(this.AcceptSocket);
            m_Server.OnListeningEvent += new SilverEvents.Listening(this.OnListen);
            m_Server.OnListeningFailedEvent += new SilverEvents.ListeningFailed(this.OnListenFailed);
        }

        public void Start()
        {
            m_Server.WaitConnection();
        }

        public void AcceptSocket(SilverSocket m_Socket)
        {
            RaiseAcceptEvent(m_Socket);
        }

        public void OnListen()
        {
            RaiseListenEvent(m_Port);
        }

        public void OnListenFailed(Exception e)
        {
            RaiseListenFailedEvent(e);
        }

        
    }
}
