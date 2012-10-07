using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Data
{
    class ItemSql
    {
        public static List<Realm.Character.Items.AbstractItem> ItemsList = new List<Realm.Character.Items.AbstractItem>();
        public static List<Realm.Character.Items.AbstractSet> SetsList = new List<Realm.Character.Items.AbstractSet>();
        public static List<Realm.Character.Items.CharUsableItem> UsablesList = new List<Realm.Character.Items.CharUsableItem>();

        public static void LoadItems()
        {
            string SQLText = "SELECT * FROM items";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Realm.Character.Items.AbstractItem m_I = new Realm.Character.Items.AbstractItem();

                m_I.ID = SQLReader.GetInt32("ID");
                m_I.Pods = SQLReader.GetInt16("Weight");
                m_I.Price = SQLReader.GetInt32("Price");
                m_I.Type = SQLReader.GetInt16("Type");
                m_I.Level = SQLReader.GetInt16("Level");
                m_I.Jet = SQLReader.GetString("Stats");
                m_I.Conditions = SQLReader.GetString("Conditions");

                ItemsList.Add(m_I);
            }

            SQLReader.Close();

            SunDofus.Logger.Status("Loaded '" + ItemsList.Count + "' items from the database !");
        }

        public static void LoadItemsSets()
        {
            string SQLText = "SELECT * FROM items_sets";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Realm.Character.Items.AbstractSet m_S = new Realm.Character.Items.AbstractSet();

                m_S.ID = SQLReader.GetInt16("ID");
                m_S.ParseBonus(SQLReader.GetString("bonus"));
                m_S.ParseItems(SQLReader.GetString("items"));

                SetsList.Add(m_S);
            }

            SQLReader.Close();

            SunDofus.Logger.Status("Loaded '" + SetsList.Count + "' items_sets from the database !");
        }

        public static void LoadUsablesItems()
        {
            string SQLText = "SELECT * FROM items_usables";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Realm.Character.Items.CharUsableItem m_U = new Realm.Character.Items.CharUsableItem();

                m_U.BaseItemID = SQLReader.GetInt16("ID");
                m_U.Args = SQLReader.GetString("Args");

                m_U.AttributeItem();

                UsablesList.Add(m_U);
            }

            SQLReader.Close();

            SunDofus.Logger.Status("Loaded '" + UsablesList.Count + "' items_usables from the database !");
        }
    }
}
