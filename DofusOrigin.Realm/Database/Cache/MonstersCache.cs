using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Cache
{
    class MonstersCache
    {
        public static List<Models.Monsters.MonsterModel> MonstersList = new List<Models.Monsters.MonsterModel>();

        public static void LoadMonsters()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_creatures";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var newMonsters = new Models.Monsters.MonsterModel();
                    {
                        newMonsters.ID = sqlReader.GetInt32("ID");
                        newMonsters.Name = sqlReader.GetString("Name");
                        newMonsters.GfxID = sqlReader.GetInt32("GfxId");
                        newMonsters.Align = sqlReader.GetInt32("Alignement");

                        newMonsters.Color = DofusOrigin.Utilities.Basic.HexToDeci(sqlReader.GetString("Colors").Split(',')[0]);
                        newMonsters.Color2 = DofusOrigin.Utilities.Basic.HexToDeci(sqlReader.GetString("Colors").Split(',')[1]);
                        newMonsters.Color3 = DofusOrigin.Utilities.Basic.HexToDeci(sqlReader.GetString("Colors").Split(',')[2]);

                        newMonsters.IA = sqlReader.GetInt16("AI_Type");

                        if (sqlReader.GetString("Kamas_Dropped") == "" || sqlReader.GetString("Kamas_Dropped").Split(';').Length <= 1)
                        {
                            newMonsters.Min_kamas = 0;
                            newMonsters.Min_kamas = 0;
                        }
                        else
                        {
                            newMonsters.Min_kamas = int.Parse(sqlReader.GetString("Kamas_Dropped").Split(';')[0]);
                            newMonsters.Max_kamas = int.Parse(sqlReader.GetString("Kamas_Dropped").Split(';')[1]);
                        }

                        foreach (var newItem in sqlReader.GetString("Items_Dropped").Split('|'))
                        {
                            if (newItem == "")
                                continue;

                            var infos = newItem.Split(';');

                            if (infos.Length < 2)
                                continue;

                            lock (newMonsters.Items)
                            {
                                newMonsters.Items.Add(new Models.Monsters.MonsterModel.MonsterItem(int.Parse(infos[0]),
                                    double.Parse(infos[1]), int.Parse(infos[2])));
                            }
                        }
                    }

                    lock(MonstersList)
                        MonstersList.Add(newMonsters);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' monsters@ from the database !", MonstersList.Count));
        }

        public static void LoadMonstersLevels()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_creatures_levels";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var newLevel = new Models.Monsters.MonsterLevelModel();
                    {
                        newLevel.ID = sqlReader.GetInt32("Id");
                        newLevel.CreatureID = sqlReader.GetInt32("Mob_Id");
                        newLevel.GradeID = sqlReader.GetInt16("Grade");

                        newLevel.Level = sqlReader.GetInt32("Level");
                        newLevel.Exp = sqlReader.GetInt32("Experience");
                        newLevel.AP = sqlReader.GetInt16("Pa");
                        newLevel.MP = sqlReader.GetInt16("Pm");
                        newLevel.Life = sqlReader.GetInt32("Life");

                        newLevel.RNeutral = sqlReader.GetInt32("rNeutral");
                        newLevel.RStrenght = sqlReader.GetInt32("rEarth");
                        newLevel.RIntel = sqlReader.GetInt32("rFire");
                        newLevel.RLuck = sqlReader.GetInt32("rWater");
                        newLevel.RAgility = sqlReader.GetInt32("rAir");

                        newLevel.RPa = sqlReader.GetInt32("rPA");
                        newLevel.RPm = sqlReader.GetInt32("rPM");

                        newLevel.Wisdom = sqlReader.GetInt32("Sagesse");
                        newLevel.Strenght = sqlReader.GetInt32("Force");
                        newLevel.Intel = sqlReader.GetInt32("Intelligence");
                        newLevel.Luck = sqlReader.GetInt32("Chance");
                        newLevel.Agility = sqlReader.GetInt32("Agilite");

                        foreach (var newSpell in sqlReader.GetString("Spells").Split(';'))
                        {
                            if (newSpell == "")
                                continue;

                            var infos = newSpell.Split('@');

                            lock (newLevel.Spells)
                            {
                                newLevel.Spells.Add(new Realm.Characters.Spells.CharacterSpell
                                    (int.Parse(infos[0]), int.Parse(infos[1]), -1));
                            }
                        }
                    }

                    if (MonstersList.Any(x => x.ID == newLevel.CreatureID))
                    {
                        var monster = MonstersList.First(x => x.ID == newLevel.CreatureID);

                        lock(monster.Levels)           
                            monster.Levels.Add(newLevel);
                    }
                }

                sqlReader.Close();
            }
        }
    }
}
