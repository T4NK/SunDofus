using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Timers;

namespace DofusOrigin.Database.Cache
{
    class GiftsCache
    {
        public static List<Models.GiftModel> m_gifts = new List<Models.GiftModel>();

        private static bool m_started = false;
        private static Timer m_cache = new Timer();

        public static void ReloadCache(object sender = null, EventArgs e = null)
        {
            Utilities.Loggers.m_infosLogger.Write("Reloading of @Gifts' Cache@ ...");

            lock (DatabaseHandler.m_locker)
            {
                string sqlText = "SELECT * FROM dyn_gifts";
                MySqlCommand sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);
                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var gifts = new Models.GiftModel();
                    {
                        gifts.m_id = sqlReader.GetInt16("Id");
                        gifts.m_target = sqlReader.GetInt16("Target");
                        gifts.m_itemID = sqlReader.GetInt16("ItemID");
                        gifts.m_title = sqlReader.GetString("Title");
                        gifts.m_message = sqlReader.GetString("Message");
                    }

                    if (!m_gifts.Any(x => x.m_id == gifts.m_id))
                        m_gifts.Add(gifts);
                }

                sqlReader.Close();
            }

            if (!m_started == true)
            {
                m_started = true;
                m_cache.Interval = Utilities.Config.m_config.GetIntElement("Time_Gifts_Reload");
                m_cache.Enabled = true;
                m_cache.Elapsed += new ElapsedEventHandler(ReloadCache);
            }
        }
    }
}
