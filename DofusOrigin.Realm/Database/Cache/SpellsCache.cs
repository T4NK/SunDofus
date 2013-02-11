using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Cache
{
    class SpellsCache
    {
        public static List<Database.Models.Spells.SpellModel> SpellsList = new List<Database.Models.Spells.SpellModel>();
        public static List<Database.Models.Spells.SpellToLearnModel> SpellsToLearnList = new List<Database.Models.Spells.SpellToLearnModel>();

        public static void LoadSpells()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_spells";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var spell = new Database.Models.Spells.SpellModel();

                    spell.m_id = sqlReader.GetInt16("id");
                    spell.m_sprite = sqlReader.GetInt16("sprite");
                    spell.m_spriteInfos = sqlReader.GetString("spriteInfos");

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
                    var spell = new Database.Models.Spells.SpellToLearnModel();

                    spell.m_race = sqlReader.GetInt16("Classe");
                    spell.m_level = sqlReader.GetInt16("Level");
                    spell.m_spellID = sqlReader.GetInt16("SpellId");
                    spell.m_pos = sqlReader.GetInt16("Position");

                    lock(SpellsToLearnList)
                        SpellsToLearnList.Add(spell);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' spells to learn@ from the database !", SpellsToLearnList.Count));
        }
    }
}
