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
            lock (DatabaseHandler.myLocker)
            {
                var SQLText = "SELECT * FROM triggers";
                var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

                while (SQLReader.Read())
                {
                    var myTrigger = new Database.Models.Maps.TriggerModel();

                    myTrigger.myMapID = SQLReader.GetInt16("MapID");
                    myTrigger.myCellID = SQLReader.GetInt16("CellID");
                    myTrigger.myActionID = SQLReader.GetInt16("ActionID");
                    myTrigger.myArgs = SQLReader.GetString("Args");
                    myTrigger.myConditions = SQLReader.GetString("Conditions");

                    myTrigger.ParseConditions();

                    TriggersList.Add(myTrigger);
                    ParseTrigger(myTrigger);
                }

                SQLReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' triggers@ from the database !", TriggersList.Count));
        }

        public static void ParseTrigger(Database.Models.Maps.TriggerModel myTrigger)
        {
            foreach (var myM in MapsCache.MapsList)
            {
                if (myM.myMap.myId == myTrigger.myMapID) myM.myTriggers.Add(myTrigger);
            }
        }
    }
}
