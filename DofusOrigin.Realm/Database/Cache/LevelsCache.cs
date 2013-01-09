using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Cache
{
    class LevelsCache
    {
        public static List<Models.Levels.LevelModel> m_levelsList = new List<Models.Levels.LevelModel>();

        public static void LoadLevels()
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "SELECT * FROM datas_levels";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var level = new Models.Levels.LevelModel();

                    level.m_id = sqlReader.GetInt16("Level");
                    level.m_character = sqlReader.GetInt64("Character");
                    level.m_job = sqlReader.GetInt64("Job");
                    level.m_mount = sqlReader.GetInt64("Mount");
                    level.m_alignment = sqlReader.GetInt64("Pvp");
                    level.m_guild = sqlReader.GetInt64("Guild");

                    m_levelsList.Add(level);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.m_statusLogger.Write(string.Format("Loaded @'{0}' levels@ from the database !", m_levelsList.Count));
        }

        public static Models.Levels.LevelModel ReturnLevel(int _level)
        {
            if (m_levelsList.Any(x => x.m_id == _level))
                return m_levelsList.First(x => x.m_id == _level);
            else
                return new Models.Levels.LevelModel(long.MaxValue);
        }

        public static int MaxLevel()
        {
            return m_levelsList.First(x => x.m_id > 0 && m_levelsList.Any(y => y.m_id > x.m_id) == false).m_id - 1;
        }
    }
}
