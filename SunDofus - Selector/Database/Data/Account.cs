using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace selector.Database.Data
{
    class Account
    {
        public Account(string Username)
        {
            ReloadThis(Username);
        }

        public void ReloadThis(string Username)
        {
            try
            {
                string SQLText = "SELECT * FROM accounts WHERE username=@name";
                MySqlCommand SQLCommand = new MySqlCommand(SQLText, SQLManager.m_Connection);
                SQLCommand.Parameters.Add(new MySqlParameter("@name", Username));
                MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

                if (SQLReader.Read())
                {
                    Id = SQLReader.GetInt16("id");
                    Username = SQLReader.GetString("username");
                    Password = SQLReader.GetString("password");
                    Level = SQLReader.GetInt16("level");
                    Communauty = SQLReader.GetInt16("communauty");
                    Pseudo = SQLReader.GetString("pseudo");
                    Question = SQLReader.GetString("question");
                    Answer = SQLReader.GetString("answer");
                    BaseChar = SQLReader.GetString("characters");
                    if (BaseChar != "") ParseCharacter();
                    BaseGifts = SQLReader.GetString("gifts");
                    SubscriptionDate = SQLReader.GetDateTime("subscription");
                }

                SQLReader.Close();
            }
            catch (Exception e)
            {
                SunDofus.Logger.Error(e);
            }
        }

        public int Id = -1;
        public string Username = "";
        public string Password = "";
        public int Level = -1;
        public int Communauty = -1;
        public string Pseudo = "";
        public string Question = "";
        public string Answer = "";
        public string BaseChar = "";
        public string BaseGifts = "";
        public DateTime SubscriptionDate = new DateTime();

        public Dictionary<int, List<string>> Characters = new Dictionary<int, List<string>>();

        public void ParseCharacter()
        {
            Dictionary<int, List<string>> Dico = new Dictionary<int, List<string>>();
            string[] AllData = BaseChar.Split(':');
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
