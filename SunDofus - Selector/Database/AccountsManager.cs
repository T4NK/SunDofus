using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Timers;

namespace selector.Database
{
    class AccountsManager
    {
        public static List<Data.Account> myAccounts = new List<Data.Account>();
        static bool AutoReload = false;
        static Timer AutoTimer = new Timer();

        public static void ReloadCache(object sender, EventArgs e)
        {
            if (SQLManager.isRunning == true) return;
            try
            {
                SQLManager.isRunning = true;
                string Text = "SELECT * FROM accounts";
                MySqlCommand Command = new MySqlCommand(Text, SQLManager.m_Connection);
                MySqlDataReader Reader = Command.ExecuteReader();

                while (Reader.Read())
                {
                    Data.Account newAccount = new Data.Account();
                    newAccount.Id = Reader.GetInt16("id");
                    newAccount.Username = Reader.GetString("username");
                    newAccount.Password = Reader.GetString("password");
                    newAccount.Pseudo = Reader.GetString("pseudo");
                    newAccount.Communauty = Reader.GetInt16("communauty");
                    newAccount.Level = Reader.GetInt16("gmLevel");
                    newAccount.Question = Reader.GetString("question");
                    newAccount.Answer = Reader.GetString("answer");
                    newAccount.ParseCharacter(Reader.GetString("characters"));
                    newAccount.SubscriptionDate = Reader.GetDateTime("subscription");

                    if (myAccounts.Count(x => x.Id == newAccount.Id && x.Password == newAccount.Password) == 0)
                    {
                        myAccounts.Add(newAccount);
                    }
                }

                Reader.Close();
                SQLManager.isRunning = false;

                SunDofus.Logger.Infos("'" + myAccounts.Count + "' accounts reloaded !");

                if (!AutoReload == true)
                {
                    AutoReload = true;
                    AutoTimer.Interval = Config.ConfigurationManager.GetInt("TimeReloadAccounts");
                    AutoTimer.Enabled = true;
                    AutoTimer.Elapsed += new ElapsedEventHandler(ReloadCache);
                }
            }
            catch (Exception ex)
            {
                SunDofus.Logger.Error(ex);
            }
        }
    }
}
