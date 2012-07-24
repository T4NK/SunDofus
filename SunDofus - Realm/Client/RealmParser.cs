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
                    Client.m_Infos.ParseCharacters();

                    Client.m_State = RealmClient.State.Character;

                    Program.m_RealmLink.Send("NC|" + Client.m_Infos.Pseudo);
                    Client.Send("ATK0");
                }
            }
        }

        public void ParseCharacter(string Data)
        {
            if (Data.Substring(0, 1) == "A")
            {
                switch (Data.Substring(1, 1))
                {
                    case "A":
                        // ADD
                        break;

                    case "D":
                        // DELETE
                        break;

                    case "L":
                        SendCharacterList();
                        break;

                    case "P":
                        //RANDOM PSEUDO
                        break;

                    case "S":
                        //SELECT
                        break;

                    case "V":
                        Client.Send("AV0");
                        break;
                }
            }
        }

        public void SendCharacterList()
        {
            long MemberTime = 60 * 60 * 24 * 365;
            string Pack = "ALK" + (MemberTime * 1000) + "|" + Client.m_Infos.CharactersNames.Count;

            Client.Send(Pack);
        }

        public void ParseInGame(string Data)
        { 

        }
    }
}
