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
            lock (DatabaseHandler.myLocker)
            {
                var SQLText = "SELECT * FROM items";
                var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

                while (SQLReader.Read())
                {
                    var myItem = new Models.Items.ItemModel();

                    myItem.myID = SQLReader.GetInt32("ID");
                    myItem.myPods = SQLReader.GetInt16("Weight");
                    myItem.myPrice = SQLReader.GetInt32("Price");
                    myItem.myType = SQLReader.GetInt16("Type");
                    myItem.myLevel = SQLReader.GetInt16("Level");
                    myItem.myJet = SQLReader.GetString("Stats");
                    myItem.myConditions = SQLReader.GetString("Conditions");

                    ItemsList.Add(myItem);
                }

                SQLReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' items@ from the database !", ItemsList.Count));
        }

        public static void LoadItemsSets()
        {
            lock (DatabaseHandler.myLocker)
            {
                var SQLText = "SELECT * FROM items_sets";
                var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

                while (SQLReader.Read())
                {
                    var mySet = new Models.Items.SetModel();

                    mySet.myID = SQLReader.GetInt16("ID");
                    mySet.ParseBonus(SQLReader.GetString("bonus"));
                    mySet.ParseItems(SQLReader.GetString("items"));

                    SetsList.Add(mySet);
                }

                SQLReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' items sets@ from the database !", SetsList.Count));
        }

        public static void LoadUsablesItems()
        {
            lock (DatabaseHandler.myLocker)
            {
                var SQLText = "SELECT * FROM items_usables";
                var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

                while (SQLReader.Read())
                {
                    var myUsable = new Models.Items.ItemUsableModel();

                    myUsable.myBaseItemID = SQLReader.GetInt16("ID");
                    myUsable.myArgs = SQLReader.GetString("Args");

                    myUsable.AttributeItem();

                    UsablesList.Add(myUsable);
                }

                SQLReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' items usables@ from the database !", UsablesList.Count));
        }
    }
}
