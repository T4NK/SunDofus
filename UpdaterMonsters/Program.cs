using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UpdaterMonsters
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var dico = new Dictionary<int, string>();
                var connexion = new MySqlConnection("server=localhost;uid=root;pwd='';database=ancestra");
                connexion.Open();

                var text = "SELECT * FROM maps";
                var command = new MySqlCommand(text, connexion);
                var reader = command.ExecuteReader();

                while (reader.Read())
                    dico.Add(reader.GetInt32("id"), reader.GetString("monsters"));

                reader.Close();

                connexion.Close();
                connexion = new MySqlConnection("server=localhost;uid=root;pwd='';database=dofusorigin_realm");
                connexion.Open();

                foreach (int i in dico.Keys)
                {
                    text = "UPDATE datas_maps SET monsters=@monsters WHERE id=@id";
                    command = new MySqlCommand(text, connexion);

                    var p = command.Parameters;
                    p.Add(new MySqlParameter("@id", i));
                    p.Add(new MySqlParameter("@monsters", dico[i]));

                    command.ExecuteNonQuery();
                }

                connexion.Close();

                Console.WriteLine("Done.");

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }
        }
    }
}
