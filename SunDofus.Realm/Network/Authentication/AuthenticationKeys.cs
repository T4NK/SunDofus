using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Network.Authentication
{
    class AuthenticationKeys
    {
        public static List<AuthenticationKeys> m_Keys = new List<AuthenticationKeys>();

        public string m_Key;
        public Database.Models.Clients.AccountModel m_Infos;

        public AuthenticationKeys(string Packet)
        {
            m_Key = "";
            m_Infos = new Database.Models.Clients.AccountModel();

            string[] Data = Packet.Split('|');
            m_Key = Data[1];
            m_Infos.Id = int.Parse(Data[2]);
            m_Infos.Pseudo = Data[3];
            m_Infos.Question = Data[4];
            m_Infos.Answer = Data[5];
            m_Infos.Level = int.Parse(Data[6]);
            m_Infos.Characters = Data[7];
            m_Infos.Subscription = long.Parse(Data[8]);

            m_Infos.Gifts = Data[9];
        }
    }
}
