using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace SunDofus.Network
{
    public class AbstractServer
    {
        SilverServer m_server { get; set; }
        string m_remote { get; set; }

        protected delegate void AcceptSocketHandler(SilverSocket _socket);
        protected AcceptSocketHandler SocketClientAccepted;

        void OnSocketClientAccepted(SilverSocket _socket)
        {
            var evnt = SocketClientAccepted;
            if (evnt != null)
                evnt(_socket);
        }

        protected delegate void ListeningServerHandler(string _remote);
        protected ListeningServerHandler ListeningServer;

        void OnListeningServer(string _remote)
        {
            var evnt = ListeningServer;
            if (evnt != null)
                evnt(_remote);
        }

        protected delegate void ListeningServerFailedHandler(Exception e);
        protected ListeningServerFailedHandler ListeningServerFailed;

        void OnListeningServerFailed(Exception _exception)
        {
            var evnt = ListeningServerFailed;
            if (evnt != null)
                evnt(_exception);
        }

        public AbstractServer(string ip, int port)
        {
            m_remote = string.Format("{0}:{1}", ip, port);

            m_server = new SilverServer(ip, port);
            m_server.OnAcceptSocketEvent += new SilverEvents.AcceptSocket(this.AcceptSocket);
            m_server.OnListeningEvent += new SilverEvents.Listening(this.OnListen);
            m_server.OnListeningFailedEvent += new SilverEvents.ListeningFailed(this.OnListenFailed);
        }

        public void Start()
        {
            m_server.WaitConnection();
        }

        void AcceptSocket(SilverSocket _socket)
        {
            OnSocketClientAccepted(_socket);
        }

        void OnListen()
        {
            OnListeningServer(m_remote);
        }

        void OnListenFailed(Exception exception)
        {
            OnListeningServerFailed(exception);
        }
    }
}
