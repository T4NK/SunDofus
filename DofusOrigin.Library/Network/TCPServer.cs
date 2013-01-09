using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace DofusOrigin.Network
{
    public class TCPServer
    {
        private SilverServer m_server { get; set; }
        private string m_remote { get; set; }

        protected delegate void AcceptSocketHandler(SilverSocket _socket);
        protected AcceptSocketHandler SocketClientAccepted;

        private void OnSocketClientAccepted(SilverSocket _socket)
        {
            var evnt = SocketClientAccepted;
            if (evnt != null)
                evnt(_socket);
        }

        protected delegate void ListeningServerHandler(string _remote);
        protected ListeningServerHandler ListeningServer;

        private void OnListeningServer(string _remote)
        {
            var evnt = ListeningServer;
            if (evnt != null)
                evnt(_remote);
        }

        protected delegate void ListeningServerFailedHandler(Exception e);
        protected ListeningServerFailedHandler ListeningServerFailed;

        private void OnListeningServerFailed(Exception _exception)
        {
            var evnt = ListeningServerFailed;
            if (evnt != null)
                evnt(_exception);
        }

        public TCPServer(string ip, int port)
        {
            m_remote = string.Format("{0}:{1}", ip, port);

            m_server = new SilverServer(ip, port);
            {
                m_server.OnAcceptSocketEvent += new SilverEvents.AcceptSocket(this.AcceptSocket);
                m_server.OnListeningEvent += new SilverEvents.Listening(this.OnListen);
                m_server.OnListeningFailedEvent += new SilverEvents.ListeningFailed(this.OnListenFailed);
            }
        }

        public void Start()
        {
            m_server.WaitConnection();
        }

        private void AcceptSocket(SilverSocket _socket)
        {
            OnSocketClientAccepted(_socket);
        }

        private void OnListen()
        {
            OnListeningServer(m_remote);
        }

        private void OnListenFailed(Exception exception)
        {
            OnListeningServerFailed(exception);
        }
    }
}
