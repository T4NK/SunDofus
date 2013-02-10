using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Cache
{
    class CharactersCache
    {
        public static void LoadCharacters()
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "SELECT * FROM dyn_characters";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                MySqlDataReader sqlResult = sqlCommand.ExecuteReader();

                while (sqlResult.Read())
                {
                    var character = new Realm.Characters.Character();

                    character.m_id = sqlResult.GetInt16("id");
                    character.m_name = sqlResult.GetString("name");
                    character.m_level = sqlResult.GetInt16("level");
                    character.m_class = sqlResult.GetInt16("class");
                    character.m_sex = sqlResult.GetInt16("sex");
                    character.m_skin = int.Parse(character.m_class + "" + character.m_sex);
                    character.m_size = 100;
                    character.m_color = sqlResult.GetInt32("color");
                    character.m_color2 = sqlResult.GetInt32("color2");
                    character.m_color3 = sqlResult.GetInt32("color3");

                    character.m_mapCell = int.Parse(sqlResult.GetString("mappos").Split(',')[1]);
                    character.m_mapID = int.Parse(sqlResult.GetString("mappos").Split(',')[0]);
                    character.m_dir = int.Parse(sqlResult.GetString("mappos").Split(',')[2]);

                    character.ParseStats(sqlResult.GetString("stats"));

                    if (sqlResult.GetString("items") != "") 
                        character.m_inventary.ParseItems(sqlResult.GetString("items"));

                    if (sqlResult.GetString("spells") != "") 
                        character.m_spellInventary.ParseSpells(sqlResult.GetString("spells"));

                    character.isNewCharacter = false;

                    Realm.Characters.CharactersManager.CharactersList.Add(character);
                }

                sqlResult.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' characters@ from the database !",Realm.Characters.CharactersManager.CharactersList.Count));
        }

        public static void CreateCharacter(Realm.Characters.Character _character)
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "INSERT INTO dyn_characters VALUES(@id, @name, @level, @class, @sex, @color, @color2, @color3, @mapinfos, @stats, @items, @spells)";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                MySqlParameterCollection P = sqlCommand.Parameters;

                P.Add(new MySqlParameter("@id", _character.m_id));
                P.Add(new MySqlParameter("@name", _character.m_name));
                P.Add(new MySqlParameter("@level", _character.m_level));
                P.Add(new MySqlParameter("@class", _character.m_class));
                P.Add(new MySqlParameter("@sex", _character.m_sex));
                P.Add(new MySqlParameter("@color", _character.m_color));
                P.Add(new MySqlParameter("@color2", _character.m_color2));
                P.Add(new MySqlParameter("@color3", _character.m_color3));
                P.Add(new MySqlParameter("@mapinfos", _character.m_mapID + "," + _character.m_mapCell + "," + _character.m_dir));
                P.Add(new MySqlParameter("@stats", _character.SqlStats()));
                P.Add(new MySqlParameter("@items", ""));
                P.Add(new MySqlParameter("@spells", ""));

                sqlCommand.ExecuteNonQuery();

                _character.isNewCharacter = false;
            }
        }

        public static void SaveCharacter(Realm.Characters.Character _character)
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "UPDATE dyn_characters SET id=@id, name=@name, level=@level, class=@class, sex=@sex," +
                    " color=@color, color2=@color2, color3=@color3, mappos=@mapinfos, stats=@stats, items=@items, spells=@spells WHERE id=@id";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                MySqlParameterCollection P = sqlCommand.Parameters;
                P.Add(new MySqlParameter("@id", _character.m_id));
                P.Add(new MySqlParameter("@name", _character.m_name));
                P.Add(new MySqlParameter("@level", _character.m_level));
                P.Add(new MySqlParameter("@class", _character.m_class));
                P.Add(new MySqlParameter("@sex", _character.m_sex));
                P.Add(new MySqlParameter("@color", _character.m_color));
                P.Add(new MySqlParameter("@color2", _character.m_color2));
                P.Add(new MySqlParameter("@color3", _character.m_color3));
                P.Add(new MySqlParameter("@mapinfos", _character.m_mapID + "," + _character.m_mapCell + "," + _character.m_dir));
                P.Add(new MySqlParameter("@stats", _character.SqlStats()));
                P.Add(new MySqlParameter("@items", _character.GetItemsToSave()));
                P.Add(new MySqlParameter("@spells", _character.m_spellInventary.SaveSpells()));

                sqlCommand.ExecuteNonQuery();
            }
        }

        public static void DeleteCharacter(string Name)
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "DELETE FROM dyn_characters WHERE name=@CharName";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                sqlCommand.Parameters.Add(new MySqlParameter("@CharName", Name));

                sqlCommand.ExecuteNonQuery();
            }
        }

        public static int m_lastID = -1;

        public static int GetNewID()
        {
            lock (DatabaseHandler.m_locker)
            {
                if (m_lastID == -1)
                {
                    var sqlText = "SELECT id FROM dyn_characters ORDER BY id DESC LIMIT 0,1";
                    var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                    MySqlDataReader sqlResult = sqlCommand.ExecuteReader();

                    m_lastID = 0;

                    if (sqlResult.Read())
                    {
                        m_lastID = sqlResult.GetInt32("id");
                    }

                    sqlResult.Close();

                    return ++m_lastID;
                }
                else
                {
                    return ++m_lastID;
                }
            }
        }
    }
}
