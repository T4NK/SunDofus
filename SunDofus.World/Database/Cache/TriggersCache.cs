using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SunDofus.Database.Cache
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

                    trigger.MapID = sqlReader.GetInt16("MapID");
                    trigger.CellID = sqlReader.GetInt16("CellID");
                    trigger.ActionID = sqlReader.GetInt16("ActionID");
                    trigger.Args = sqlReader.GetString("Args");
                    trigger.Conditions = sqlReader.GetString("Conditions");
                    

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

        public static void InsertTrigger(Models.Maps.TriggerModel trigger)
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "INSERT INTO datas_triggers VALUES(@mapid, @cellid, @action, @args, @condi)";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var P = sqlCommand.Parameters;

                P.Add(new MySqlParameter("@mapid", trigger.MapID));
                P.Add(new MySqlParameter("@cellid", trigger.CellID));
                P.Add(new MySqlParameter("@action", trigger.ActionID));
                P.Add(new MySqlParameter("@args", trigger.Args));
                P.Add(new MySqlParameter("@condi", trigger.Conditions));

                sqlCommand.ExecuteNonQuery();
            }
        }

        public static bool ParseTrigger(Database.Models.Maps.TriggerModel trigger)
        {
            if (MapsCache.MapsList.Any(x => x.GetModel.ID == trigger.MapID))
            {
                MapsCache.MapsList.First(x => x.GetModel.ID == trigger.MapID).Triggers.Add(trigger);
                return true;
            }
            else
                return false;
        }
    }
}
