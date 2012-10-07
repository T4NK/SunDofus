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
            string SQLText = "SELECT * FROM maps";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Models.Maps.MapModel myM = new Models.Maps.MapModel();

                myM.id = SQLReader.GetInt16("id");
                myM.date = SQLReader.GetInt32("date");
                myM.width = SQLReader.GetInt16("width");
                myM.height = SQLReader.GetInt16("heigth");
                myM.capabilities = SQLReader.GetInt16("capabilities");
                myM.numgroup = SQLReader.GetInt16("numgroup");
                myM.groupmaxsize = SQLReader.GetInt16("groupmaxsize");
                myM.mappos = SQLReader.GetString("mappos");
                myM.monsters = SQLReader.GetString("monsters");
                myM.cells = SQLReader.GetString("cells");
                myM.MapData = SQLReader.GetString("mapData");
                myM.key = SQLReader.GetString("key");

                MapsList.Add(new Realm.Map.Map(myM));
            }

            SQLReader.Close();

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' maps@ from the database !", MapsList.Count));
        }
    }
}
