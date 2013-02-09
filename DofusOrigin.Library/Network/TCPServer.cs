using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace DofusOrigin.Network
{
    public class TCPServer
    {
        private SilverServer _server;
        private string _remote;

        protected delegate void AcceptSocketHandler(SilverSocket socket);
        protected AcceptSocketHandler SocketClientAccepted;

        private void OnSocketClientAccepted(SilverSocket socket)
        {
            var evnt = SocketClientAccepted;
            if (evnt != null)
                evnt(socket);
        }

        protected delegate void ListeningServerHandler(string remote);
        protected ListeningServerHandler ListeningServer;

        private void OnListeningServer(string remote)
        {
            var evnt = ListeningServer;
            if (evnt != null)
                evnt(remote);
        }

        protected delegate void ListeningServerFailedHandler(Exception e);
        protected ListeningServerFailedHandler ListeningServerFailed;

        private void OnListeningServerFailed(Exception exception)
        {
            var evnt = ListeningServerFailed;
            if (evnt != null)
                evnt(exception);
        }

        public TCPServer(string ip, int port)
        {
            _remote = string.Format("{0}:{1}", ip, port);

            _server = new SilverServer(ip, port);
            {
                _server.OnAcceptSocketEvent += new SilverEvents.AcceptSocket(this.AcceptSocket);
                _server.OnListeningEvent += new SilverEvents.Listening(this.OnListen);
                _server.OnListeningFailedEvent += new SilverEvents.ListeningFailed(this.OnListenFailed);
            }
        }

        public void Start()
        {
            _server.WaitConnection();
        }

        private void AcceptSocket(SilverSocket socket)
        {
            OnSocketClientAccepted(socket);
        }

        private void OnListen()
        {
            OnListeningServer(_remote);
        }

        private void OnListenFailed(Exception exception)
        {
            OnListeningServerFailed(exception);
        }
    }
}
