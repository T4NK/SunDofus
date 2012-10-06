using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Data
{
    class SpellSql
    {
        public static List<Realm.Character.Spells.AbstractSpell> SpellsList = new List<Realm.Character.Spells.AbstractSpell>();
        public static List<Realm.Character.Spells.SpellToLearn> SpellsToLearn = new List<Realm.Character.Spells.SpellToLearn>();

        public static void LoadSpells()
        {
            string SQLText = "SELECT * FROM spells";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, SQLManager.m_Connection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Realm.Character.Spells.AbstractSpell m_S = new Realm.Character.Spells.AbstractSpell();

                m_S.id = SQLReader.GetInt16("id");
                m_S.sprite = SQLReader.GetInt16("sprite");
                m_S.spriteInfos = SQLReader.GetString("spriteInfos");

                for (int i = 1; i <= 6; i++)
                    m_S.ParseLevel(SQLReader.GetString("lvl" + i));

                SpellsList.Add(m_S);
            }

            SQLReader.Close();

            SunDofus.Logger.Status("Loaded '" + SpellsList.Count + "' spells from the database !");
        }

        public static void LoadSpellsToLearn()
        {
            string SQLText = "SELECT * FROM spells_learn";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, SQLManager.m_Connection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Realm.Character.Spells.SpellToLearn m_S = new Realm.Character.Spells.SpellToLearn();
                
                m_S.Race = SQLReader.GetInt16("Classe");
                m_S.Level = SQLReader.GetInt16("Level");
                m_S.SpellID = SQLReader.GetInt16("SpellId");
                m_S.Pos = SQLReader.GetInt16("Position");

                SpellsToLearn.Add(m_S);
            }

            SQLReader.Close();

            SunDofus.Logger.Status("Loaded '" + SpellsToLearn.Count + "' spells to learn from the database !");
        }
    }
}
