using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Timers;

namespace auth.Database.Cache
{
    class AccountsCache
    {
        public static List<Models.AccountModel> myAccounts = new List<Models.AccountModel>();
        static bool AutoStarted = false;
        static Timer AutoCache = new Timer();

        public static void ReloadCache(object sender = null, EventArgs e = null)
        {
            Utilities.Loggers.InfosLogger.Write("Reloading of @Accounts' Cache@ !");

            try
            {
                lock (DatabaseHandler.myLocker)
                {
                    string Text = "SELECT * FROM accounts";
                    MySqlCommand Command = new MySqlCommand(Text, DatabaseHandler.myConnection);
                    MySqlDataReader Reader = Command.ExecuteReader();

                    while (Reader.Read())
                    {
                        Models.AccountModel newAccount = new Models.AccountModel();
                        newAccount.myId = Reader.GetInt16("id");
                        newAccount.myUsername = Reader.GetString("username");
                        newAccount.myPassword = Reader.GetString("password");
                        newAccount.myPseudo = Reader.GetString("pseudo");
                        newAccount.myCommunauty = Reader.GetInt16("communauty");
                        newAccount.myLevel = Reader.GetInt16("gmLevel");
                        newAccount.myQuestion = Reader.GetString("question");
                        newAccount.myAnswer = Reader.GetString("answer");
                        newAccount.ParseCharacter(Reader.GetString("characters"));
                        newAccount.mySubscriptionDate = Reader.GetDateTime("subscription");

                        if (!myAccounts.Any(x => x.myId == newAccount.myId))
                            myAccounts.Add(newAccount);
                    }

                    Reader.Close();
                }

                if (!AutoStarted == true)
                {
                    AutoStarted = true;
                    AutoCache.Interval = Utilities.Config.myConfig.GetIntElement("Time_Accounts_Reload");
                    AutoCache.Enabled = true;
                    AutoCache.Elapsed += new ElapsedEventHandler(ReloadCache);
                }

                foreach (var server in Network.ServersHandler.mySyncServer.myClients.Where(x => x.myState == Network.Sync.SyncClient.State.Connected))
                {
                    foreach (var acc in myAccounts.Where(x => x.myLevel > 0))
                        server.Send(string.Format("ANAA|{0}|{1}", acc.myUsername, acc.myPassword));
                }
            }
            catch (Exception ex)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot reload @accounts@ ({0})", ex.ToString()));
            }
        }
    }
}
