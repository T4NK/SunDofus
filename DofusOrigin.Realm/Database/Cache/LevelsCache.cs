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

                    level.ID = sqlReader.GetInt16("Level");
                    level.Character = sqlReader.GetInt64("Character");
                    level.Job = sqlReader.GetInt64("Job");
                    level.Mount = sqlReader.GetInt64("Mount");
                    level.Alignment = sqlReader.GetInt64("Pvp");
                    level.Guild = sqlReader.GetInt64("Guild");

                    lock(LevelsList)
                        LevelsList.Add(level);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' levels@ from the database !", LevelsList.Count));
        }

        public static Models.Levels.LevelModel ReturnLevel(int _level)
        {
            if (LevelsList.Any(x => x.ID == _level))
                return LevelsList.First(x => x.ID == _level);
            else
                return new Models.Levels.LevelModel(long.MaxValue);
        }

        public static int MaxLevel()
        {
            return LevelsList.First(x => x.ID > 0 && LevelsList.Any(y => y.ID > x.ID) == false).ID - 1;
        }
    }
}
