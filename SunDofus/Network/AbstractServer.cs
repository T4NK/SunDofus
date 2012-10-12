using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace SunDofus
{
    public class AbstractServer
    {
        SilverServer m_Server;
        string myRemote;

        public delegate void AcceptEvent(SilverSocket Socket);
        public AcceptEvent RaiseAcceptEvent = null;

        public delegate void OnListenEvent(string Remote);
        public OnListenEvent RaiseListenEvent = null;

        public delegate void OnListenFailedEvent(Exception e);
        public OnListenFailedEvent RaiseListenFailedEvent = null;

        public AbstractServer(string ip, int port)
        {
            myRemote = string.Format("{0}:{1}", ip, port);

            m_Server = new SilverServer(ip, port);
            m_Server.OnAcceptSocketEvent += new SilverEvents.AcceptSocket(this.AcceptSocket);
            m_Server.OnListeningEvent += new SilverEvents.Listening(this.OnListen);
            m_Server.OnListeningFailedEvent += new SilverEvents.ListeningFailed(this.OnListenFailed);
        }

        public void Start()
        {
            m_Server.WaitConnection();
        }

        void AcceptSocket(SilverSocket m_Socket)
        {
            RaiseAcceptEvent(m_Socket);
        }

        void OnListen()
        {
            RaiseListenEvent(myRemote);
        }

        void OnListenFailed(Exception e)
        {
            RaiseListenFailedEvent(e);
        }
    }
}
