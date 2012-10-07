using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Cache
{
    class TriggersCache
    {
        public static List<Database.Models.Maps.TriggerModel> TriggersList = new List<Database.Models.Maps.TriggerModel>();

        public static void LoadTriggers()
        {
            string SQLText = "SELECT * FROM triggers";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Database.Models.Maps.TriggerModel myT = new Database.Models.Maps.TriggerModel();

                myT.MapID = SQLReader.GetInt16("MapID");
                myT.CellID = SQLReader.GetInt16("CellID");
                myT.NewMapID = int.Parse(SQLReader.GetString("NewMap").Split(',')[0]);
                myT.NewCellID = int.Parse(SQLReader.GetString("NewMap").Split(',')[1]);

                TriggersList.Add(myT);
                ParseTrigger(myT);
            }

            SQLReader.Close();

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' triggers@ from the database !", TriggersList.Count));
        }

        public static void ParseTrigger(Database.Models.Maps.TriggerModel myT)
        {
            foreach (Realm.Map.Map myM in MapsCache.MapsList)
            {
                if (myM.myMap.id == myT.MapID) myM.myTriggers.Add(myT);
            }
        }
    }
}
