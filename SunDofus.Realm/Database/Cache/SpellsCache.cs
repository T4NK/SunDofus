using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Cache
{
    class SpellsCache
    {
        public static List<Database.Models.Spells.SpellModel> SpellsList = new List<Database.Models.Spells.SpellModel>();
        public static List<Database.Models.Spells.SpellToLearnModel> SpellsToLearn = new List<Database.Models.Spells.SpellToLearnModel>();

        public static void LoadSpells()
        {
            lock (DatabaseHandler.myLocker)
            {
                var SQLText = "SELECT * FROM spells";
                var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

                while (SQLReader.Read())
                {
                    var mySpell = new Database.Models.Spells.SpellModel();

                    mySpell.myId = SQLReader.GetInt16("id");
                    mySpell.mySprite = SQLReader.GetInt16("sprite");
                    mySpell.mySpriteInfos = SQLReader.GetString("spriteInfos");

                    for (int i = 1; i <= 6; i++)
                        mySpell.ParseLevel(SQLReader.GetString("lvl" + i));

                    SpellsList.Add(mySpell);
                }

                SQLReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' spells@ from the database !", SpellsList.Count));
        }

        public static void LoadSpellsToLearn()
        {
            lock (DatabaseHandler.myLocker)
            {
                var SQLText = "SELECT * FROM spells_learn";
                var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                MySqlDataReader SQLReader = SQLCommand.ExecuteReader();

                while (SQLReader.Read())
                {
                    var mySpell = new Database.Models.Spells.SpellToLearnModel();

                    mySpell.myRace = SQLReader.GetInt16("Classe");
                    mySpell.myLevel = SQLReader.GetInt16("Level");
                    mySpell.mySpellID = SQLReader.GetInt16("SpellId");
                    mySpell.myPos = SQLReader.GetInt16("Position");

                    SpellsToLearn.Add(mySpell);
                }

                SQLReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' spells to learn@ from the database !", SpellsToLearn.Count));
        }
    }
}
