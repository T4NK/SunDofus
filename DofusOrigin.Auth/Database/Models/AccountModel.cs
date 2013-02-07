using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Models
{
    class AccountModel
    {
        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        private int _level;

        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        private int _communauty;

        public int Communauty
        {
            get
            {
                return _communauty;
            }
            set
            {
                _communauty = value;
            }
        }

        private string _username;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        private string _password;

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        private string _pseudo;

        public string Pseudo
        {
            get
            {
                return _pseudo;
            }
            set
            {
                _pseudo = value;
            }
        }

        private string _question;

        public string Question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = value;
            }
        }

        public string _answer;

        public string Answer
        {
            get
            {
                return _answer;
            }
            set
            {
                _answer = value;
            }
        }

        public DateTime SubscriptionDate;
        public Dictionary<int, List<string>> Characters;

        public AccountModel()
        {
            SubscriptionDate = new DateTime();
            Characters = new Dictionary<int, List<string>>();
        }

        public void ParseCharacters(string charstr)
        {
            if (charstr == "")
            {
                Characters = new Dictionary<int, List<string>>();
                return;
            }

            var dico = new Dictionary<int, List<string>>();
            var datas = charstr.Split(':');

            foreach (var infos in datas)
            {
                var characterdatas = infos.Split(',');

                if (!dico.ContainsKey(int.Parse(characterdatas[1]))) 
                    dico.Add(int.Parse(characterdatas[1]), new List<string>());

                dico[int.Parse(characterdatas[1])].Add(characterdatas[0]);
            }

            Characters = dico;
        }

        public long SubscriptionTime()
        {
            var time = SubscriptionDate.Subtract(DateTime.Now).TotalMilliseconds;

            if (Utilities.Config.GetConfig.GetBoolElement("Subscription_Time") == false)
                return 31536000000;
            else if (SubscriptionDate.Subtract(DateTime.Now).TotalMilliseconds <= 1)
                return 0;
            else if (time >= Utilities.Config.GetConfig.GetLongElement("Max_Subscription_Time"))
                return 31536000000;

            return (long)time;
        }

        public string CharactersString()
        {
            var str = "";

            foreach (var i in Characters.Keys)
            {
                if (Characters[i].Count < 1)
                    continue;

                foreach (var character in Characters[i])
                    str += string.Format("{0},{1}:", character, i);
            }

            return (str == "" ? str : str.Substring(0, str.Length - 1));
        }
    }
}
