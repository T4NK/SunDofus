using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Cache
{
    class SpellsCache
    {
        public static List<Realm.Character.Spells.SpellModel> SpellsList = new List<Realm.Character.Spells.SpellModel>();
        public static List<Realm.Character.Spells.SpellToLearn> SpellsToLearn = new List<Realm.Character.Spells.SpellToLearn>();

        public static void LoadSpells()
        {
            string SQLText = "SELECT * FROM spells";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Realm.Character.Spells.SpellModel myS = new Realm.Character.Spells.SpellModel();

                myS.id = SQLReader.GetInt16("id");
                myS.sprite = SQLReader.GetInt16("sprite");
                myS.spriteInfos = SQLReader.GetString("spriteInfos");

                for (int i = 1; i <= 6; i++)
                    myS.ParseLevel(SQLReader.GetString("lvl" + i));

                SpellsList.Add(myS);
            }

            SQLReader.Close();

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded '{0}' spells from the database !", SpellsList.Count));
        }

        public static void LoadSpellsToLearn()
        {
            string SQLText = "SELECT * FROM spells_learn";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

            MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

            while (SQLReader.Read())
            {
                Realm.Character.Spells.SpellToLearn myS = new Realm.Character.Spells.SpellToLearn();
                
                myS.Race = SQLReader.GetInt16("Classe");
                myS.Level = SQLReader.GetInt16("Level");
                myS.SpellID = SQLReader.GetInt16("SpellId");
                myS.Pos = SQLReader.GetInt16("Position");

                SpellsToLearn.Add(myS);
            }

            SQLReader.Close();

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded '{0}' spells to learn from the database !", SpellsToLearn.Count));
        }
    }
}
