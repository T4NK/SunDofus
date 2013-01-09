using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace auth.Network.Sync
{
    class SyncAction
    {
        public static void UpdateCharacters(int _compteID, string _character, int _serverID, bool _add = true)
        {
            try
            {
                lock(Database.DatabaseHandler.m_locker)
                {
                    var account = Database.Cache.AccountsCache.m_accounts.First(x => x.m_id == _compteID);

                    if(_add)
                        account.m_characters[_serverID].Add(_character);
                    else
                        account.m_characters[_serverID].Remove(_character);

                    var sqlText = "UPDATE dyn_accounts SET characters=@Characters WHERE Id=@id";
                    var sqlCommand = new MySqlCommand(sqlText, Database.DatabaseHandler.m_connection);

                    sqlCommand.Parameters.Add(new MySqlParameter("@id", _compteID));
                    sqlCommand.Parameters.Add(new MySqlParameter("@Characters", account.CharactersSaveString()));

                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Utilities.Loggers.m_errorsLogger.Write(string.Format("Cannot update characters from the account {0} ({1})", _compteID, e.ToString()));
            }
        }

        public static void DeleteGift(int _giftID, int _compteID)
        {
            try
            {
                lock (Database.DatabaseHandler.m_locker)
                {
                    Database.Cache.GiftsCache.m_gifts.Remove(Database.Cache.GiftsCache.m_gifts.First(x => x.m_id == _giftID && x.m_target == _compteID));

                    var sqlText = "DELETE FROM dyn_gifts WHERE id=@id";
                    var sqlCommand = new MySqlCommand(sqlText, Database.DatabaseHandler.m_connection);

                    sqlCommand.Parameters.Add(new MySqlParameter("@id", _giftID));

                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Utilities.Loggers.m_errorsLogger.Write(string.Format("Cannot remove gift from the account {0} ({1})", _compteID, e.ToString()));
            }
        }
    }
}
