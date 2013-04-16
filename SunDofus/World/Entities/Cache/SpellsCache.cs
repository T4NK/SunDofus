using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SunDofus.World.Entities.Cache
{
    class SpellsCache
    {
        public static List<Entities.Models.Spells.SpellModel> SpellsList = new List<Entities.Models.Spells.SpellModel>();
        public static List<Entities.Models.Spells.SpellToLearnModel> SpellsToLearnList = new List<Entities.Models.Spells.SpellToLearnModel>();

        public static void LoadSpells()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_spells";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var spell = new Entities.Models.Spells.SpellModel();

                    spell.ID = sqlReader.GetInt16("id");
                    spell.Sprite = sqlReader.GetInt16("sprite");
                    spell.SpriteInfos = sqlReader.GetString("spriteInfos");

                    for (int i = 1; i <= 6; i++)
                        spell.ParseLevel(sqlReader.GetString("lvl" + i));

                    lock(SpellsList)
                        SpellsList.Add(spell);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' spells@ from the database !", SpellsList.Count));
        }

        public static void LoadSpellsToLearn()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_spells_learn";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var spell = new Entities.Models.Spells.SpellToLearnModel();

                    spell.Race = sqlReader.GetInt16("Classe");
                    spell.Level = sqlReader.GetInt16("Level");
                    spell.SpellID = sqlReader.GetInt16("SpellId");
                    spell.Pos = sqlReader.GetInt16("Position");

                    lock(SpellsToLearnList)
                        SpellsToLearnList.Add(spell);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' spells to learn@ from the database !", SpellsToLearnList.Count));
        }
    }
}
