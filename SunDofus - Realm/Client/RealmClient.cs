using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace realm.Client
{
    class RealmClient : SunDofus.AbstractClient
    {
        RealmParser m_Parser;
        public RealmInfos m_Infos;
        public List<Realm.Character.Character> m_Characters;
        public Realm.Character.Character m_Player = null;
        public bool isAuth = false;

        public RealmClient(SilverSocket Socket) :  base(Socket)
        {
            this.RaiseClosedEvent += new OnClosedEvent(this.Disconnected);
            this.RaiseDataArrivalEvent += new DataArrivalEvent(this.ReceivedPackets);
            m_Characters = new List<Realm.Character.Character>();
            m_Parser = new RealmParser(this);
            Send("HG");
        }

        public void ReceivedPackets(string Data)
        {
            m_Parser.Parse(Data);
        }

        public void Disconnected()
        {
            SunDofus.Logger.Infos("New closed connection !");
            if (isAuth == true)
            {
                Program.m_RealmLink.Send("DC|" + m_Infos.Pseudo);
                if (m_Player != null)
                {
                    m_Player.GetMap().DelPlayer(m_Player);
                    m_Player.isConnected = false;
                }
            }
            Program.m_AuthServer.m_Clients.Remove(this);
        }

        public void ParseCharacters()
        {
            foreach (string Name in m_Infos.CharactersNames)
            {
                Realm.Character.Character m_C = Realm.Character.CharactersManager.CharactersList.First(x => x.m_Name == Name);
                if (m_C != null)
                {
                    m_Characters.Add(m_C);
                }
            }
        }
    }
}
