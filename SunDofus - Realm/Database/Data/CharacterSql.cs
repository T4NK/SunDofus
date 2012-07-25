using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace realm.Database.Data
{
    class CharacterSql
    {
        public static void LoadCharacters()
        {
            string SQLText = "SELECT * FROM characters";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, SQLManager.m_Connection);

            MySqlDataReader SQLResult = SQLCommand.ExecuteReader();

            while (SQLResult.Read())
            {
                Realm.Character.Character c = new Realm.Character.Character();
                c.ID = SQLResult.GetInt16("id");
                c.Name = SQLResult.GetString("name");
                c.Level = SQLResult.GetInt16("level");
                c.Class = SQLResult.GetInt16("class");
                c.Sex = SQLResult.GetInt16("sex");
                c.Skin = int.Parse(c.Class + "" + c.Sex);
                c.Size = 100;
                c.Color = SQLResult.GetInt32("color");
                c.Color2 = SQLResult.GetInt32("color2");
                c.Color3 = SQLResult.GetInt32("color3");
                c.NewCharacter = false;

                Realm.Character.CharactersManager.ListOfCharacters.Add(c);
            }

            SQLResult.Close();

            Utils.Logger.Status("Loaded '" + Realm.Character.CharactersManager.ListOfCharacters.Count + "' characters from the database !");
        }

        public static void CreateCharacter(Realm.Character.Character m_C)
        {
            string SQLText = "INSERT INTO characters VALUES(@id, @name, @level, @class, @sex, @color, @color2, @color3)";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, SQLManager.m_Connection);

            MySqlParameterCollection P = SQLCommand.Parameters;
            P.Add(new MySqlParameter("@id", m_C.ID));
            P.Add(new MySqlParameter("@name", m_C.Name));
            P.Add(new MySqlParameter("@level", m_C.Level));
            P.Add(new MySqlParameter("@class", m_C.Class));
            P.Add(new MySqlParameter("@sex", m_C.Sex));
            P.Add(new MySqlParameter("@color", m_C.Color));
            P.Add(new MySqlParameter("@color2", m_C.Color2));
            P.Add(new MySqlParameter("@color3", m_C.Color3));

            SQLCommand.ExecuteNonQuery();

            m_C.NewCharacter = false;
        }

        public static void SaveCharacter(Realm.Character.Character m_C)
        {

        }

        public static void DeleteCharacter(string Name)
        {
            string SQLText = "DELETE FROM characters WHERE name=@CharName";
            MySqlCommand SQLCommand = new MySqlCommand(SQLText, SQLManager.m_Connection);
            SQLCommand.Parameters.Add(new MySqlParameter("@CharName", Name));

            SQLCommand.ExecuteNonQuery();
        }

        public static int LastID = -1;

        public static int GetNewID()
        {
            if (LastID == -1)
            {
                string SQLText = "SELECT id FROM characters ORDER BY id DESC LIMIT 0,1";
                MySqlCommand SQLCommand = new MySqlCommand(SQLText, SQLManager.m_Connection);

                MySqlDataReader SQLResult = SQLCommand.ExecuteReader();

                LastID = 0;

                if (SQLResult.Read())
                {
                    LastID = SQLResult.GetInt32("id");
                }

                SQLResult.Close();

                LastID += 1;
                return LastID;
            }
            else
            {
                LastID += 1;
                return LastID;
            }
        }
    }
}
