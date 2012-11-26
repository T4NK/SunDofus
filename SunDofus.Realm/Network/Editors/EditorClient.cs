using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace realm.Network.Editors
{
    class EditorClient : SunDofus.Network.TCPClient
    {
        object m_packetLocker;
        bool isLogged = false;

        public EditorClient(SilverSocket _socket)
            : base(_socket)
        { 
            m_packetLocker = new object();

            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);
            this.ReceivedDatas += new ReceiveDatasHandler(this.ReceivedPackets);

            Send("HCE");
        }

        public void Send(string _message)
        {
            this.SendDatas(_message);
            Utilities.Loggers.m_infosLogger.Write(string.Format("Sent to @<{0}>@ : {1}", myIp(), _message));
        }

        void ReceivedPackets(string _datas)
        {
            Utilities.Loggers.m_infosLogger.Write(string.Format("Receive datas from @<{0}>@ : {1}", myIp(), _datas));

            lock (m_packetLocker)
            {
                var datas = _datas.Split('|');

                switch (datas[0])
                {
                    case "HCE":

                        ParseAuth(datas);
                        break;

                    case "ANM":

                        if(isLogged == true)
                            Database.Cache.MapsCache.ReloadMaps();
                        break;
                }
            }
        }

        void Disconnected()
        {
            Utilities.Loggers.m_infosLogger.Write(string.Format("New closed editor @<{0}>@ connection !", myIp()));
            Network.ServersHandler.m_editorServer.m_clients.Remove(this);
        }

        void ParseAuth(string[] _datas)
        {
            if (ServersHandler.adminAccount.ContainsKey(_datas[1]))
            {
                if (ServersHandler.adminAccount[_datas[1]] == _datas[2])
                {
                    Send("SCE");
                    isLogged = true;
                }
                else
                    Disconnect();
            }
            else
                Disconnect();
        }
    }
}
