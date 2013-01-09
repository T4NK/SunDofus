using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SunDofus.Tools.Class
{
    class DatabaseHandler
    {
        static MySqlConnection m_connection = new MySqlConnection();
        static object m_locker = new object();

        public static bool isConnected = false;
        public static bool isStartToWrite = false;
        public static System.IO.StreamWriter m_writer;

        public static bool Initialise(string _server, string _uid, string _pass, string _db)
        {
            try
            {
                m_connection.ConnectionString = string.Format("server={0};uid={1};pwd='{2}';database={3}",
                    _server, _uid, _pass, _db);

                lock (m_locker)
                    m_connection.Open();

                isConnected = true;

                return true;
            }
            catch 
            {
                return false;
            }
        }

        public static void Close()
        {
            try
            {
                m_connection.Close();
                isConnected = false;
            }
            catch { }
        }

        public static void CreateItemDB(Item _item)
        {
            try
            {
                lock (m_locker)
                {
                    var condistr = string.Join("&", _item.m_conditions);
                    var jetstr = string.Join(",", _item.m_jets);

                    var weaponInfos = _item.m_costAP + ";" + _item.m_minRP + ";" + _item.m_maxRP + ";" + _item.m_critical + ";" + _item.m_fail + ";" +
                        (_item.isTwohands ? "true" : "false") + ";" + (_item.isInline ? "true" : "false") + ";" + (_item.isTwohands ? "true" : "false");

                    var sqlText = "INSERT INTO datas_items VALUES(@id, @name, @type, @level, @weight, @infos, @price, @condi, @jet, @gfx, @descri)";

                    var sqlCommand = new MySqlCommand(sqlText, m_connection);
                    MySqlParameterCollection parametersCollec = sqlCommand.Parameters;
                    
                    parametersCollec.Add(new MySqlParameter("@id", _item.m_id));
                    parametersCollec.Add(new MySqlParameter("@name", _item.m_name));
                    parametersCollec.Add(new MySqlParameter("@type", _item.m_type));
                    parametersCollec.Add(new MySqlParameter("@level", _item.m_level));
                    parametersCollec.Add(new MySqlParameter("@weight", _item.m_weight));
                    parametersCollec.Add(new MySqlParameter("@infos", weaponInfos));
                    parametersCollec.Add(new MySqlParameter("@price", _item.m_price));
                    parametersCollec.Add(new MySqlParameter("@condi", condistr));
                    parametersCollec.Add(new MySqlParameter("@jet", jetstr));
                    parametersCollec.Add(new MySqlParameter("@gfx", _item.m_gfxid));
                    parametersCollec.Add(new MySqlParameter("@descri", _item.m_description));

                    sqlCommand.ExecuteNonQuery();

                    _item.isCreated = true;
                }
            }
            catch(Exception test)
            {
                throw new Exception(test.ToString());
            }
        }

        public static void DeleteItemDB(int ID)
        {
            try
            {                    
                lock (m_locker)
                {
                    var sqlText = "DELETE FROM datas_items WHERE ID=@id";
                    var sqlCommand = new MySqlCommand(sqlText, m_connection);

                    MySqlParameterCollection parametersCollec = sqlCommand.Parameters;
                    parametersCollec.Add(new MySqlParameter("@id", ID));

                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static void UpdateItemDB(Item _item)
        {
            DeleteItemDB(_item.m_id);
            CreateItemDB(_item);
        }

        public static Class.Item GetItemDB(int ID)
        {
            try
            {
                var item = new Item();

                var sqlText = "SELECT * FROM datas_items WHERE ID=@id";
                var sqlCommand = new MySqlCommand(sqlText, m_connection);

                sqlCommand.Parameters.Add(new MySqlParameter("@id", ID));
                var sqlResult = sqlCommand.ExecuteReader();

                if (sqlResult.Read())
                {
                    item.m_id = sqlResult.GetInt32("ID");
                    item.m_name = sqlResult.GetString("Name");

                    item.m_type = sqlResult.GetInt32("Type");
                    item.m_level = sqlResult.GetInt32("Level");
                    item.m_weight = sqlResult.GetInt32("Weight");

                    item.m_gfxid = sqlResult.GetInt32("GfxID");

                    item.m_description = sqlResult.GetString("Description");

                    var weaponDatas = sqlResult.GetString("WeaponInfo").Split(';');
                    if(weaponDatas.Length > 1)
                    {
                        item.m_costAP = int.Parse(weaponDatas[0]);
                        item.m_minRP = int.Parse(weaponDatas[1]);
                        item.m_maxRP = int.Parse(weaponDatas[2]);
                        item.m_critical = int.Parse(weaponDatas[3]);
                        item.m_fail = int.Parse(weaponDatas[4]);

                        item.isTwohands = bool.Parse(weaponDatas[5]);
                        item.isInline = bool.Parse(weaponDatas[6]);
                    }

                    item.m_price = sqlResult.GetInt32("Price");

                    if (sqlResult.GetString("Conditions") != "")
                    {
                        var datas = sqlResult.GetString("Conditions").Split('&');

                        foreach (var infos in datas)
                            item.m_conditions.Add(infos);
                    }

                    if (sqlResult.GetString("Stats") != "")
                    {
                        var datas = sqlResult.GetString("Stats").Split(',');

                        foreach (var infos in datas)
                            item.m_jets.Add(infos);
                    }

                    item.isCreated = true;
                }

                sqlResult.Close();

                if (item.m_id == -1)
                    return null;

                return item;
            }
            catch
            {
                return null;
            }
        }

        public static void CreateItemSQLFile(Item _item)
        {
            if (!isStartToWrite)
            {
                if (!System.IO.Directory.Exists("./output/"))
                    System.IO.Directory.CreateDirectory("./output/");

                if (!System.IO.Directory.Exists("./output/" + DateTime.Now.Date.ToString("MMMM dd yyyy")))
                    System.IO.Directory.CreateDirectory("./output/" + DateTime.Now.Date.ToString("MMMM dd yyyy"));

                m_writer = new System.IO.StreamWriter("./output/" + DateTime.Now.Date.ToString("MMMM dd yyyy") + "/" + DateTime.Now.ToString().Replace(":", "-") + ".sql");
                m_writer.AutoFlush = true;

                isStartToWrite = true;
            }

            var condistr = string.Join("&", _item.m_conditions);
            var jetstr = string.Join(",", _item.m_jets);

            var weaponInfos = _item.m_costAP + ";" + _item.m_minRP + ";" + _item.m_maxRP + ";" + _item.m_critical + ";" + _item.m_fail + ";" +
                (_item.isTwohands ? "true" : "false") + ";" + (_item.isInline ? "true" : "false") + ";" + (_item.isTwohands ? "true" : "false");

            m_writer.WriteLine("DELETE FROM datas_items WHERE ID='{0}';", _item.m_id);
            m_writer.WriteLine("INSERT INTO datas_items VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');",
                _item.m_id, _item.m_name, _item.m_type, _item.m_level, _item.m_weight, weaponInfos, _item.m_price, condistr, jetstr, _item.m_gfxid, _item.m_description);
        }
    }
}
