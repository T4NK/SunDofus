using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Timers;

namespace SunDofus.Database.Cache
{
    class GiftsCache
    {
        public static List<Models.GiftModel> _gifts = new List<Models.GiftModel>();

        public static List<Models.GiftModel> Cache
        {
            get
            {
                return _gifts;
            }
            set
            {
                _gifts = value;
            }
        }

        private static bool _started = false;
        private static Timer _cache = new Timer();

        public static void ReloadCache(object sender = null, EventArgs e = null)
        {
            Utilities.Loggers.InfosLogger.Write("Reloading of @Gifts' Cache@ ...");

            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM dyn_gifts";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var gifts = new Models.GiftModel();
                    {
                        gifts.ID = sqlReader.GetInt16("Id");
                        gifts.Target = sqlReader.GetInt16("Target");
                        gifts.ItemID = sqlReader.GetInt16("ItemID");
                        gifts.Title = sqlReader.GetString("Title");
                        gifts.Message = sqlReader.GetString("Message");
                        gifts.Image = sqlReader.GetString("Image");
                    }

                    lock (_gifts)
                    {
                        if (!_gifts.Any(x => x.ID == gifts.ID))
                            _gifts.Add(gifts);
                    }
                }

                sqlReader.Close();
            }

            if (!_started == true)
            {
                _started = true;
                _cache.Interval = Utilities.Config.GetConfig.GetIntElement("Time_Gifts_Reload");
                _cache.Enabled = true;
                _cache.Elapsed += new ElapsedEventHandler(ReloadCache);
            }
        }
    }
}
