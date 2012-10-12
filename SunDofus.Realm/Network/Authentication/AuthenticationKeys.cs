using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Network.Authentication
{
    class AuthenticationKeys
    {
        public static List<AuthenticationKeys> myKeys = new List<AuthenticationKeys>();

        public string myKey;
        public Database.Models.Clients.AccountModel myInfos;

        public AuthenticationKeys(string Packet)
        {
            myKey = "";
            myInfos = new Database.Models.Clients.AccountModel();

            string[] Data = Packet.Split('|');
            myKey = Data[1];
            myInfos.myId = int.Parse(Data[2]);
            myInfos.mymPseudo = Data[3];
            myInfos.myQuestion = Data[4];
            myInfos.myAnswer = Data[5];
            myInfos.myLevel = int.Parse(Data[6]);
            myInfos.Characters = Data[7];
            myInfos.mySubscription = long.Parse(Data[8]);
            myInfos.Gifts = Data[9];
        }
    }
}
