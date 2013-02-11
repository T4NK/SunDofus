using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Cache
{
    class LevelsCache
    {
        public static List<Models.Levels.LevelModel> LevelsList = new List<Models.Levels.LevelModel>();

        public static void LoadLevels()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_levels";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var level = new Models.Levels.LevelModel();

                    level.m_id = sqlReader.GetInt16("Level");
                    level.m_character = sqlReader.GetInt64("Character");
                    level.m_job = sqlReader.GetInt64("Job");
                    level.m_mount = sqlReader.GetInt64("Mount");
                    level.m_alignment = sqlReader.GetInt64("Pvp");
                    level.m_guild = sqlReader.GetInt64("Guild");

                    lock(LevelsList)
                        LevelsList.Add(level);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' levels@ from the database !", LevelsList.Count));
        }

        public static Models.Levels.LevelModel ReturnLevel(int _level)
        {
            if (LevelsList.Any(x => x.m_id == _level))
                return LevelsList.First(x => x.m_id == _level);
            else
                return new Models.Levels.LevelModel(long.MaxValue);
        }

        public static int MaxLevel()
        {
            return LevelsList.First(x => x.m_id > 0 && LevelsList.Any(y => y.m_id > x.m_id) == false).m_id - 1;
        }
    }
}
