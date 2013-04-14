using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SunDofus.Database.Cache
{
    class MapsCache
    {
        public static List<Realm.Maps.Map> MapsList = new List<Realm.Maps.Map>();

        public static void LoadMaps()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_maps";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var map = new Models.Maps.MapModel();

                    map.ID = sqlReader.GetInt32("id");
                    map.Date = sqlReader.GetInt32("date");
                    map.Width = sqlReader.GetInt16("width");
                    map.Height = sqlReader.GetInt16("heigth");
                    map.Capabilities = sqlReader.GetInt16("capabilities");
                    map.Mappos = sqlReader.GetString("mappos");
                    map.MapData = sqlReader.GetString("mapData");
                    map.Key = sqlReader.GetString("key");
                    map.MaxMonstersGroup = sqlReader.GetInt16("maxgroups");
                    map.MaxGroupSize = sqlReader.GetInt16("groupsize");

                    foreach (var newMonster in sqlReader.GetString("monsters").Split('|'))
                    {
                        if (newMonster == "")
                            continue;

                        var infos = newMonster.Split(',');

                        if (infos.Length < 2)
                            continue;
                        if (infos[1].Length < 1)
                            continue;

                        lock (map.Monsters)
                        {
                            if (!map.Monsters.ContainsKey(int.Parse(infos[0])))
                                map.Monsters.Add(int.Parse(infos[0]), new List<int>());
                        }

                        lock(map.Monsters[int.Parse(infos[0])])
                            map.Monsters[int.Parse(infos[0])].Add(int.Parse(infos[1]));
                    }

                    map.ParsePos();

                    lock(MapsList)
                        MapsList.Add(new Realm.Maps.Map(map));
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' maps@ from the database !", MapsList.Count));
        }
    }
}
