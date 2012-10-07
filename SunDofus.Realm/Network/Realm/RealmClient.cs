using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace realm.Network.Realm
{
    class RealmClient : SunDofus.AbstractClient
    {
        public Database.Models.Clients.AccountModel m_Infos;
        public List<realm.Realm.Character.Character> m_Characters;
        public realm.Realm.Character.Character m_Player = null;
        public bool isAuth = false;
        public RealmCommand m_Commander;

        object PacketLocker;
        RealmParser myParser;

        public RealmClient(SilverSocket Socket) :  base(Socket)
        {
            PacketLocker = new object();

            this.RaiseClosedEvent += new OnClosedEvent(this.Disconnected);
            this.RaiseDataArrivalEvent += new DataArrivalEvent(this.ReceivedPackets);

            m_Characters = new List<realm.Realm.Character.Character>();
            m_Commander = new RealmCommand(this);
            myParser = new RealmParser(this);

            Send("HG");
        }

        public void ParseCharacters()
        {
            foreach (string Name in m_Infos.myCharacters)
            {
                realm.Realm.Character.Character m_C = realm.Realm.Character.CharactersManager.CharactersList.First(x => x.m_Name == Name);
                if (m_C != null)
                {
                    m_Characters.Add(m_C);
                }
            }
        }

        public void SendGifts()
        {
            m_Infos.ParseGifts();
            foreach (Database.Models.Clients.GiftModel myGift in m_Infos.myGifts)
            {
                realm.Realm.Character.Items.CharacterItem Item = new realm.Realm.Character.Items.CharacterItem(Database.Cache.ItemsCache.ItemsList.First(x => x.ID == myGift.itemID));
                Item.ParseJet();
                Item.GeneratItem();

                myGift.item = Item;

                this.Send("Ag1|" + myGift.id + "|" + myGift.title + "|" + myGift.message + "|http://s2.e-monsite.com/2009/12/26/04/167wpr7.png" + "|" + Utilities.Basic.DeciToHex(Item.BaseItem.ID) +
                    "~" + Utilities.Basic.DeciToHex(Item.BaseItem.ID) + "~" + Utilities.Basic.DeciToHex(Item.Quantity) + "~~" + Item.EffectsInfos() + ";");
            }
        }

        public void SendConsoleMessage(string Message, int Color)
        {
            Send("BAT" + Color + Message);
        }

        public void SendMessage(string Message)
        {
            Send("cs<font color=\"#FF0000\">" + Message + "</font>");
        }

        void ReceivedPackets(string Data)
        {
            lock (PacketLocker)
                myParser.Parse(Data);
        }

        void Disconnected()
        {
            Utilities.Loggers.InfosLogger.Write("New closed client connection !");
            if (isAuth == true)
            {
                Network.ServersHandler.myAuthLink.Send("DC|" + m_Infos.Pseudo);
                if (m_Player != null)
                {
                    m_Player.GetMap().DelPlayer(m_Player);
                    m_Player.isConnected = false;
                }
            }
            Network.ServersHandler.myRealmServer.m_Clients.Remove(this);
        }
    }
}
