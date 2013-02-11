using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Cache
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

                    item.m_id = sqlReader.GetInt32("ID");
                    item.m_pods = sqlReader.GetInt16("Weight");
                    item.m_price = sqlReader.GetInt32("Price");
                    item.m_type = sqlReader.GetInt16("Type");
                    item.m_level = sqlReader.GetInt16("Level");
                    item.m_jet = sqlReader.GetString("Stats");
                    item.m_condistr = sqlReader.GetString("Conditions");

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

                    set.m_id = sqlReader.GetInt16("ID");
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

                    item.m_base = sqlReader.GetInt16("ID");
                    item.m_args = sqlReader.GetString("Args");

                    if (sqlReader.GetInt16("MustDelete") == 1)
                        item.m_mustDelete = true;
                    else
                        item.m_mustDelete = false;

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
