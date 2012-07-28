using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus;

namespace realm.Realm.Character
{
    class Character
    {
        public string Name = "";
        public int ID, Color, Color2, Color3, Class, Sex, Skin, Size, Level, MapID, MapCell, Dir = -1;
        public bool NewCharacter, isConnected = false;

        public Client.RealmClient Client;
        public CharacterState State;

        public string Channel = "*#$p%i:?!";

        #region Pattern

        public string PatternList()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(ID + ";");
            Builder.Append(Name + ";");
            Builder.Append(Level + ";"); // Level
            Builder.Append(Skin + ";");
            Builder.Append(Basic.DeciToHex(Color) + ";");
            Builder.Append(Basic.DeciToHex(Color2) + ";");
            Builder.Append(Basic.DeciToHex(Color3) + ";");
            Builder.Append(",,,,,;"); // Items
            Builder.Append("0;" + Program.m_ServerID + ";;;");

            return Builder.ToString();
        }

        public string PatternSelect()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append("|" + ID + "|");
            Builder.Append(Name + "|");
            Builder.Append(Level + "|"); // Level
            Builder.Append(Class + "|");
            Builder.Append(Skin + "|");
            Builder.Append(Basic.DeciToHex(Color) + "|");
            Builder.Append(Basic.DeciToHex(Color2) + "|");
            Builder.Append(Basic.DeciToHex(Color3) + "|");
            Builder.Append(""); // Items

            return Builder.ToString();
        }

        public string PatternDisplayChar()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(MapCell + ";");
            Builder.Append(Dir + ";0;");
            Builder.Append(ID + ";");
            Builder.Append(Name + ";");
            Builder.Append(Class + ";");
            Builder.Append(Skin + "^" + Size + ";");
            Builder.Append(Sex + ";0,0,0," + (Level + ID) + ";"); // Sex + Alignment
            Builder.Append(SunDofus.Basic.DeciToHex(Color) + ";");
            Builder.Append(SunDofus.Basic.DeciToHex(Color2) + ";");
            Builder.Append(SunDofus.Basic.DeciToHex(Color3) + ";");
            Builder.Append(",,,,,;"); // Items
            Builder.Append("0;"); //Aura
            Builder.Append(";;");
            Builder.Append(";"); // Guild
            Builder.Append(";0;");
            Builder.Append(";"); // Mount

            return Builder.ToString();
        }

        #endregion

        #region Map

        public void LoadMap()
        {
            Map.Map m_M = Database.Data.MapSql.ListOfMaps.First(x => x.id == this.MapID);
            if (m_M == null) return;

            if (m_M.key == "")
            {
                Client.Send("GDM|" + m_M.id + "|" + m_M.date);
            }
            else
            {
                Client.Send("GDM|" + m_M.id + "|" + m_M.date + "|" + m_M.key);
            }
        }

        public void TeleportNewMap(int m_MID, int m_C)
        {
            Client.Send("GA;2;" + ID + ";");

            GetMap().DelPlayer(this);
            Map.Map m_M = Database.Data.MapSql.ListOfMaps.First(x => x.id == m_MID);

            MapID = m_M.id;
            MapCell = m_C;

            LoadMap();
        }

        public Map.Map GetMap()
        {
            return Database.Data.MapSql.ListOfMaps.First(x => x.id == this.MapID);
        }

        #endregion
    }
}
