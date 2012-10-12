using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace auth.Network.Sync
{
    class SyncAction
    {
        public static void UpdateCharacters(int CompteID, string NewCharacters, int ServerID)
        {
            try
            {
                Database.Models.AccountModel myAccount = Database.Cache.AccountsCache.myAccounts.First(x => x.myId == CompteID);
                myAccount.ParseCharacter(NewCharacters);

                var SQLText = "UPDATE accounts SET characters=@NewCharacters WHERE Id=@Me";
                var SQLCommand = new MySqlCommand(SQLText, Database.DatabaseHandler.myConnection);

                SQLCommand.Parameters.Add(new MySqlParameter("@Me", CompteID));
                SQLCommand.Parameters.Add(new MySqlParameter("@NewCharacters", NewCharacters));

                SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot update characters from the account {0} ({1})", CompteID, e.ToString()));
            }
        }

        public static void DeleteGift(int GiftID, int CompteID)
        {
            try
            {
                Database.Cache.GiftsCache.myGifts.Remove(Database.Cache.GiftsCache.myGifts.First(x => x.myId == GiftID && x.myTarget == CompteID));

                var SQLText = "DELETE FROM gifts WHERE id=@ID";
                var SQLCommand = new MySqlCommand(SQLText, Database.DatabaseHandler.myConnection);

                SQLCommand.Parameters.Add(new MySqlParameter("@ID", GiftID));

                SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot remove gift from the account {0} ({1})", CompteID, e.ToString()));
            }
        }
    }
}
