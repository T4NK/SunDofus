using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.Characters;

namespace realm.Realm.World
{
    class Chat
    {
        public static void SendGeneralMessage(Network.Realms.RealmClient _client, string _message)
        {
            if (_client.m_player.GetMap() == null) 
                return;

            _client.m_player.GetMap().Send(string.Format("cMK|{0}|{1}|{2}", _client.m_player.m_id, _client.m_player.m_name, _message));
        }

        public static void SendPrivateMessage(Network.Realms.RealmClient _client, string _receiver, string _message)
        {
            if(CharactersManager.m_charactersList.Any(x => x.m_name == _receiver))
            {
                var character = CharactersManager.m_charactersList.First(x => x.m_name == _receiver);

                if (character.isConnected == true)
                {
                    character.m_networkClient.Send(string.Format("cMKF|{0}|{1}|{2}", _client.m_player.m_id, _client.m_player.m_name, _message));
                    _client.Send(string.Format("cMKT|{0}|{1}|{2}", _client.m_player.m_id, character.m_name, _message));
                }
                else
                    _client.Send(string.Format("cMEf{0}", _receiver));
            }
        }

        public static void SendTradeMessage(Network.Realms.RealmClient _client, string _message)
        {
            if (_client.m_player.CanSendinTrade() == true)
            {
                foreach (var character in Network.ServersHandler.m_realmServer.m_clients.Where(x => x.isAuth == true))
                    character.Send(string.Format("cMK:|{0}|{1}|{2}", _client.m_player.m_id, _client.m_player.m_name, _message));

                _client.m_player.RefreshTrade();
            }
            else
                _client.Send(string.Format("Im0115;{0}", _client.m_player.TimeTrade()));
        }

        public static void SendRecruitmentMessage(Network.Realms.RealmClient _client, string _message)
        {
            if (_client.m_player.CanSendinRecruitment() == true)
            {
                foreach (var character in Network.ServersHandler.m_realmServer.m_clients.Where(x => x.isAuth == true))
                    character.Send(string.Format("cMK?|{0}|{1}|{2}", _client.m_player.m_id, _client.m_player.m_name, _message));

                _client.m_player.RefreshRecruitment();
            }
            else
                _client.Send(string.Format("Im0115;{0}", _client.m_player.TimeRecruitment()));
        }
    }
}
