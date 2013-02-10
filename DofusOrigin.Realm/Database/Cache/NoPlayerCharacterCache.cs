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
        public static List<Models.NPC.NPCsQuestion> m_questions = new List<Models.NPC.NPCsQuestion>();
        public static List<Models.NPC.NPCsAnswer> m_answers = new List<Models.NPC.NPCsAnswer>();

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

                        if(sqlReader.GetInt32("initQuestion") != -1)
                            npcModel.m_question = m_questions.First(x => x.m_questionID == sqlReader.GetInt32("initQuestion"));

                        npcModel.m_name = sqlReader.GetString("Name");
                        npcModel.m_items = sqlReader.GetString("Items");
                    }

                    foreach (var itemToSell in sqlReader.GetString("SellingList").Split(','))
                    {
                        if (itemToSell == "")
                            continue;

                        npcModel.m_sellingList.Add(int.Parse(itemToSell));
                    }

                    var npc = new Realm.Characters.NPC.NPCMap(npcModel);
                    {
                        var infosMap = sqlReader.GetString("Mapinfos").Split(';');
                        npc.m_mapid = int.Parse(infosMap[0]);
                        npc.m_cellid = int.Parse(infosMap[1]);
                        npc.m_dir = int.Parse(infosMap[2]);
                        npc.mustMove = bool.Parse(infosMap[3]);

                        if (MapsCache.m_mapsList.Any(x => x.GetModel.m_id == npc.m_mapid))
                        {
                            npc.m_idOnMap = MapsCache.m_mapsList.First(x => x.GetModel.m_id == npc.m_mapid).NextNpcID();
                            MapsCache.m_mapsList.First(x => x.GetModel.m_id == npc.m_mapid).Npcs.Add(npc);

                            npc.StartMove();
                        }
                    }

                    m_npcList.Add(npc);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' npcs@ from the database !", m_npcList.Count));
        }

        public static void LoadNPCsQuestions()
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "SELECT * FROM datas_npcs_questions";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var question = new Models.NPC.NPCsQuestion();
                    question.m_questionID = sqlReader.GetInt32("questionID");
                    question.m_rescueQuestionID = sqlReader.GetInt32("rescueQuestion");

                    foreach (var answer in sqlReader.GetString("answers").Split(';'))
                    {
                        if (answer == "")
                            continue;

                        question.m_answers.Add(m_answers.First(x => x.m_answerID == int.Parse(answer)));
                    }

                    foreach (var condi in sqlReader.GetString("conditions").Split('&'))
                    {
                        if (condi == "")
                            continue;

                        var condiInfos = condi.Split(';');
                        var condiObject = new Realm.World.Conditions.NPCConditions();

                        condiObject.CondiID = int.Parse(condiInfos[0]);
                        condiObject.Args = condiInfos[1];

                        question.m_conditions.Add(condiObject);
                    }

                    m_questions.Add(question);

                }

                sqlReader.Close();
            }

            foreach (var question in m_questions.Where(x => x.m_rescueQuestionID != -1))
                question.m_rescueQuestion = m_questions.First(x => x.m_questionID == question.m_rescueQuestionID);

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' npcsQuestions@ from the database !", m_questions.Count));
        }

        public static void LoadNPCsAnswers()
        {
            lock (DatabaseHandler.m_locker)
            {
                var sqlText = "SELECT * FROM datas_npcs_answers";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.m_connection);

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var answer = new Models.NPC.NPCsAnswer();

                    answer.m_answerID = sqlReader.GetInt32("answerID");
                    answer.m_effects = sqlReader.GetString("effects");

                    foreach (var condi in sqlReader.GetString("conditions").Split('&'))
                    {
                        if (condi == "")
                            continue;

                        var condiInfos = condi.Split(';');
                        var condiObject = new Realm.World.Conditions.NPCConditions();

                        condiObject.CondiID = int.Parse(condiInfos[0]);
                        condiObject.Args = condiInfos[1];

                        answer.m_conditions.Add(condiObject);
                    }

                    m_answers.Add(answer);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' npcsAnswers@ from the database !", m_answers.Count));
        }
    }
}
