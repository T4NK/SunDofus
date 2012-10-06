using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.World
{
    class Save
    {
        public static void ParseSave(string Data)
        {
            switch (Data)
            {
                case "all":
                    SaveWorld();
                    break;

                case "char":
                    SaveChar();
                    break;
            }
        }
        
        public static void SaveWorld()
        {

        }

        public static void SaveChar()
        {
            Program.m_RealmLink.Send("StartM");
            foreach (Character.Character m_C in Character.CharactersManager.CharactersList)
            {
                Database.Data.CharacterSql.SaveCharacter(m_C);
                System.Threading.Thread.Sleep(100);
            }
            Program.m_RealmLink.Send("StopM");
        }
    }
}
