﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SunDofus.Auth.Network.Sync
{
    class SyncAction
    {
        public static void UpdateCharacters(int accountID, string character, int serverID, bool add = true)
        {
            lock (SunDofus.Auth.Entities.DatabaseProvider.ConnectionLocker)
            {
                var account = SunDofus.Auth.Entities.Requests.AccountsRequests.LoadAccount(accountID);

                if (account == null)
                    return;

                if (add)
                    account.Characters[serverID].Add(character);
                else
                    account.Characters[serverID].Remove(character);

                var sqlText = "UPDATE dyn_accounts SET characters=@Characters WHERE Id=@id";
                var sqlCommand = new MySqlCommand(sqlText, SunDofus.Auth.Entities.DatabaseProvider.Connection);

                sqlCommand.Parameters.Add(new MySqlParameter("@id", accountID));
                sqlCommand.Parameters.Add(new MySqlParameter("@Characters", account.CharactersString()));

                sqlCommand.ExecuteNonQuery();
            }
        }

        public static void UpdateConnectedValue(int accountID, bool isConnected)
        {
            if (accountID == -1)
                return;

            lock (SunDofus.Auth.Entities.DatabaseProvider.ConnectionLocker)
            {
                var sqlText = "UPDATE dyn_accounts SET connected=@connected WHERE Id=@id";
                var sqlCommand = new MySqlCommand(sqlText, SunDofus.Auth.Entities.DatabaseProvider.Connection);

                sqlCommand.Parameters.Add(new MySqlParameter("@id", accountID));
                sqlCommand.Parameters.Add(new MySqlParameter("@connected", (isConnected ? 1 : 0)));

                sqlCommand.ExecuteNonQuery();
            }
        }

        public static void DeleteGift(int giftID, int accountID)
        {
            lock (SunDofus.Auth.Entities.DatabaseProvider.ConnectionLocker)
            {
                var sqlText = "DELETE FROM dyn_gifts WHERE id=@id";
                var sqlCommand = new MySqlCommand(sqlText, SunDofus.Auth.Entities.DatabaseProvider.Connection);

                sqlCommand.Parameters.Add(new MySqlParameter("@id", giftID));

                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
