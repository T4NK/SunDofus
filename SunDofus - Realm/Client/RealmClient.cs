using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace realm.Client
{
    class RealmClient
    {
        SilverSocket m_Socket;
        public State m_State;
        RealmParser m_Parser;
        public RealmInfos m_Infos;
        public List<Realm.Character.Character> m_Characters;

        public RealmClient(SilverSocket Socket)
        {
            m_Socket = Socket;
            m_Socket.OnDataArrivalEvent += new SilverEvents.DataArrival(this.ReceivedPackets);
            m_Socket.OnSocketClosedEvent += new SilverEvents.SocketClosed(this.Disconnected);
            m_State = State.Ticket;
            m_Characters = new List<Realm.Character.Character>();
            m_Parser = new RealmParser(this);
            Send("HG");
        }

        public void ReceivedPackets(byte[] data)
        {
            string NotParsed = Encoding.ASCII.GetString(data);
            foreach (string Packet in NotParsed.Replace("\x0a", "").Split('\x00'))
            {
                if (Packet == "") continue;
                Utils.Logger.Packets("[Received]! " + Packet);
                m_Parser.Parse(Packet);
            }
        }

        public void Disconnected()
        {
            Utils.Logger.Infos("New closed connection !");
            Program.m_AuthServer.m_Clients.Remove(this);
        }

        public void Send(string Message)
        {
            Utils.Logger.Packets("[Sended]! " + Message);
            byte[] P = Encoding.ASCII.GetBytes(Message + "\x00");
            m_Socket.Send(P);
        }

        public void ParseCharacters()
        {
            foreach (string Name in m_Infos.CharactersNames)
            {
                Realm.Character.Character m_C = Realm.Character.CharactersManager.GetCharacter(Name);
                if (m_C != null)
                {
                    m_Characters.Add(m_C);
                }
            }
        }

        public enum State
        { 
            Ticket,
            Character,
            InGame,
        }
    }
}
