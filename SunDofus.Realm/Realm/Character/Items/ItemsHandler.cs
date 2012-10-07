using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class ItemsHandler
    {
        public static int LastID = 0;

        public static int GetNewID()
        {
            return ++LastID;
        }

        public static bool PositionAvaliable(int ItemType, bool Usable, int Position)
        {

            return true;

        }

        public static bool ConditionsAvaliable(Database.Models.Items.ItemModel m_I, Character m_C)
        {
            string Condi = m_I.Conditions;

            if (Condi == "")
                return true;

            bool Avaliable = false;

            foreach(string Cond in Condi.Split('&'))
            {
                string Spliter = Cond.Substring(2, 1);
                int Value = -1;
                int ToCompare = int.Parse(Cond.Substring(3));

                switch(Cond.Substring(0,1))
                {
                    case "C":

                        switch(Cond.Substring(1,1))
                        {
                            case "a":
                                Value = m_C.m_Stats.Agility.Bases;
                                break;

                            case "i":
                                Value = m_C.m_Stats.Intelligence.Bases;
                                break;

                            case "c":
                                Value = m_C.m_Stats.Luck.Bases;
                                break;

                            case "s":
                                Value = m_C.m_Stats.Strenght.Bases;
                                break;

                            case "v":
                                Value = m_C.m_Stats.Life.Bases;
                                break;

                            case "w":
                                Value = m_C.m_Stats.Wisdom.Bases;
                                break;

                            case "A":
                                Value = m_C.m_Stats.Agility.Total();
                                break;

                            case "I":
                                Value = m_C.m_Stats.Intelligence.Total();
                                break;

                            case "C":
                                Value = m_C.m_Stats.Luck.Total();
                                break;

                            case "S":
                                Value = m_C.m_Stats.Strenght.Total();
                                break;

                            case "V":
                                Value = m_C.m_Stats.Life.Total();
                                break;

                            case "W":
                                Value = m_C.m_Stats.Wisdom.Total();
                                break;

                            default:
                                Avaliable = true;
                                break;
                        }

                        break;

                    case "P":

                        switch(Cond.Substring(1,1))
                        {
                            case "G":
                                Value = m_C.Class;
                                break;

                            case "L":
                                Value = m_C.Level;
                                break;

                            case "K":
                                Value = m_C.Kamas;
                                break;

                            default:
                                Avaliable = true;
                                break;
                        }

                        break;

                    default:
                        Avaliable = true;
                        break;
                }

                if (Avaliable == true) 
                    return true;

                if (Spliter != "")
                {
                    switch (Spliter)
                    {
                        case "<":
                            Avaliable = (Value < ToCompare ? true : false);
                            break;

                        case ">":
                            Avaliable = (Value > ToCompare ? true : false);
                            break;

                        case "=":
                            Avaliable = (Value == ToCompare ? true : false);
                            break;

                        case "~":
                            Avaliable = (Value == ToCompare ? true : false);
                            break;

                        case "!":
                            Avaliable = (Value != ToCompare ? true : false);
                            break;

                        default:
                            Avaliable = true;
                            break;
                    }

                    if (Avaliable == false)
                        return false;
                }
            }

            return Avaliable;
        }
    }
}
