using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.Character;

namespace realm.Realm.World
{
    class Chat
    {
        public static void SendGeneralMessage(Client.RealmClient Client, string Message)
        {
            if (Client.m_Player.GetMap() == null) return;
            Client.m_Player.GetMap().Send("cMK|" + Client.m_Player.ID + "|" + Client.m_Player.m_Name + "|" + Message);
        }

        public static void SendPrivateMessage(Client.RealmClient Client, string Receiver, string Message)
        {
            if(CharactersManager.CharactersList.Any(x => x.m_Name == Receiver))
            {
                Character.Character m_C = CharactersManager.CharactersList.First(x => x.m_Name == Receiver);
                if (m_C.isConnected == true)
                {
                    m_C.Client.Send("cMKF|" + Client.m_Player.ID + "|" + Client.m_Player.m_Name + "|" + Message);
                    Client.Send("cMKT|" + Client.m_Player.ID + "|" + m_C.m_Name + "|" + Message);
                }
                else
                {
                    Client.Send("cMEf" + Receiver);
                }
            }
        }
    }
}
