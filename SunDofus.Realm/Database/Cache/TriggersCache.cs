using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Cache
{
    class TriggersCache
    {
        public static List<Database.Models.Maps.TriggerModel> m_triggersList = new List<Database.Models.Maps.TriggerModel>();

        public static void LoadTriggers()
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "SELECT * FROM triggers";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var trigger = new Database.Models.Maps.TriggerModel();

                    trigger.m_mapID = sqlReader.GetInt16("MapID");
                    trigger.m_cellID = sqlReader.GetInt16("CellID");
                    trigger.m_actionID = sqlReader.GetInt16("ActionID");
                    trigger.m_args = sqlReader.GetString("Args");
                    trigger.m_conditions = sqlReader.GetString("Conditions");

                    trigger.ParseConditions();

                    m_triggersList.Add(trigger);
                    ParseTrigger(trigger);

                    Utilities.Loggers.m_infosLogger.Write(string.Format("Loaded trigger @{0}:{1}@ !", trigger.m_mapID, trigger.m_cellID));
                }

                sqlReader.Close();
            }

            Utilities.Loggers.m_statusLogger.Write(string.Format("Loaded @'{0}' triggers@ from the database !", m_triggersList.Count));
        }

        public static void ParseTrigger(Database.Models.Maps.TriggerModel _trigger)
        {
            if (MapsCache.m_mapsList.Any(x => x.m_map.m_id == _trigger.m_mapID))
                MapsCache.m_mapsList.First(x => x.m_map.m_id == _trigger.m_mapID).m_triggers.Add(_trigger);
        }
    }
}
