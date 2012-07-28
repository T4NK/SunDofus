using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Data
{
    class TriggerSql
    {
        public static List<Realm.Map.Trigger> TriggersList = new List<Realm.Map.Trigger>();

        public static void LoadTriggers()
        {
            string SQLText = "SELECT * FROM triggers";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, SQLManager.m_Connection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Realm.Map.Trigger m_T = new Realm.Map.Trigger();

                m_T.MapID = SQLReader.GetInt16("MapID");
                m_T.CellID = SQLReader.GetInt16("CellID");
                m_T.NewMapID = int.Parse(SQLReader.GetString("NewMap").Split(',')[0]);
                m_T.NewCellID = int.Parse(SQLReader.GetString("NewMap").Split(',')[1]);

                TriggersList.Add(m_T);
                ParseTrigger(m_T);
            }

            SQLReader.Close();

            SunDofus.Logger.Status("Loaded '" + TriggersList.Count + "' triggers from the database !");
        }

        public static void ParseTrigger(Realm.Map.Trigger m_T)
        {
            foreach (Realm.Map.Map m_M in MapSql.MapsList)
            {
                if (m_M.id == m_T.MapID) m_M.m_Triggers.Add(m_T);
            }
        }
    }
}
