using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace auth.Database.Models
{
    class AccountModel
    {
        public int myId = -1, myLevel = -1, myCommunauty = -1;
        public string myUsername = "", myPassword = "", myPseudo = "";
        public string myQuestion = "", myAnswer = "";
        public string myBaseChar = "";

        public DateTime mySubscriptionDate;
        public Dictionary<int, List<string>> myCharacters;

        public AccountModel()
        {
            mySubscriptionDate = new DateTime();
            myCharacters = new Dictionary<int, List<string>>();
        }

        public void ParseCharacter(string BaseCharacters)
        {
            if (BaseCharacters == "")
            {
                myCharacters = new Dictionary<int, List<string>>();
                return;
            }

            myBaseChar = BaseCharacters;
            Dictionary<int, List<string>> Dico = new Dictionary<int, List<string>>();
            string[] AllData = BaseCharacters.Split(':');
            foreach (string Data in AllData)
            {
                string[] CharData = Data.Split(',');
                if (!Dico.ContainsKey(int.Parse(CharData[1]))) Dico.Add(int.Parse(CharData[1]), new List<string>());
                Dico[int.Parse(CharData[1])].Add(CharData[0]);
            }

            myCharacters = Dico;
        }

        public long mySubscriptionTime()
        {
            var Time = mySubscriptionDate.Subtract(DateTime.Now).TotalMilliseconds;

            if (Utilities.Config.m_config.GetBoolElement("Subscription_Time") == false)
                return 31536000000;
            else if (mySubscriptionDate.Subtract(DateTime.Now).TotalMilliseconds <= 1)
                return 0;
            else if (Time >= Utilities.Config.m_config.GetLongElement("Max_Subscription_Time"))
                return 31536000000;

            return (long)Time;
        }
    }
}
