using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace realm.Network.Realm
{
    class RealmClient : SunDofus.Network.AbstractClient
    {
        public Database.Models.Clients.AccountModel myInfos;
        public List<realm.Realm.Character.Character> myCharacters;
        public realm.Realm.Character.Character myPlayer = null;
        public bool isAuth = false;
        public RealmCommand myCommander;

        object myPacketLocker;
        RealmParser myParser;

        public RealmClient(SilverSocket Socket) :  base(Socket)
        {
            myPacketLocker = new object();

            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);
            this.ReceivedDatas += new ReceiveDatasHandler(this.ReceivedPackets);

            myCharacters = new List<realm.Realm.Character.Character>();
            myCommander = new RealmCommand(this);
            myParser = new RealmParser(this);

            Send("HG");
        }

        public void Send(string Message)
        {
            this.SendDatas(Message);
            Utilities.Loggers.InfosLogger.Write(string.Format("Sent to @<{0}>@ : {1}", myIp(), Message));
        }

        public void ParseCharacters()
        {
            foreach (var Name in myInfos.myCharacters)
            {
                if (!realm.Realm.Character.CharactersManager.CharactersList.Any(x => x.myName == Name)) continue;

                var m_C = realm.Realm.Character.CharactersManager.CharactersList.First(x => x.myName == Name);
                myCharacters.Add(m_C);
            }
        }

        public void SendGifts()
        {
            myInfos.ParseGifts();

            foreach (var myGift in myInfos.myGifts)
            {
                if (Database.Cache.ItemsCache.ItemsList.Any(x => x.myID == myGift.myItemID) == false)
                    return;

                var Item = new realm.Realm.Character.Items.CharacterItem(Database.Cache.ItemsCache.ItemsList.First(x => x.myID == myGift.myItemID));

                Item.ParseJet();
                Item.GeneratItem();

                myGift.myItem = Item;

                this.Send(string.Format("Ag1|{0}|{1}|{2}|{3}|{4}~{5}~{6}~~{7};", myGift.myId, myGift.myTitle, myGift.myMessage, "http://s2.e-monsite.com/2009/12/26/04/167wpr7.png",
                   Utilities.Basic.DeciToHex(Item.myBaseItem.myID), Utilities.Basic.DeciToHex(Item.myBaseItem.myID), Utilities.Basic.DeciToHex(Item.myQuantity), Item.EffectsInfos()));
            }
        }

        public void SendConsoleMessage(string Message, int Color = 1)
        {
            Send(string.Format("BAT{0}{1}", Color, Message));
        }

        public void SendMessage(string Message)
        {
            Send(string.Format("cs<font color=\"#FF0000\">{0}</font>", Message));
        }

        void ReceivedPackets(string Data)
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("Receive datas from @<{0}>@ : {1}", myIp(), Data));

            lock (myPacketLocker)
                myParser.Parse(Data);
        }

        void Disconnected()
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("New closed client @<{0}>@ connection !", myIp()));

            if (isAuth == true)
            {
                Network.ServersHandler.myAuthLink.Send(string.Format("SND|{0}", myInfos.mymPseudo));
                if (myPlayer != null)
                {
                    myPlayer.GetMap().DelPlayer(myPlayer);
                    myPlayer.isConnected = false;
                }
            }
            Network.ServersHandler.myRealmServer.myClients.Remove(this);
        }
    }
}
