using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace selector.Client
{
    class RealmParser
    {
        public RealmClient Client;

        public RealmParser(RealmClient m_C)
        {
            Client = m_C;
        }

        public void Parse(string Data)
        {
            string[] Packet = Data.Split('|');

            switch (Packet[0])
            {
                case "Auth":
                    Authentification(int.Parse(Packet[1]), Packet[2], int.Parse(Packet[3]));
                    break;
            }
        }

        public void Authentification(int ServerId, string ServerIp, int ServerPort)
        {
            foreach(Database.Data.Server m_Server in Database.Data.Server.ListOfServers)
            {
                if (m_Server.ID == ServerId && m_Server.Ip == ServerIp && m_Server.Port == ServerPort)
                {
                    Client.m_Server = m_Server;
                    Client.Send("Connected!");
                    Utils.Logger.Status("Server '" + ServerId + "' authentified !");
                    Client.ChangeState(RealmClient.State.Connected);
                    return;
                }
            }
            Client.Send("FailedToConnect!");
        }
    }
}
