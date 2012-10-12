using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.Character;

namespace realm.Realm.World
{
    class Chat
    {
        public static void SendGeneralMessage(Network.Realm.RealmClient Client, string Message)
        {
            if (Client.myPlayer.GetMap() == null) 
                return;

            Client.myPlayer.GetMap().Send(string.Format("cMK|{0}|{1}|{2}", Client.myPlayer.ID, Client.myPlayer.myName, Message));
        }

        public static void SendPrivateMessage(Network.Realm.RealmClient Client, string Receiver, string Message)
        {
            if(CharactersManager.CharactersList.Any(x => x.myName == Receiver))
            {
                var myCharacter = CharactersManager.CharactersList.First(x => x.myName == Receiver);

                if (myCharacter.isConnected == true)
                {
                    myCharacter.Client.Send(string.Format("cMKF|{0}|{1}|{2}", Client.myPlayer.ID, Client.myPlayer.myName, Message));
                    Client.Send(string.Format("cMKT|{0}|{1}|{2}", Client.myPlayer.ID, myCharacter.myName, Message));
                }
                else
                    Client.Send(string.Format("cMEf{0}", Receiver));
            }
        }
    }
}
