using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace selector.Client
{
    class RealmClient : AbstractClient
    {
        public State m_State;
        public RealmParser m_Parser;
        public Database.Data.Server m_Server;

        public RealmClient(SilverSocket Socket)
            : base(Socket)
        {
            this.RaiseDataArrivalEvent += new DataArrivalEvent(this.PacketsReceived);
            this.RaiseClosedEvent += new OnClosedEvent(this.Disconnected);
            m_State = State.Auth;
            m_Parser = new RealmParser(this);
        }

        public void PacketsReceived(string Data)
        {
            m_Parser.Parse(Data);
        }

        public void Disconnected()
        {
            ChangeState(State.Disconnected);
            Utils.Logger.Infos("New losed server connection ('" + m_Server.ID + "') !");
            Program.m_Realm.m_Clients.Remove(this);
        }

        public void ChangeState(State NewState)
        {
            this.m_State = NewState;

            switch (this.m_State)
            { 
                case State.Auth:
                    m_Server.Connected = 0;
                    break;

                case State.Connected:
                    m_Server.Connected = 1;
                    break;

                case State.Disconnected:
                    m_Server.Connected = 0;
                    break;

                case State.Maintenance:
                    m_Server.Connected = 2;
                    break;
            }

            Program.m_Auth.RefreshAllHosts();
        }

        public enum State
        {
            Auth,
            Connected,
            Disconnected,
            Maintenance,
        }
    }
}
