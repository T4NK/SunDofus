using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Network
{
    class SelectorKeys
    {
        public static List<SelectorKeys> m_Keys = new List<SelectorKeys>();

        public string m_Key;
        public Client.RealmInfos m_Infos;

        public SelectorKeys(string Packet)
        {
            m_Key = "";
            m_Infos = new Client.RealmInfos();

            string[] Data = Packet.Split('|');
            m_Key = Data[1];
            m_Infos.Id = int.Parse(Data[2]);
            m_Infos.Pseudo = Data[3];
            m_Infos.Question = Data[4];
            m_Infos.Answer = Data[5];
            m_Infos.Level = int.Parse(Data[6]);
            m_Infos.Characters = Data[7];

            if (Config.ConfigurationManager.Subscription == true)
                m_Infos.Subscription = long.Parse(Data[8].Substring(0, Data[8].Length - 3));
            else
                m_Infos.Subscription = (60 * 60 * 24 * 365);

            m_Infos.Gifts = Data[9];
        }
    }
}
