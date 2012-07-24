using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Client
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
            switch (Client.m_State)
            { 
                case RealmClient.State.Ticket:
                    ParseTicket(Data);
                    break;

                case RealmClient.State.Character:
                    ParseCharacter(Data);
                    break;

                case RealmClient.State.InGame:
                    ParseInGame(Data);
                    break;
            }
        }

        public void ParseTicket(string Data)
        {
            Data = Data.Replace("AT", "");
            foreach (Network.SelectorKeys Key in Network.SelectorKeys.m_Keys)
            {
                if (Key.m_Key == Data)
                {
                    Client.m_Infos = Key.m_Infos;
                    Client.m_State = RealmClient.State.Character;
                    Client.Send("ATK0");
                }
            }
        }

        public void ParseCharacter(string Data)
        {

        }

        public void ParseInGame(string Data)
        { 

        }
    }
}
