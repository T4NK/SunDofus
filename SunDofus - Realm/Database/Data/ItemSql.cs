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

        public static void LoadItems()
        {
            string SQLText = "SELECT * FROM items";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, SQLManager.m_Connection);

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

                ItemsList.Add(m_I);
            }

            SQLReader.Close();

            SunDofus.Logger.Status("Loaded '" + ItemsList.Count + "' items from the database !");
        }
    }
}
