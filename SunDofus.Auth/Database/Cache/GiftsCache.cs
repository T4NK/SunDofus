using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Timers;

namespace auth.Database.Cache
{
    class GiftsCache
    {
        public static List<Models.GiftModel> myGifts = new List<Models.GiftModel>();
        static bool AutoStarted = false;
        static Timer AutoCache = new Timer();

        public static void ReloadCache(object sender = null, EventArgs e = null)
        {
            Utilities.Loggers.InfosLogger.Write("Reloading of @Gifts' Cache@ !");

            try
            {
                lock (DatabaseHandler.myLocker)
                {
                    string Text = "SELECT * FROM gifts";
                    MySqlCommand Command = new MySqlCommand(Text, DatabaseHandler.myConnection);
                    MySqlDataReader Reader = Command.ExecuteReader();

                    while (Reader.Read())
                    {
                        Models.GiftModel newGifts = new Models.GiftModel();

                        newGifts.myId = Reader.GetInt16("Id");
                        newGifts.myTarget = Reader.GetInt16("Target");
                        newGifts.myItemID = Reader.GetInt16("ItemID");
                        newGifts.myTitle = Reader.GetString("Title");
                        newGifts.myMessage = Reader.GetString("Message");

                        if (!myGifts.Any(x => x.myId == newGifts.myId))
                            myGifts.Add(newGifts);
                    }

                    Reader.Close();
                }

                if (!AutoStarted == true)
                {
                    AutoStarted = true;
                    AutoCache.Interval = Utilities.Config.myConfig.GetIntElement("Time_Gifts_Reload");
                    AutoCache.Enabled = true;
                    AutoCache.Elapsed += new ElapsedEventHandler(ReloadCache);
                }
            }
            catch (Exception ex)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot reload @gifts@ ({0})", ex.ToString()));
            }
        }
    }
}
