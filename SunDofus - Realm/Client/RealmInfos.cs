using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Client
{
    class RealmInfos
    {
        public string Pseudo;
        public string Question;
        public string Answer;
        public int Id;
        public int Level;
        public string Characters;

        public RealmInfos()
        {
            Pseudo = "";
            Question = "";
            Answer = "";
            Id = -1;
            Level = -1;
            Characters = "";
        }
    }
}
