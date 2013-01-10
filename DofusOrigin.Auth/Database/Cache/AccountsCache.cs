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
        public static List<Models.AccountModel> m_accounts = new List<Models.AccountModel>();

        private static bool m_started = false;
        private static Timer m_cache = new Timer();

        public static void ReloadCache(object sender = null, EventArgs e = null)
        {
            Utilities.Loggers.m_infosLogger.Write("Reloading of @Accounts' Cache@ ...");

            lock (DatabaseHandler.m_locker)
            {
                string sqlText = "SELECT * FROM dyn_accounts";
                MySqlCommand sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);
                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var account = new Models.AccountModel();
                    {
                        account.m_id = sqlReader.GetInt16("id");
                        account.m_username = sqlReader.GetString("username");
                        account.m_password = sqlReader.GetString("password");
                        account.m_pseudo = sqlReader.GetString("pseudo");
                        account.m_communauty = sqlReader.GetInt16("communauty");
                        account.m_level = sqlReader.GetInt16("gmLevel");
                        account.m_question = sqlReader.GetString("question");
                        account.m_answer = sqlReader.GetString("answer");
                        account.ParseCharacter(sqlReader.GetString("characters"));
                        account.m_subscriptionDate = sqlReader.GetDateTime("subscription");
                    }

                    if (!m_accounts.Any(x => x.m_id == account.m_id))
                        m_accounts.Add(account);
                }

                sqlReader.Close();
            }

            if (!m_started == true)
            {
                m_started = true;
                m_cache.Interval = Utilities.Config.m_config.GetIntElement("Time_Accounts_Reload");
                m_cache.Enabled = true;
                m_cache.Elapsed += new ElapsedEventHandler(ReloadCache);
            }
        }
    }
}
