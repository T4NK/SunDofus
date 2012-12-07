using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Cache
{
    class MapsCache
    {
        public static List<Realm.Maps.Map> m_mapsList = new List<Realm.Maps.Map>();

        public static void LoadMaps()
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "SELECT * FROM datas_maps";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var map = new Models.Maps.MapModel();

                    map.m_id = sqlReader.GetInt32("id");
                    map.m_date = sqlReader.GetInt32("date");
                    map.m_width = sqlReader.GetInt16("width");
                    map.m_height = sqlReader.GetInt16("heigth");
                    map.m_capabilities = sqlReader.GetInt16("capabilities");
                    map.m_mappos = sqlReader.GetString("mappos");
                    map.m_mapData = sqlReader.GetString("mapData");
                    map.m_key = sqlReader.GetString("key");

                    map.ParsePos();

                    m_mapsList.Add(new Realm.Maps.Map(map));

                    Utilities.Loggers.m_infosLogger.Write(string.Format("Loaded map @{0}@ !", map.m_id));
                }

                sqlReader.Close();
            }

            Utilities.Loggers.m_statusLogger.Write(string.Format("Loaded @'{0}' maps@ from the database !", m_mapsList.Count));
        }
    }
}
