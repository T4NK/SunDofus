using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace selector.Database.Data
{
    class Account
    {
        public int Id = -1;
        public string Username = "";
        public string Password = "";
        public int Level = -1;
        public int Communauty = -1;
        public string Pseudo = "";
        public string Question = "";
        public string Answer = "";
        public string BaseGifts = "";
        public string BaseChar = "";
        public DateTime SubscriptionDate = new DateTime();

        public Dictionary<int, List<string>> Characters = new Dictionary<int, List<string>>();

        public void ParseCharacter(string BaseCharacters)
        {
            if (BaseCharacters == "")
            {
                Characters = new Dictionary<int, List<string>>();
                return;
            }

            BaseChar = BaseCharacters;
            Dictionary<int, List<string>> Dico = new Dictionary<int, List<string>>();
            string[] AllData = BaseCharacters.Split(':');
            foreach (string Data in AllData)
            {
                string[] CharData = Data.Split(',');
                if (!Dico.ContainsKey(int.Parse(CharData[1]))) Dico.Add(int.Parse(CharData[1]), new List<string>());
                Dico[int.Parse(CharData[1])].Add(CharData[0]);
            }

            Characters = Dico;
        }

        public string SubscriptionTime()
        {
            if (SubscriptionDate.Subtract(DateTime.Now).TotalMilliseconds <= 1) return "0";
            return SubscriptionDate.Subtract(DateTime.Now).TotalMilliseconds.ToString().Split(',')[0];
        }
    }
}
