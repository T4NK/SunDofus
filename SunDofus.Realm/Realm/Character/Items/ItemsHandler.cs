using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class ItemsHandler
    {
        public static int myLastID = 0;

        public static int GetNewID()
        {
            return ++myLastID;
        }

        public static bool PositionAvaliable(int ItemType, bool Usable, int Position)
        {

            return true;

        }

        public static bool ConditionsAvaliable(Database.Models.Items.ItemModel myItem, Character myCharacter)
        {
            var Condi = myItem.myConditions;

            if (Condi == "")
                return true;

            var Avaliable = false;

            foreach(var Cond in Condi.Split('&'))
            {
                var Spliter = Cond.Substring(2, 1);
                var Value = -1;
                var ToCompare = int.Parse(Cond.Substring(3));

                switch(Cond.Substring(0,1))
                {
                    case "C":

                        switch(Cond.Substring(1,1))
                        {
                            case "a":
                                Value = myCharacter.myStats.Agility.Bases;
                                break;

                            case "i":
                                Value = myCharacter.myStats.Intelligence.Bases;
                                break;

                            case "c":
                                Value = myCharacter.myStats.Luck.Bases;
                                break;

                            case "s":
                                Value = myCharacter.myStats.Strenght.Bases;
                                break;

                            case "v":
                                Value = myCharacter.myStats.Life.Bases;
                                break;

                            case "w":
                                Value = myCharacter.myStats.Wisdom.Bases;
                                break;

                            case "A":
                                Value = myCharacter.myStats.Agility.Total();
                                break;

                            case "I":
                                Value = myCharacter.myStats.Intelligence.Total();
                                break;

                            case "C":
                                Value = myCharacter.myStats.Luck.Total();
                                break;

                            case "S":
                                Value = myCharacter.myStats.Strenght.Total();
                                break;

                            case "V":
                                Value = myCharacter.myStats.Life.Total();
                                break;

                            case "W":
                                Value = myCharacter.myStats.Wisdom.Total();
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
                                Value = myCharacter.Class;
                                break;

                            case "L":
                                Value = myCharacter.Level;
                                break;

                            case "K":
                                Value = myCharacter.Kamas;
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
