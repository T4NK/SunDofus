using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Data
{
    class MapSql
    {
        public static List<Realm.Map.Map> MapsList = new List<Realm.Map.Map>();

        public static void LoadMaps()
        {
            string SQLText = "SELECT * FROM maps";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, SQLManager.m_Connection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Realm.Map.Map m_M = new Realm.Map.Map();

                m_M.id = SQLReader.GetInt16("id");
                m_M.date = SQLReader.GetInt32("date");
                m_M.width = SQLReader.GetInt16("width");
                m_M.height = SQLReader.GetInt16("heigth");
                m_M.capabilities = SQLReader.GetInt16("capabilities");
                m_M.numgroup = SQLReader.GetInt16("numgroup");
                m_M.groupmaxsize = SQLReader.GetInt16("groupmaxsize");
                m_M.mappos = SQLReader.GetString("mappos");
                m_M.monsters = SQLReader.GetString("monsters");
                m_M.cells = SQLReader.GetString("cells");
                m_M.MapData = SQLReader.GetString("mapData");
                m_M.key = SQLReader.GetString("key");

                MapsList.Add(m_M);
            }

            SQLReader.Close();

            SunDofus.Logger.Status("Loaded '" + MapsList.Count + "' maps from the database !");
        }
    }
}
