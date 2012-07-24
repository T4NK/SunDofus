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

        public List<string> CharactersNames;

        public RealmInfos()
        {
            CharactersNames = new List<string>();
            Pseudo = "";
            Question = "";
            Answer = "";
            Id = -1;
            Level = -1;
            Characters = "";
        }

        public void ParseCharacters()
        {
            if (Characters == "") return;
            string[] AllData = Characters.Split(':');
            foreach (string Data in AllData)
            {
                string[] CharData = Data.Split(',');
                if (!CharactersNames.Contains(CharData[0]) && Program.m_ServerID == int.Parse(CharData[1]))
                    CharactersNames.Add(CharData[0]);
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
            CharactersNames.Add(Name);
            return Characters;
        }
    }
}
