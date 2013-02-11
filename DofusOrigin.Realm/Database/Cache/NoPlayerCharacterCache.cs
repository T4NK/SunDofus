using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DofusOrigin.Database.Cache
{
    class NoPlayerCharacterCache
    {
        public static List<Realm.Characters.NPC.NPCMap> NpcsList = new List<Realm.Characters.NPC.NPCMap>();
        public static List<Models.NPC.NPCsQuestion> QuestionsList = new List<Models.NPC.NPCsQuestion>();
        public static List<Models.NPC.NPCsAnswer> AnswersList = new List<Models.NPC.NPCsAnswer>();

        public static void LoadNPCs()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_npcs";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

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
                            npcModel.m_question = QuestionsList.First(x => x.m_questionID == sqlReader.GetInt32("initQuestion"));

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
                        npc.MapID = int.Parse(infosMap[0]);
                        npc.MapCell = int.Parse(infosMap[1]);
                        npc.Dir = int.Parse(infosMap[2]);
                        npc.mustMove = bool.Parse(infosMap[3]);

                        if (MapsCache.MapsList.Any(x => x.GetModel.m_id == npc.MapID))
                        {
                            npc.ID = MapsCache.MapsList.First(x => x.GetModel.m_id == npc.MapID).NextNpcID();
                            MapsCache.MapsList.First(x => x.GetModel.m_id == npc.MapID).Npcs.Add(npc);

                            npc.StartMove();
                        }
                    }

                    lock(NpcsList)
                        NpcsList.Add(npc);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' npcs@ from the database !", NpcsList.Count));
        }

        public static void LoadNPCsQuestions()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_npcs_questions";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    var question = new Models.NPC.NPCsQuestion();
                    question.m_questionID = sqlReader.GetInt32("questionID");
                    question.m_rescueQuestionID = sqlReader.GetInt32("rescueQuestion");

                    foreach (var answer in sqlReader.GetString("answers").Split(';'))
                    {
                        if (answer == "")
                            continue;

                        question.m_answers.Add(AnswersList.First(x => x.m_answerID == int.Parse(answer)));
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

                    lock(QuestionsList)
                        QuestionsList.Add(question);
                }

                sqlReader.Close();
            }

            foreach (var question in QuestionsList.Where(x => x.m_rescueQuestionID != -1))
                question.m_rescueQuestion = QuestionsList.First(x => x.m_questionID == question.m_rescueQuestionID);

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' npcsQuestions@ from the database !", QuestionsList.Count));
        }

        public static void LoadNPCsAnswers()
        {
            lock (DatabaseHandler.ConnectionLocker)
            {
                var sqlText = "SELECT * FROM datas_npcs_answers";
                var sqlCommand = new MySqlCommand(sqlText, DatabaseHandler.Connection);

                var sqlReader = sqlCommand.ExecuteReader();

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

                    lock(AnswersList)
                        AnswersList.Add(answer);
                }

                sqlReader.Close();
            }

            Utilities.Loggers.StatusLogger.Write(string.Format("Loaded @'{0}' npcsAnswers@ from the database !", AnswersList.Count));
        }
    }
}
