using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Cache
{
    class ItemsCache
    {
        public static List<Models.Items.ItemModel> ItemsList = new List<Models.Items.ItemModel>();
        public static List<Models.Items.SetModel> SetsList = new List<Models.Items.SetModel>();
        public static List<Models.Items.ItemUsableModel> UsablesList = new List<Models.Items.ItemUsableModel>();

        public static void LoadItems()
        {
            string SQLText = "SELECT * FROM items";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Models.Items.ItemModel myI = new Models.Items.ItemModel();

                myI.ID = SQLReader.GetInt32("ID");
                myI.Pods = SQLReader.GetInt16("Weight");
                myI.Price = SQLReader.GetInt32("Price");
                myI.Type = SQLReader.GetInt16("Type");
                myI.Level = SQLReader.GetInt16("Level");
                myI.Jet = SQLReader.GetString("Stats");
                myI.Conditions = SQLReader.GetString("Conditions");

                ItemsList.Add(myI);
            }

            SQLReader.Close();

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded '{0}' items from the database !", ItemsList.Count));
        }

        public static void LoadItemsSets()
        {
            string SQLText = "SELECT * FROM items_sets";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Models.Items.SetModel myS = new Models.Items.SetModel();

                myS.ID = SQLReader.GetInt16("ID");
                myS.ParseBonus(SQLReader.GetString("bonus"));
                myS.ParseItems(SQLReader.GetString("items"));

                SetsList.Add(myS);
            }

            SQLReader.Close();

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded '{0}' items_sets from the database !", SetsList.Count));
        }

        public static void LoadUsablesItems()
        {
            string SQLText = "SELECT * FROM items_usables";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Models.Items.ItemUsableModel myU = new Models.Items.ItemUsableModel();

                myU.BaseItemID = SQLReader.GetInt16("ID");
                myU.Args = SQLReader.GetString("Args");

                myU.AttributeItem();

                UsablesList.Add(myU);
            }

            SQLReader.Close();

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded '{0}' items_usables from the database !", UsablesList.Count));
        }
    }
}
