using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace selector.Client
{
    class SelectorParser
    {
        public SelectorClient Client;

        public SelectorParser(SelectorClient m_C)
        {
            Client = m_C;
        }

        public void Parse(string Data)
        {
            switch (Client.m_State)
            {
                case SelectorClient.State.Version:
                    Version(Data);
                    break;

                case SelectorClient.State.Account:
                    Account(Data);
                    break;

                case SelectorClient.State.Queue:

                    break;

                case SelectorClient.State.OnList:

                    break;
            }
        }

        public void Version(string Packet)
        {
            if (Packet.Contains(Config.ConfigurationManager.GetString("Auth_Version")))
            {
                Client.m_State = SelectorClient.State.Account;
            }
            else
            {
                Client.Send("AlEv" + Config.ConfigurationManager.GetString("Auth_Version"));
            }
        }

        public void Account(string Packet)
        {
            string[] Infos = Packet.Split('#');
            string Username = Infos[0];
            string Password = Infos[1];

        }
    }
}
