using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Timers;

namespace DofusOrigin.Database.Cache
{
    class AccountsCache
    {
        public static Models.AccountModel LoadAccount(string username)
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var account = new Models.AccountModel();

                var sqlText = "SELECT * FROM dyn_accounts WHERE username=@username";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);
                sqlCommand.Parameters.Add(new MySqlParameter("@username", username));

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                if (sqlReader.Read())
                {

                    account.ID = sqlReader.GetInt16("id");
                    account.Username = sqlReader.GetString("username");
                    account.Password = sqlReader.GetString("password");
                    account.Pseudo = sqlReader.GetString("pseudo");
                    account.Communauty = sqlReader.GetInt16("communauty");
                    account.Level = sqlReader.GetInt16("gmLevel");
                    account.Question = sqlReader.GetString("question");
                    account.Answer = sqlReader.GetString("answer");

                    account.ParseCharacters(sqlReader.GetString("characters"));
                    account.SubscriptionDate = sqlReader.GetDateTime("subscription");

                }

                sqlReader.Close();

                return account;
            }
        }

        public static Models.AccountModel LoadAccount(int accountID)
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var account = new Models.AccountModel();

                var sqlText = "SELECT * FROM dyn_accounts WHERE id=@id";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);
                sqlCommand.Parameters.Add(new MySqlParameter("@id", accountID));

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                if (sqlReader.Read())
                {

                    account.ID = sqlReader.GetInt16("id");
                    account.Username = sqlReader.GetString("username");
                    account.Password = sqlReader.GetString("password");
                    account.Pseudo = sqlReader.GetString("pseudo");
                    account.Communauty = sqlReader.GetInt16("communauty");
                    account.Level = sqlReader.GetInt16("gmLevel");
                    account.Question = sqlReader.GetString("question");
                    account.Answer = sqlReader.GetString("answer");

                    account.ParseCharacters(sqlReader.GetString("characters"));
                    account.SubscriptionDate = sqlReader.GetDateTime("subscription");

                }

                sqlReader.Close();

                return account;
            }
        }

        public static int GetAccountID(string pseudo)
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var accountID = -1;

                var sqlText = "SELECT id FROM dyn_accounts WHERE pseudo=@pseudo";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);
                sqlCommand.Parameters.Add(new MySqlParameter("@pseudo", pseudo));

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                if (sqlReader.Read())
                    accountID = sqlReader.GetInt32("id");

                sqlReader.Close();

                return accountID;
            }
        }

        public static void ResetConnectedValue()
        {
            lock (Database.DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "UPDATE dyn_accounts SET connected=@connected";
                var sqlCommand = new MySqlCommand(sqlText, Database.DatabaseHandler.Connection);

                sqlCommand.Parameters.Add(new MySqlParameter("@connected", 0));

                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
