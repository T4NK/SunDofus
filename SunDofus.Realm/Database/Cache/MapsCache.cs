using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Cache
{
    class MapsCache
    {
        public static List<Realm.Map.Map> MapsList = new List<Realm.Map.Map>();

        public static void LoadMaps()
        {
            lock (DatabaseHandler.myLocker)
            {
                var SQLText = "SELECT * FROM maps";
                var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

                while (SQLReader.Read())
                {
                    var myMap = new Models.Maps.MapModel();

                    myMap.myId = SQLReader.GetInt16("id");
                    myMap.myDate = SQLReader.GetInt32("date");
                    myMap.myWidth = SQLReader.GetInt16("width");
                    myMap.myHeight = SQLReader.GetInt16("heigth");
                    myMap.myCapabilities = SQLReader.GetInt16("capabilities");
                    myMap.myNumgroup = SQLReader.GetInt16("numgroup");
                    myMap.myGroupmaxsize = SQLReader.GetInt16("groupmaxsize");
                    myMap.myMappos = SQLReader.GetString("mappos");
                    myMap.myMonsters = SQLReader.GetString("monsters");
                    myMap.myCells = SQLReader.GetString("cells");
                    myMap.myMapData = SQLReader.GetString("mapData");
                    myMap.myKey = SQLReader.GetString("key");

                    MapsList.Add(new Realm.Map.Map(myMap));
                }

                SQLReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' maps@ from the database !", MapsList.Count));
        }
    }
}
