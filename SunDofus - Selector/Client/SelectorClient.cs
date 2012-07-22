using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace selector.Client
{
    class SelectorClient : AbstractClient
    {
        public string m_Key = "";
        public SelectorParser m_Parser;
        public State m_State;

        public SelectorClient(SilverSocket Socket) : base(Socket)
        {
            this.RaiseClosedEvent += new OnClosedEvent(this.Disconnected);
            this.RaiseDataArrivalEvent += new DataArrivalEvent(this.PacketReceived);
            m_Key = Utils.Basic.RandomString(32);
            m_State = State.Version;
            m_Parser = new SelectorParser(this);
            Send("HC" + m_Key);
        }

        public void Disconnected()
        {
            Utils.Logger.Infos("New losted connection !");
            Program.m_Auth.m_Clients.Remove(this);
        }

        public void PacketReceived(string Data)
        {
            m_Parser.Parse(Data);
        }

        public enum State
        {
            Version,
            Account,
            Queue,
            OnList,
            None,
        }
    }
}
