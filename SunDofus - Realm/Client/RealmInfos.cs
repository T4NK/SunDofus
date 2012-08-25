using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Client
{
    class RealmInfos
    {
        public string Pseudo;
        public string Question;
        public string Answer;
        public int Id;
        public int Level;
        public string Characters;
        public long Subscription;
        public string Gifts;

        public List<string> myCharacters;
        public List<RealmGifts> myGifts;

        public RealmInfos()
        {
            myCharacters = new List<string>();
            myGifts = new List<RealmGifts>();

            Pseudo = "";
            Question = "";
            Answer = "";
            Id = -1;
            Level = -1;
            Characters = "";
            Subscription = 0;
            Gifts = "";
        }

        public void ParseCharacters()
        {
            if (Characters == "") return;
            string[] AllData = Characters.Split(':');
            foreach (string Data in AllData)
            {
                string[] CharData = Data.Split(',');
                if (!myCharacters.Contains(CharData[0]) && Program.m_ServerID == int.Parse(CharData[1]))
                    myCharacters.Add(CharData[0]);
            }
        }

        public void ParseGifts()
        {
            if (Gifts == "") return;
            string[] AllData = Gifts.Split('+');
            foreach (string Data in AllData)
            {
                string[] Infos = Data.Split('~');
                RealmGifts myGift = new RealmGifts();
                myGift.id = int.Parse(Infos[0]);
                myGift.title = Infos[1];
                myGift.message = Infos[2];
                myGift.itemID = int.Parse(Infos[3]);

                myGifts.Add(myGift);
            }
        }

        public string AddNewCharacterToAccount(string Name)
        {
            if (Characters == "")
            {
                Characters = Name + "," + Program.m_ServerID;
            }
            else
            {
                Characters = Characters + ":" + Name + "," + Program.m_ServerID;
            }
            myCharacters.Add(Name);
            return Characters;
        }

        public string RemoveCharacterToAccount(string Name)
        {
            if (Characters == (Name + "," + Program.m_ServerID))
            {
                Characters = Characters.Replace(Name + "," + Program.m_ServerID, "");
            }
            else if (Characters.StartsWith(Name + "," + Program.m_ServerID + ":"))
            {
                Characters = Characters.Replace(Name + "," + Program.m_ServerID + ":", "");
            }
            else
            {
                Characters = Characters.Replace(":" + Name + "," + Program.m_ServerID, "");
            }
            myCharacters.Remove(Name);
            return Characters;
        }
    }
}
