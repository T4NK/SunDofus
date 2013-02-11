using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Cache
{
    class TriggersCache
    {
        public static List<Database.Models.Maps.TriggerModel> TriggersList = new List<Database.Models.Maps.TriggerModel>();

        public static void LoadTriggers()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_triggers";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var trigger = new Database.Models.Maps.TriggerModel();

                    trigger.m_mapID = sqlReader.GetInt16("MapID");
                    trigger.m_cellID = sqlReader.GetInt16("CellID");
                    trigger.m_actionID = sqlReader.GetInt16("ActionID");
                    trigger.m_args = sqlReader.GetString("Args");
                    trigger.m_conditions = sqlReader.GetString("Conditions");
                    

                    lock (TriggersList)
                    {
                        if(ParseTrigger(trigger))
                            TriggersList.Add(trigger);
                    }
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' triggers@ from the database !", TriggersList.Count));
        }

        public static bool ParseTrigger(Database.Models.Maps.TriggerModel trigger)
        {
            if (MapsCache.MapsList.Any(x => x.GetModel.m_id == trigger.m_mapID))
            {
                MapsCache.MapsList.First(x => x.GetModel.m_id == trigger.m_mapID).Triggers.Add(trigger);
                return true;
            }
            else
                return false;
        }
    }
}
