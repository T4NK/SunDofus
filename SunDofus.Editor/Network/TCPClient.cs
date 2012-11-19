using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace SunDofus.Editor.Network
{
    class TCPClient : SunDofus.Network.AbstractClient
    {
        bool isLogged = false;

        public TCPClient()
            : base(new SilverSocket())
        {
            this.ReceivedDatas += new ReceiveDatasHandler(this.DataArrival);
            this.ConnectFailed += new ConnectFailedHandler(this.FailedToConnect);
            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);
        }

        public void Send(string Message, bool Force = false)
        {
            if (isLogged == false && Force == false)
                return;

            SendDatas(Message);
        }

        void FailedToConnect(Exception e)
        {
            Program.Message(string.Format("Impossible de se connecter au serveur ! ({0}) !", e.ToString()));
        }

        void Disconnected()
        {
            Program.Message("Déconnecté du serveur de jeu ! Mauvais identifiants ?");
        }

        void DataArrival(string Message)
        {
            var datas = Message.Split('|');

            switch(datas[0])
            {
                case "HCE":

                    Send(string.Format("HCE|{0}|{1}", Class.Infos.Username, Class.Infos.Password), true);
                    break;

                case "SCE":

                    isLogged = true;
                    break;
            }
        }

    }
}
