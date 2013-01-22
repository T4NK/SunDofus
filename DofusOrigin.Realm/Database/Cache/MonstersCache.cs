using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Cache
{
    class MonstersCache
    {
        public static List<Models.Monsters.MonsterModel> m_monsters = new List<Models.Monsters.MonsterModel>();

        public static void LoadMonsters()
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "SELECT * FROM datas_creatures";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);
                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var newMonsters = new Models.Monsters.MonsterModel();
                    {
                        newMonsters.m_id = sqlReader.GetInt32("ID");
                        newMonsters.m_name = sqlReader.GetString("Name");
                        newMonsters.m_gfx = sqlReader.GetInt32("GfxId");
                        newMonsters.m_align = sqlReader.GetInt32("Alignement");

                        newMonsters.m_color = DofusOrigin.Utilities.Basic.HexToDeci(sqlReader.GetString("Colors").Split(',')[0]);
                        newMonsters.m_color2 = DofusOrigin.Utilities.Basic.HexToDeci(sqlReader.GetString("Colors").Split(',')[1]);
                        newMonsters.m_color3 = DofusOrigin.Utilities.Basic.HexToDeci(sqlReader.GetString("Colors").Split(',')[2]);

                        newMonsters.m_ia = sqlReader.GetInt16("AI_Type");

                        if (sqlReader.GetString("Kamas_Dropped") == "" || sqlReader.GetString("Kamas_Dropped").Split(';').Length <= 1)
                        {
                            newMonsters.min_kamas = 0;
                            newMonsters.min_kamas = 0;
                        }
                        else
                        {
                            newMonsters.min_kamas = int.Parse(sqlReader.GetString("Kamas_Dropped").Split(';')[0]);
                            newMonsters.max_kamas = int.Parse(sqlReader.GetString("Kamas_Dropped").Split(';')[1]);
                        }

                        foreach (var newItem in sqlReader.GetString("Items_Dropped").Split('|'))
                        {
                            if (newItem == "")
                                continue;

                            var infos = newItem.Split(';');

                            newMonsters.m_items.Add(new Models.Monsters.MonsterModel.MonsterItem(int.Parse(infos[0]),
                                double.Parse(infos[1]), int.Parse(infos[2])));
                        }
                    }

                    m_monsters.Add(newMonsters);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.m_statusLogger.Write(string.Format("Loaded @'{0}' monsters@ from the database !", m_monsters.Count));
        }

        public static void LoadMonstersLevels()
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "SELECT * FROM datas_creatures_levels";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);
                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var newLevel = new Models.Monsters.MonsterLevelModel();
                    {
                        newLevel.m_id = sqlReader.GetInt32("Id");
                        newLevel.m_creature = sqlReader.GetInt32("Mob_Id");
                        newLevel.m_grade = sqlReader.GetInt16("Grade");

                        newLevel.m_level = sqlReader.GetInt32("Level");
                        newLevel.m_exp = sqlReader.GetInt32("Experience");
                        newLevel.m_pa = sqlReader.GetInt16("Pa");
                        newLevel.m_pm = sqlReader.GetInt16("Pm");
                        newLevel.m_life = sqlReader.GetInt32("Life");

                        newLevel.m_rNeutral = sqlReader.GetInt32("rNeutral");
                        newLevel.m_rStrenght = sqlReader.GetInt32("rEarth");
                        newLevel.m_rIntel = sqlReader.GetInt32("rFire");
                        newLevel.m_rLuck = sqlReader.GetInt32("rWater");
                        newLevel.m_rAgility = sqlReader.GetInt32("rAir");

                        newLevel.m_rPa = sqlReader.GetInt32("rPA");
                        newLevel.m_rPm = sqlReader.GetInt32("rPM");

                        newLevel.m_wisdom = sqlReader.GetInt32("Sagesse");
                        newLevel.m_strenght = sqlReader.GetInt32("Force");
                        newLevel.m_intel = sqlReader.GetInt32("Intelligence");
                        newLevel.m_luck = sqlReader.GetInt32("Chance");
                        newLevel.m_agility = sqlReader.GetInt32("Agilite");

                        foreach (var newSpell in sqlReader.GetString("Spells").Split(';'))
                        {
                            if (newSpell == "")
                                continue;

                            var infos = newSpell.Split('@');

                            newLevel.m_spells.Add(new Realm.Characters.Spells.CharacterSpell
                                (int.Parse(infos[0]), int.Parse(infos[1]), -1));
                        }
                    }

                    if (m_monsters.Any(x => x.m_id == newLevel.m_creature))
                        m_monsters.First(x => x.m_id == newLevel.m_creature).m_levels.Add(newLevel);
                }

                sqlReader.Close();
            }
        }
    }
}
