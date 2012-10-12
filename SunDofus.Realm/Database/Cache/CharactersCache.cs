using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Cache
{
    class CharactersCache
    {
        public static void LoadCharacters()
        {
            lock (DatabaseHandler.myLocker)
            {
                var SQLText = "SELECT * FROM characters";
                var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                MySqlDataReader SQLResult = SQLCommand.ExecuteReader();

                while (SQLResult.Read())
                {
                    var myCharacter = new Realm.Character.Character();

                    myCharacter.ID = SQLResult.GetInt16("id");
                    myCharacter.myName = SQLResult.GetString("name");
                    myCharacter.Level = SQLResult.GetInt16("level");
                    myCharacter.Class = SQLResult.GetInt16("class");
                    myCharacter.Sex = SQLResult.GetInt16("sex");
                    myCharacter.Skin = int.Parse(myCharacter.Class + "" + myCharacter.Sex);
                    myCharacter.Size = 100;
                    myCharacter.Color = SQLResult.GetInt32("color");
                    myCharacter.Color2 = SQLResult.GetInt32("color2");
                    myCharacter.Color3 = SQLResult.GetInt32("color3");

                    myCharacter.MapCell = int.Parse(SQLResult.GetString("mappos").Split(',')[1]);
                    myCharacter.MapID = int.Parse(SQLResult.GetString("mappos").Split(',')[0]);
                    myCharacter.Dir = int.Parse(SQLResult.GetString("mappos").Split(',')[2]);

                    myCharacter.ParseStats(SQLResult.GetString("stats"));

                    if (SQLResult.GetString("items") != "") 
                        myCharacter.myInventary.ParseItems(SQLResult.GetString("items"));

                    if (SQLResult.GetString("spells") != "") 
                        myCharacter.mySpellInventary.ParseSpells(SQLResult.GetString("spells"));

                    myCharacter.NewCharacter = false;

                    Realm.Character.CharactersManager.CharactersList.Add(myCharacter);
                }

                SQLResult.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' characters@ from the database !",Realm.Character.CharactersManager.CharactersList.Count));
        }

        public static void CreateCharacter(Realm.Character.Character myCharacter)
        {
            lock (DatabaseHandler.myLocker)
            {
                var SQLText = "INSERT INTO characters VALUES(@id, @name, @level, @class, @sex, @color, @color2, @color3, @mapinfos, @stats, @items, @spells)";
                var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                MySqlParameterCollection P = SQLCommand.Parameters;

                P.Add(new MySqlParameter("@id", myCharacter.ID));
                P.Add(new MySqlParameter("@name", myCharacter.myName));
                P.Add(new MySqlParameter("@level", myCharacter.Level));
                P.Add(new MySqlParameter("@class", myCharacter.Class));
                P.Add(new MySqlParameter("@sex", myCharacter.Sex));
                P.Add(new MySqlParameter("@color", myCharacter.Color));
                P.Add(new MySqlParameter("@color2", myCharacter.Color2));
                P.Add(new MySqlParameter("@color3", myCharacter.Color3));
                P.Add(new MySqlParameter("@mapinfos", myCharacter.MapID + "," + myCharacter.MapCell + "," + myCharacter.Dir));
                P.Add(new MySqlParameter("@stats", myCharacter.SqlStats()));
                P.Add(new MySqlParameter("@items", ""));
                P.Add(new MySqlParameter("@spells", ""));

                SQLCommand.ExecuteNonQuery();

                myCharacter.NewCharacter = false;
            }
        }

        public static void SaveCharacter(Realm.Character.Character myCharacter)
        {
            lock (DatabaseHandler.myLocker)
            {
                var SQLText = "UPDATE characters SET id=@id, name=@name, level=@level, class=@class, sex=@sex," +
                    " color=@color, color2=@color2, color3=@color3, mappos=@mapinfos, stats=@stats, items=@items, spells=@spells WHERE id=@id";
                var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                MySqlParameterCollection P = SQLCommand.Parameters;
                P.Add(new MySqlParameter("@id", myCharacter.ID));
                P.Add(new MySqlParameter("@name", myCharacter.myName));
                P.Add(new MySqlParameter("@level", myCharacter.Level));
                P.Add(new MySqlParameter("@class", myCharacter.Class));
                P.Add(new MySqlParameter("@sex", myCharacter.Sex));
                P.Add(new MySqlParameter("@color", myCharacter.Color));
                P.Add(new MySqlParameter("@color2", myCharacter.Color2));
                P.Add(new MySqlParameter("@color3", myCharacter.Color3));
                P.Add(new MySqlParameter("@mapinfos", myCharacter.MapID + "," + myCharacter.MapCell + "," + myCharacter.Dir));
                P.Add(new MySqlParameter("@stats", myCharacter.SqlStats()));
                P.Add(new MySqlParameter("@items", myCharacter.GetItemsToSave()));
                P.Add(new MySqlParameter("@spells", myCharacter.mySpellInventary.SaveSpells()));

                SQLCommand.ExecuteNonQuery();
            }
        }

        public static void DeleteCharacter(string Name)
        {
            lock (DatabaseHandler.myLocker)
            {
                var SQLText = "DELETE FROM characters WHERE name=@CharName";
                var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                SQLCommand.Parameters.Add(new MySqlParameter("@CharName", Name));

                SQLCommand.ExecuteNonQuery();
            }
        }

        public static int LastID = -1;

        public static int GetNewID()
        {
            lock (DatabaseHandler.myLocker)
            {
                if (LastID == -1)
                {
                    var SQLText = "SELECT id FROM characters ORDER BY id DESC LIMIT 0,1";
                    var SQLCommand = new MySqlCommand(SQLText, DatabaseHandler.myConnection);

                    MySqlDataReader SQLResult = SQLCommand.ExecuteReader();

                    LastID = 0;

                    if (SQLResult.Read())
                    {
                        LastID = SQLResult.GetInt32("id");
                    }

                    SQLResult.Close();

                    return ++LastID;
                }
                else
                {
                    return ++LastID;
                }
            }
        }
    }
}
