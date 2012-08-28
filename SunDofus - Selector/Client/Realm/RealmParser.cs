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
            try
            {
                string[] Packet = Data.Split('|');

                switch (Packet[0])
                {
                    case "Auth":
                        Authentification(int.Parse(Packet[1]), Packet[2], int.Parse(Packet[3]));
                        break;

                    case "DC":
                        Client.m_Server.Clients.Remove(Packet[1]);
                        break;

                    case "DG":
                        RealmSqlAction.DeleteGift(int.Parse(Packet[1]), int.Parse(Packet[2]));
                        break;

                    case "NC":
                        Client.m_Server.Clients.Add(Packet[1]);
                        break;

                    case "NCHAR":
                        RealmSqlAction.UpdateCharacters(int.Parse(Packet[1]), Packet[2], Client.m_Server.ID);
                        break;

                    case "StartM":
                        Client.m_Server.Connected = 2;
                        Program.m_Auth.RefreshAllHosts();
                        break;

                    case "StopM":
                        Client.m_Server.Connected = 1;
                        Program.m_Auth.RefreshAllHosts();
                        break;
                }
            }
            catch (Exception e)
            {
                SunDofus.Logger.Error(e);
            }
        }

        public void Authentification(int ServerId, string ServerIp, int ServerPort)
        {
            if (Database.ServersManager.myServers.Any(x => x.ID == ServerId && x.Ip == ServerIp && x.Port == ServerPort && x.Connected == 0))
            {
                Database.Data.Server m_Server = Database.ServersManager.myServers.First(x => x.ID == ServerId && x.Ip == ServerIp && x.Port == ServerPort && x.Connected == 0);

                Client.m_Server = m_Server;
                Client.Send("Connected!");
                SunDofus.Logger.Status("Server '" + ServerId + "' authentified !");
                Client.ChangeState(RealmClient.State.Connected);
                return;
            }
            else
                Client.Send("FailedToConnect!");
        }
    }
}
