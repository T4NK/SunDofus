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

        public static void SendTradeMessage(Network.Realm.RealmClient myClient, string Message)
        {
            if (myClient.myPlayer.CanSendinTrade() == true)
            {
                foreach (var me in Network.ServersHandler.myRealmServer.myClients.Where(x => x.isAuth == true))
                    me.Send(string.Format("cMK:|{0}|{1}|{2}", myClient.myPlayer.ID, myClient.myPlayer.myName, Message));

                myClient.myPlayer.RefreshTrade();
            }
            else
                myClient.Send(string.Format("Im0115;{0}", myClient.myPlayer.TimeTrade()));
        }

        public static void SendRecruitmentMessage(Network.Realm.RealmClient myClient, string Message)
        {
            if (myClient.myPlayer.CanSendinRecruitment() == true)
            {
                foreach (var me in Network.ServersHandler.myRealmServer.myClients.Where(x => x.isAuth == true))
                    me.Send(string.Format("cMK?|{0}|{1}|{2}", myClient.myPlayer.ID, myClient.myPlayer.myName, Message));

                myClient.myPlayer.RefreshRecruitment();
            }
            else
                myClient.Send(string.Format("Im0115;{0}", myClient.myPlayer.TimeRecruitment()));
        }
    }
}
