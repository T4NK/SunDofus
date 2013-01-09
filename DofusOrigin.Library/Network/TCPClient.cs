﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace DofusOrigin.Network
{
    public class TCPClient
    {
        private SilverSocket m_socket { get; set; }
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

        public TCPClient(SilverSocket _socket)
        {
            m_socket = _socket;

            m_socket.OnConnected += new SilverEvents.Connected(this.Connected);
            m_socket.OnSocketClosedEvent += new SilverEvents.SocketClosed(this.Disconnected);
            m_socket.OnDataArrivalEvent += new SilverEvents.DataArrival(this.DatasArrival);
            m_socket.OnFailedToConnect += new SilverEvents.FailedToConnect(this.FailedToConnect);
        }

        public void ConnectTo(string _ip, int _port)
        {
            m_socket.ConnectTo(_ip, _port);
        }

        public string myIp()
        {
            return m_socket.IP;
        }

        public void Disconnect()
        {
            m_socket.CloseSocket();
        }

        protected void SendDatas(string _message)
        {
            try
            {
                var P = Encoding.ASCII.GetBytes(string.Format("{0}\x00", _message));
                m_socket.Send(P);
            }
            catch { }
        }

        #region toEvent

        private void DatasArrival(byte[] _datas)
        {
            var notParsed = Encoding.ASCII.GetString(_datas);

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