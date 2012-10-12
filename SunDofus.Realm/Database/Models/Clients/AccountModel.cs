using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.Clients
{
    class AccountModel
    {
        public string mymPseudo;
        public string myQuestion;
        public string myAnswer;
        public int myId;
        public int myLevel;
        public long mySubscription;
        public string Characters;
        public string Gifts;

        public List<string> myCharacters;
        public List<GiftModel> myGifts;

        public AccountModel()
        {
            myCharacters = new List<string>();
            myGifts = new List<GiftModel>();

            mymPseudo = "";
            myQuestion = "";
            myAnswer = "";
            myId = -1;
            myLevel = -1;
            Characters = "";
            mySubscription = 0;
            Gifts = "";
        }

        public void ParseCharacters()
        {
            if (Characters == "") return;
            string[] AllData = Characters.Split(':');
            foreach (var Data in AllData)
            {
                string[] CharData = Data.Split(',');
                if (!myCharacters.Contains(CharData[0]) && Utilities.Config.myConfig.GetIntElement("ServerId") == int.Parse(CharData[1]))
                    myCharacters.Add(CharData[0]);
            }
        }

        public void ParseGifts()
        {
            if (Gifts == "") return;
            string[] AllData = Gifts.Split('+');
            foreach (var Data in AllData)
            {
                string[] Infos = Data.Split('~');
                var myGift = new GiftModel();
                myGift.myId = int.Parse(Infos[0]);
                myGift.myTitle = Infos[1];
                myGift.myMessage = Infos[2];
                myGift.myItemID = int.Parse(Infos[3]);

                myGifts.Add(myGift);
            }
        }

        public string AddNewCharacterToAccount(string Name)
        {
            if (Characters == "")
                Characters = string.Format("{0},{1}", Name, Utilities.Config.myConfig.GetIntElement("ServerId"));
            else
                Characters += string.Format(":{0},{1}", Name, Utilities.Config.myConfig.GetIntElement("ServerId"));

            myCharacters.Add(Name);
            return Characters;
        }

        public string RemoveCharacterToAccount(string Name)
        {
            if (Characters == (string.Format("{0},{1}", Name, Utilities.Config.myConfig.GetIntElement("ServerId"))))
                Characters = Characters.Replace(string.Format("{0},{1}", Name, Utilities.Config.myConfig.GetIntElement("ServerId")), "");

            else if (Characters.StartsWith(string.Format("{0},{1}:", Name, Utilities.Config.myConfig.GetIntElement("ServerId"))))
                Characters = Characters.Replace(string.Format("{0},{1}:", Name, Utilities.Config.myConfig.GetIntElement("ServerId")), "");

            else
                Characters = Characters.Replace(string.Format(":{0},{1}", Name, Utilities.Config.myConfig.GetIntElement("ServerId")), "");

            myCharacters.Remove(Name);
            return Characters;
        }
    }
}
