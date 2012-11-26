using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Cache
{
    class SpellsCache
    {
        public static List<Database.Models.Spells.SpellModel> m_spellsList = new List<Database.Models.Spells.SpellModel>();
        public static List<Database.Models.Spells.SpellToLearnModel> m_spellsToLearn = new List<Database.Models.Spells.SpellToLearnModel>();

        public static void LoadSpells()
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "SELECT * FROM spells";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var spell = new Database.Models.Spells.SpellModel();

                    spell.m_id = sqlReader.GetInt16("id");
                    spell.m_sprite = sqlReader.GetInt16("sprite");
                    spell.m_spriteInfos = sqlReader.GetString("spriteInfos");

                    for (int i = 1; i <= 6; i++)
                        spell.ParseLevel(sqlReader.GetString("lvl" + i));

                    m_spellsList.Add(spell);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.m_statusLogger.Write(string.Format("Loaded @'{0}' spells@ from the database !", m_spellsList.Count));
        }

        public static void LoadSpellsToLearn()
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "SELECT * FROM spells_learn";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var spell = new Database.Models.Spells.SpellToLearnModel();

                    spell.m_race = sqlReader.GetInt16("Classe");
                    spell.m_level = sqlReader.GetInt16("Level");
                    spell.m_spellID = sqlReader.GetInt16("SpellId");
                    spell.m_pos = sqlReader.GetInt16("Position");

                    m_spellsToLearn.Add(spell);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.m_statusLogger.Write(string.Format("Loaded @'{0}' spells to learn@ from the database !", m_spellsToLearn.Count));
        }
    }
}
