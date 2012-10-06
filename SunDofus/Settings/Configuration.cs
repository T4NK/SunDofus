using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SunDofus
{
    public class Configuration
    {
        Dictionary<string, string> myElements;

        public Configuration()
        {
            myElements = new Dictionary<string, string>();
        }

        public void LoadConfiguration(string Path)
        {
            StreamReader Reader = new StreamReader(Path);

            while (!Reader.EndOfStream)
            {
                string Line = Reader.ReadLine();
                if (Line.Contains("#")) continue;
                if (Line == "") continue;

                string[] Line_Infos = Line.Split(' ');
                myElements.Add(Line_Infos[0], Line_Infos[1].Replace(";", ""));
            }
        }

        public void InsertElement(string Key, string Value)
        {
            myElements.Add(Key, Value);
        }

        public bool ExistElement(string Key)
        {
            if (myElements.ContainsKey(Key)) return true;
            return false;
        }

        public string GetStringElement(string Element)
        {
            if (!myElements.ContainsKey(Element)) return "";
            return myElements[Element];
        }

        public int GetIntElement(string Element)
        {
            if (!myElements.ContainsKey(Element)) return -1;
            return int.Parse(myElements[Element]);
        }

        public bool GetBoolElement(string Element)
        {
            if (!myElements.ContainsKey(Element)) return false;
            return bool.Parse(myElements[Element]);
        }

        public long GetLongElement(string Element)
        {
            if (!myElements.ContainsKey(Element)) return -1;
            return long.Parse(myElements[Element]);
        }
    }
}
