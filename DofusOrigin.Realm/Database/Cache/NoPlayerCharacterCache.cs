using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Cache
{
    class NoPlayerCharacterCache
    {
        public static List<Realm.Characters.NPC.NPCMap> m_npcList = new List<Realm.Characters.NPC.NPCMap>();

        public static void LoadNPCs()
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "SELECT * FROM datas_npcs";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var npcModel = new Models.NPC.NoPlayerCharacterModel();
                    {
                        npcModel.m_id = sqlReader.GetInt32("ID");
                        npcModel.m_gfxid = sqlReader.GetInt32("Gfx");
                        npcModel.m_size = sqlReader.GetInt32("Size");
                        npcModel.m_sex = sqlReader.GetInt32("Sex");
                        npcModel.m_color = sqlReader.GetInt32("Color1");
                        npcModel.m_color2 = sqlReader.GetInt32("Color2");
                        npcModel.m_color3 = sqlReader.GetInt32("Color3");

                        npcModel.m_name = sqlReader.GetString("Name");
                        npcModel.m_items = sqlReader.GetString("Items");
                    }

                    foreach (var itemToSell in sqlReader.GetString("SellingList").Split(','))
                    {
                        if (itemToSell == "")
                            continue;

                        npcModel.m_sellingList.Add(int.Parse(itemToSell));
                    }

                    foreach (var question in sqlReader.GetString("initQuestions").Split(','))
                    {
                        if (question == "")
                            continue;

                        npcModel.m_questions.Add(int.Parse(question));
                    }

                    var npc = new Realm.Characters.NPC.NPCMap(npcModel);
                    {
                        var infosMap = sqlReader.GetString("Mapinfos").Split(';');
                        npc.m_mapid = int.Parse(infosMap[0]);
                        npc.m_cellid = int.Parse(infosMap[1]);
                        npc.m_dir = int.Parse(infosMap[2]);
                        npc.mustMove = bool.Parse(infosMap[3]);

                        if (MapsCache.m_mapsList.Any(x => x.m_map.m_id == npc.m_mapid))
                        {
                            npc.m_idOnMap = MapsCache.m_mapsList.First(x => x.m_map.m_id == npc.m_mapid).NextNpcID();
                            MapsCache.m_mapsList.First(x => x.m_map.m_id == npc.m_mapid).m_npcs.Add(npc);

                            npc.StartMove();
                        }
                    }

                    m_npcList.Add(npc);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.m_statusLogger.Write(string.Format("Loaded @'{0}' npcs@ from the database !", m_npcList.Count));
        }
    }
}
