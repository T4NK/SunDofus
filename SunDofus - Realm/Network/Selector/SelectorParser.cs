using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Network
{
    class SelectorParser
    {
        public SelectorLink Client;

        public SelectorParser(SelectorLink m_C)
        {
            Client = m_C;
        }

        public void Parse(string Data)
        {
            string[] Packet = Data.Split('|');

            switch (Packet[0])
            {
                case "FailedToConnect!":
                    Environment.Exit(0);
                    break;

                case "Connected!":
                    Utils.Logger.Status("Server authentified !");
                    break;

                case"ANT":
                    ParseNewTicket(Data);
                    break;
            }
        }

        public void ParseNewTicket(string Data)
        {
            SelectorKeys.m_Keys.Add(new SelectorKeys(Data));            
        }
    }
}
