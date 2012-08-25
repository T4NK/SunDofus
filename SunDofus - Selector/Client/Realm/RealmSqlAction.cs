using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace selector.Client
{
    class RealmSqlAction
    {
        public static void UpdateCharacters(int CompteID, string NewCharacters)
        {
            string SQLText = "UPDATE accounts SET characters=@NewCharacters WHERE id=@Me";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, Database.SQLManager.m_Connection);

            SQLCommand.Parameters.Add(new MySqlParameter("@Me", CompteID));
            SQLCommand.Parameters.Add(new MySqlParameter("@NewCharacters", NewCharacters));

            SQLCommand.ExecuteNonQuery();
        }
    }
}
