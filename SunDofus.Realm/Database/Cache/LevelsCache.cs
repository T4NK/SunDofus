using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Cache
{
    class LevelsCache
    {
        public static List<Models.Levels.LevelModel> LevelsList = new List<Models.Levels.LevelModel>();

        public static void LoadLevels()
        {
            lock (DatabaseHandler.myLocker)
            {
                var SQLText = "SELECT * FROM levels";
                var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

                while (SQLReader.Read())
                {
                    var newLevel = new Models.Levels.LevelModel();

                    newLevel.Id = SQLReader.GetInt16("Level");
                    newLevel.Character = SQLReader.GetInt64("Character");
                    newLevel.Job = SQLReader.GetInt64("Job");
                    newLevel.Mount = SQLReader.GetInt64("Mount");
                    newLevel.Alignment = SQLReader.GetInt64("Pvp");
                    newLevel.Guild = SQLReader.GetInt64("Guild");

                    LevelsList.Add(newLevel);
                }

                SQLReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' levels@ from the database !", LevelsList.Count));
        }

        public static Models.Levels.LevelModel ReturnLevel(int Level)
        {
            if (LevelsList.Any(x => x.Id == Level))
                return LevelsList.First(x => x.Id == Level);
            else
                return new Models.Levels.LevelModel(long.MaxValue);
        }

        public static int MaxLevel()
        {
            return LevelsList.First(x => x.Id > 0 && LevelsList.Any(y => y.Id > x.Id) == false).Id - 1;
        }
    }
}
