using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Timers;

namespace selector.Database
{
    class GiftsManager
    {
        public static List<Data.Gift> myGifts = new List<Data.Gift>();
        static bool AutoReload = false;
        static Timer AutoTimer = new Timer();

        public static void ReloadCache(object sender, EventArgs e)
        {
            if (SQLManager.isRunning == true) return;
            try
            {
                SQLManager.isRunning = true;
                string Text = "SELECT * FROM gifts";
                MySqlCommand Command = new MySqlCommand(Text, SQLManager.m_Connection);
                MySqlDataReader Reader = Command.ExecuteReader();

                while (Reader.Read())
                {
                    Data.Gift newGifts = new Data.Gift();

                    newGifts.id = Reader.GetInt16("Id");
                    newGifts.target = Reader.GetInt16("Target");
                    newGifts.itemID = Reader.GetInt16("ItemID");
                    newGifts.title = Reader.GetString("Title");
                    newGifts.message = Reader.GetString("Message");

                    if (myGifts.Count(x => x.id == newGifts.id) == 0)
                    {
                        myGifts.Add(newGifts);
                    }
                }

                Reader.Close();
                SQLManager.isRunning = false;

                SunDofus.Logger.Infos("'" + myGifts.Count + "' gifts reloaded !");

                if (!AutoReload == true)
                {
                    AutoReload = true;
                    AutoTimer.Interval = Config.ConfigurationManager.GetInt("TimeReloadGifts");
                    AutoTimer.Enabled = true;
                    AutoTimer.Elapsed += new ElapsedEventHandler(ReloadCache);
                }
            }
            catch (Exception ex)
            {
                SunDofus.Logger.Error(ex);
            }
        }
    }
}
