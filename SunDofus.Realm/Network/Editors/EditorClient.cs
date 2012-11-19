using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace realm.Network.Editors
{
    class EditorClient : SunDofus.Network.AbstractClient
    {
        object myPacketLocker;
        bool isLogged = false;

        public EditorClient(SilverSocket socket)
            : base(socket)
        { 
            myPacketLocker = new object();

            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);
            this.ReceivedDatas += new ReceiveDatasHandler(this.ReceivedPackets);

            Send("HCE");
        }

        public void Send(string Message)
        {
            this.SendDatas(Message);
            Utilities.Loggers.InfosLogger.Write(string.Format("Sent to @<{0}>@ : {1}", myIp(), Message));
        }

        void ReceivedPackets(string Data)
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("Receive datas from @<{0}>@ : {1}", myIp(), Data));

            lock (myPacketLocker)
            {
                var datas = Data.Split('|');

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
            Utilities.Loggers.InfosLogger.Write(string.Format("New closed editor @<{0}>@ connection !", myIp()));
            Network.ServersHandler.myEditorServer.myClients.Remove(this);
        }

        void ParseAuth(string[] datas)
        {
            if (ServersHandler.adminAccount.ContainsKey(datas[1]))
            {
                if (ServersHandler.adminAccount[datas[1]] == datas[2])
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
