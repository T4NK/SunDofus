using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SunDofus.Editor.Network
{
    class DBClient
    {
        public static MySqlConnection myConnection;
        public static object myLocker;
        public static bool isConnected = false;

        public static void InitialiseConnection(string s, string u, string p, string n)
        {
            myConnection = new MySqlConnection();
            myLocker = new object();

            try
            {
                myConnection.ConnectionString = string.Format("server={0};uid={1};pwd='{2}';database={3}",s,u,p,n);

                lock (myLocker)
                    myConnection.Open();

                isConnected = true;
            }
            catch (Exception e)
            {
                Program.Message(string.Format("Can't connect to the @database@ : {0}", e.ToString()));
            }
        }

        public static void InsertNewMap()
        {
            if (!isConnected)
                return;

            lock (myLocker)
            {
                var SQLText = "INSERT INTO maps VALUES(@id, @date, @width, @heigh, @places, @key, @datas, @cells, @monsters, @capabilities, @mappos, @numgroup, @groupmaxsize)";
                var SQLCommand = new MySqlCommand(SQLText, myConnection);

                MySqlParameterCollection P = SQLCommand.Parameters;

                P.Add(new MySqlParameter("@id", Program.ActualMap.id));
                P.Add(new MySqlParameter("@date", Program.ActualMap.date));
                P.Add(new MySqlParameter("@width", Program.ActualMap.width));
                P.Add(new MySqlParameter("@heigh", Program.ActualMap.height));
                P.Add(new MySqlParameter("@places", ""));
                P.Add(new MySqlParameter("@key", ""));
                P.Add(new MySqlParameter("@datas", Program.GetMapData()));
                P.Add(new MySqlParameter("@cells", ""));
                P.Add(new MySqlParameter("@monsters", ""));
                P.Add(new MySqlParameter("@capabilities", Program.ActualMap.capabilities));
                P.Add(new MySqlParameter("@mappos", ""));
                P.Add(new MySqlParameter("@numgroup", 3));
                P.Add(new MySqlParameter("@groupmaxsize", 3));
                

                SQLCommand.ExecuteNonQuery();
            }
        }
    }
}
