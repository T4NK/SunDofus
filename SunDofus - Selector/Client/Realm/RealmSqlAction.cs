using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace selector.Client
{
    class RealmSqlAction
    {
        public static void UpdateCharacters(int CompteID, string NewCharacters, int ServerID)
        {
            try
            {
                Database.Data.Account myAccount = Database.AccountsManager.myAccounts.First(x => x.Id == CompteID);
                myAccount.ParseCharacter(NewCharacters);

                string SQLText = "UPDATE accounts SET characters=@NewCharacters WHERE Id=@Me";
                MySqlCommand SQLCommand = new MySqlCommand(SQLText, Database.SQLManager.m_Connection);

                SQLCommand.Parameters.Add(new MySqlParameter("@Me", CompteID));
                SQLCommand.Parameters.Add(new MySqlParameter("@NewCharacters", NewCharacters));

                SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SunDofus.Logger.Error(e);
            }
        }

        public static void DeleteGift(int GiftID, int CompteID)
        {
            try
            {
                Database.Data.Account myAccount = Database.AccountsManager.myAccounts.First(x => x.Id == CompteID);
                Database.GiftsManager.myGifts.Remove(Database.GiftsManager.myGifts.First(x => x.id == GiftID && x.target == CompteID));

                string SQLText = "DELETE FROM gifts WHERE id=@ID";
                MySqlCommand SQLCommand = new MySqlCommand(SQLText, Database.SQLManager.m_Connection);

                SQLCommand.Parameters.Add(new MySqlParameter("@ID", GiftID));

                SQLCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SunDofus.Logger.Error(e);
            }
        }
    }
}
