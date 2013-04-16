using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SunDofus.World.Entities.Cache
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
                        npcModel.ID = sqlReader.GetInt32("ID");
                        npcModel.GfxID = sqlReader.GetInt32("Gfx");
                        npcModel.Size = sqlReader.GetInt32("Size");
                        npcModel.Sex = sqlReader.GetInt32("Sex");
                        npcModel.Color = sqlReader.GetInt32("Color1");
                        npcModel.Color2 = sqlReader.GetInt32("Color2");
                        npcModel.Color3 = sqlReader.GetInt32("Color3");

                        if(sqlReader.GetInt32("initQuestion") != -1)
                            npcModel.Question = QuestionsList.First(x => x.QuestionID == sqlReader.GetInt32("initQuestion"));

                        npcModel.Name = sqlReader.GetString("Name");
                        npcModel.Items = sqlReader.GetString("Items");
                    }

                    foreach (var itemToSell in sqlReader.GetString("SellingList").Split(','))
                    {
                        if (itemToSell == "")
                            continue;

                        lock(npcModel.SellingList)
                            npcModel.SellingList.Add(int.Parse(itemToSell));
                    }

                    var npc = new Realm.Characters.NPC.NPCMap(npcModel);
                    {
                        var infosMap = sqlReader.GetString("Mapinfos").Split(';');
                        npc.MapID = int.Parse(infosMap[0]);
                        npc.MapCell = int.Parse(infosMap[1]);
                        npc.Dir = int.Parse(infosMap[2]);
                        npc.mustMove = bool.Parse(infosMap[3]);

                        if (MapsCache.MapsList.Any(x => x.GetModel.ID == npc.MapID))
                        {
                            var map = MapsCache.MapsList.First(x => x.GetModel.ID == npc.MapID);
                            npc.ID = MapsCache.MapsList.First(x => x.GetModel.ID == npc.MapID).NextNpcID();

                            lock(map.Npcs)
                                map.Npcs.Add(npc);

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
                    question.QuestionID = sqlReader.GetInt32("questionID");
                    question.RescueQuestionID = sqlReader.GetInt32("rescueQuestion");

                    foreach (var answer in sqlReader.GetString("answers").Split(';'))
                    {
                        if (answer == "")
                            continue;

                        lock(question.Answers)
                            question.Answers.Add(AnswersList.First(x => x.AnswerID == int.Parse(answer)));
                    }

                    foreach (var condi in sqlReader.GetString("conditions").Split('&'))
                    {
                        if (condi == "")
                            continue;

                        var condiInfos = condi.Split(';');
                        var condiObject = new Realm.World.Conditions.NPCConditions();

                        condiObject.CondiID = int.Parse(condiInfos[0]);
                        condiObject.Args = condiInfos[1];

                        lock(question.Conditions)
                            question.Conditions.Add(condiObject);
                    }

                    lock(QuestionsList)
                        QuestionsList.Add(question);
                }

                sqlReader.Close();
            }

            foreach (var question in QuestionsList.Where(x => x.RescueQuestionID != -1))
                question.RescueQuestion = QuestionsList.First(x => x.QuestionID == question.RescueQuestionID);

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

                    answer.AnswerID = sqlReader.GetInt32("answerID");
                    answer.Effects = sqlReader.GetString("effects");

                    foreach (var condi in sqlReader.GetString("conditions").Split('&'))
                    {
                        if (condi == "")
                            continue;

                        var condiInfos = condi.Split(';');
                        var condiObject = new Realm.World.Conditions.NPCConditions();

                        condiObject.CondiID = int.Parse(condiInfos[0]);
                        condiObject.Args = condiInfos[1];

                        lock(answer.Conditions)
                            answer.Conditions.Add(condiObject);
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
