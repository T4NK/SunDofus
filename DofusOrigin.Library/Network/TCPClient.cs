using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace DofusOrigin.Network
{
    public class TCPClient
    {
        private SilverSocket _socket;
        public bool isConnected = false;

        protected delegate void DisconnectedSocketHandler();
        protected DisconnectedSocketHandler DisconnectedSocket;

        private void OnDisconnectedSocket()
        {
            var evnt = DisconnectedSocket;
            if (evnt != null)
                evnt();
        }

        protected delegate void ReceiveDatasHandler(string _message);
        protected ReceiveDatasHandler ReceivedDatas;

        private void OnReceivedDatas(string _message)
        {
            var evnt = ReceivedDatas;
            if (evnt != null)
                evnt(_message);
        }

        protected delegate void ConnectFailedHandler(Exception _exception);
        protected ConnectFailedHandler ConnectFailed;

        private void OnConnectFailed(Exception _exception)
        {
            var evnt = ConnectFailed;
            if (evnt != null)
                evnt(_exception);
        }

        public TCPClient(SilverSocket socket)
        {
            _socket = socket;

            _socket.OnConnected += new SilverEvents.Connected(this.Connected);
            _socket.OnSocketClosedEvent += new SilverEvents.SocketClosed(this.Disconnected);
            _socket.OnDataArrivalEvent += new SilverEvents.DataArrival(this.DatasArrival);
            _socket.OnFailedToConnect += new SilverEvents.FailedToConnect(this.FailedToConnect);
        }

        public void ConnectTo(string ip, int port)
        {
            _socket.ConnectTo(ip, port);
        }

        public string myIp()
        {
            return _socket.IP;
        }

        public void Disconnect()
        {
            _socket.CloseSocket();
        }

        protected void SendDatas(string message)
        {
            try
            {
                var P = Encoding.UTF8.GetBytes(string.Format("{0}\x00", message));
                _socket.Send(P);
            }
            catch { }
        }

        #region toEvent

        private void DatasArrival(byte[] datas)
        {
            var notParsed = Encoding.UTF8.GetString(datas);

            foreach (var Packet in notParsed.Replace("\x0a", "").Split('\x00'))
            {
                if (Packet == "")
                    continue;

                OnReceivedDatas(Packet);
            }
        }

        private void Connected()
        {
            isConnected = true;
        }

        private void FailedToConnect(Exception e)
        {
            OnConnectFailed(e);
        }

        private void Disconnected()
        {
            isConnected = false;

            OnDisconnectedSocket();
        }

        #endregion
    }
}
