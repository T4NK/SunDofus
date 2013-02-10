using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Realm.Characters;

namespace DofusOrigin.Realm.World
{
    class Chat
    {
        public static void SendGeneralMessage(Network.Realm.RealmClient client, string message)
        {
            if (client.m_player.GetMap() == null) 
                return;

            client.m_player.GetMap().Send(string.Format("cMK|{0}|{1}|{2}", client.m_player.m_id, client.m_player.m_name, message));
        }

        public static void SendPrivateMessage(Network.Realm.RealmClient client, string receiver, string message)
        {
            lock (CharactersManager.CharactersList)
            {
                if (CharactersManager.CharactersList.Any(x => x.m_name == receiver))
                {
                    var character = CharactersManager.CharactersList.First(x => x.m_name == receiver);

                    if (character.isConnected == true)
                    {
                        character.m_networkClient.Send(string.Format("cMKF|{0}|{1}|{2}", client.m_player.m_id, client.m_player.m_name, message));
                        client.Send(string.Format("cMKT|{0}|{1}|{2}", client.m_player.m_id, character.m_name, message));
                    }
                    else
                        client.Send(string.Format("cMEf{0}", receiver));
                }
            }
        }

        public static void SendTradeMessage(Network.Realm.RealmClient client, string message)
        {
            if (client.m_player.CanSendinTrade() == true)
            {
                lock (Network.ServersHandler.m_realmServer.m_clients)
                {
                    foreach (var character in Network.ServersHandler.m_realmServer.m_clients.Where(x => x.isAuth == true))
                        character.Send(string.Format("cMK:|{0}|{1}|{2}", client.m_player.m_id, client.m_player.m_name, message));
                }

                client.m_player.RefreshTrade();
            }
            else
                client.Send(string.Format("Im0115;{0}", client.m_player.TimeTrade()));
        }

        public static void SendRecruitmentMessage(Network.Realm.RealmClient client, string message)
        {
            if (client.m_player.CanSendinRecruitment() == true)
            {
                lock (Network.ServersHandler.m_realmServer.m_clients)
                {
                    foreach (var character in Network.ServersHandler.m_realmServer.m_clients.Where(x => x.isAuth == true))
                        character.Send(string.Format("cMK?|{0}|{1}|{2}", client.m_player.m_id, client.m_player.m_name, message));
                }

                client.m_player.RefreshRecruitment();
            }
            else
                client.Send(string.Format("Im0115;{0}", client.m_player.TimeRecruitment()));
        }

        public static void SendPartyMessage(Network.Realm.RealmClient client, string message)
        {
            if (client.m_player.m_state.Party != null)
            {
                lock (client.m_player.m_state.Party.Members)
                {
                    foreach (var character in client.m_player.m_state.Party.Members.Keys)
                        character.m_networkClient.Send(string.Format("cMK$|{0}|{1}|{2}", client.m_player.m_id, client.m_player.m_name, message));
                }
            }
        }

        public static void SendAdminMessage(Network.Realm.RealmClient client, string message)
        {
            if (client.m_infos.m_level > 0)
            {
                lock (Network.ServersHandler.m_realmServer.m_clients)
                {
                    foreach (var character in Network.ServersHandler.m_realmServer.m_clients.Where(x => x.isAuth == true && x.m_infos.m_level > 0))
                        character.Send(string.Format("cMK@|{0}|{1}|{2}", client.m_player.m_id, client.m_player.m_name, message));
                }
            }
        }
    }
}
