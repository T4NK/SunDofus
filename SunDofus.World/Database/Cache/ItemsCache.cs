using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SunDofus.Database.Cache
{
    class ItemsCache
    {
        public static List<Models.Items.ItemModel> ItemsList = new List<Models.Items.ItemModel>();
        public static List<Models.Items.SetModel> SetsList = new List<Models.Items.SetModel>();
        public static List<Models.Items.ItemUsableModel> UsablesList = new List<Models.Items.ItemUsableModel>();

        public static void LoadItems()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_items";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var item = new Models.Items.ItemModel();

                    item.ID = sqlReader.GetInt32("ID");
                    item.Pods = sqlReader.GetInt16("Weight");
                    item.Price = sqlReader.GetInt32("Price");
                    item.Type = sqlReader.GetInt16("Type");
                    item.Level = sqlReader.GetInt16("Level");
                    item.Jet = sqlReader.GetString("Stats");
                    item.Condistr = sqlReader.GetString("Conditions");

                    item.ParseWeaponInfos(sqlReader.GetString("WeaponInfo"));

                    item.ParseRandomJet();

                    lock(ItemsList)
                        ItemsList.Add(item);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' items@ from the database !", ItemsList.Count));
        }

        public static void LoadItemsSets()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_items_sets";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var set = new Models.Items.SetModel();

                    set.ID = sqlReader.GetInt16("ID");
                    set.ParseBonus(sqlReader.GetString("bonus"));
                    set.ParseItems(sqlReader.GetString("items"));

                    lock(SetsList)
                        SetsList.Add(set);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' items sets@ from the database !", SetsList.Count));
        }

        public static void LoadUsablesItems()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_items_usables";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var item = new Models.Items.ItemUsableModel();

                    item.Base = sqlReader.GetInt16("ID");
                    item.Args = sqlReader.GetString("Args");

                    if (sqlReader.GetInt16("MustDelete") == 1)
                        item.MustDelete = true;
                    else
                        item.MustDelete = false;

                    item.AttributeItem();

                    lock(UsablesList)
                        UsablesList.Add(item);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' items usables@ from the database !", UsablesList.Count));
        }
    }
}
