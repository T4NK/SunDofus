using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using realm.Realm.Characters;
using realm.Database.Models.Clients;

namespace realm.Network.Realms
{
    class RealmClient : SunDofus.Network.TCPClient
    {
        public bool isAuth { get; set; }

        public Character m_player { get; set; }
        public List<Character> m_characters { get; set; }
        public AccountModel m_infos { get; set; }
        public RealmCommand m_commander { get; set; }

        object m_packetLocker { get; set; }
        RealmParser m_parser { get; set; }

        public RealmClient(SilverSocket _socket) :  base(_socket)
        {
            m_packetLocker = new object();

            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);
            this.ReceivedDatas += new ReceiveDatasHandler(this.ReceivedPackets);

            m_characters = new List<realm.Realm.Characters.Character>();
            m_commander = new RealmCommand(this);
            m_parser = new RealmParser(this);

            m_player = null;
            isAuth = false;

            Send("HG");
        }

        public void Send(string _message)
        {
            this.SendDatas(_message);
            Utilities.Loggers.m_infosLogger.Write(string.Format("Sent to @<{0}>@ : {1}", myIp(), _message));
        }

        public void ParseCharacters()
        {
            foreach (var name in m_infos.m_characters)
            {
                if (!realm.Realm.Characters.CharactersManager.m_charactersList.Any(x => x.m_name == name))
                    continue;

                var character = realm.Realm.Characters.CharactersManager.m_charactersList.First(x => x.m_name == name);
                m_characters.Add(character);
            }
        }

        public void SendGifts()
        {
            m_infos.ParseGifts();

            foreach (var gift in m_infos.m_gifts)
            {
                if (Database.Cache.ItemsCache.m_itemsList.Any(x => x.m_id == gift.m_itemID) == false)
                    return;

                var item = new realm.Realm.Characters.Items.CharacterItem(Database.Cache.ItemsCache.m_itemsList.First(x => x.m_id == gift.m_itemID));

                item.ParseJet();
                item.GeneratItem();

                gift.m_item = item;

                this.Send(string.Format("Ag1|{0}|{1}|{2}|{3}|{4}~{5}~{6}~~{7};", gift.m_id, gift.m_title, gift.m_message, "http://s2.e-monsite.com/2009/12/26/04/167wpr7.png",
                   Utilities.Basic.DeciToHex(item.m_base.m_id), Utilities.Basic.DeciToHex(item.m_base.m_id), Utilities.Basic.DeciToHex(item.m_quantity), item.EffectsInfos()));
            }
        }

        public void SendConsoleMessage(string _message, int Color = 1)
        {
            Send(string.Format("BAT{0}{1}", Color, _message));
        }

        public void SendMessage(string _message)
        {
            Send(string.Format("cs<font color=\"#FF0000\">{0}</font>", _message));
        }

        void ReceivedPackets(string _datas)
        {
            Utilities.Loggers.m_infosLogger.Write(string.Format("Receive datas from @<{0}>@ : {1}", myIp(), _datas));

            lock (m_packetLocker)
                m_parser.Parse(_datas);
        }

        void Disconnected()
        {
            Utilities.Loggers.m_infosLogger.Write(string.Format("New closed client @<{0}>@ connection !", myIp()));

            if (isAuth == true)
            {
                Network.ServersHandler.m_authLinks.Send(string.Format("SND|{0}", m_infos.m_pseudo));

                if (m_player != null)
                {
                    m_player.GetMap().DelPlayer(m_player);
                    m_player.isConnected = false;
                }
            }
            Network.ServersHandler.m_realmServer.m_clients.Remove(this);
        }
    }
}
