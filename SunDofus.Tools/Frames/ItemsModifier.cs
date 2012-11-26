using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SunDofus.Tools.Frames
{
    public partial class ItemsModifier : Form
    {
        public bool isTwoHands = false;
        public bool isTargetable = false;

        int value1 { get; set; }
        int value2 { get; set; }

        public ItemsModifier()
        {
            InitializeComponent();

            treeView1.Nodes.Add("+ Life");
            treeView1.Nodes.Add("+ Life(2)");
            treeView1.Nodes.Add("+ Wisdom");
            treeView1.Nodes.Add("+ Strengh");
            treeView1.Nodes.Add("+ Intelligence");
            treeView1.Nodes.Add("+ Agility");
            treeView1.Nodes.Add("+ Luck");
            treeView1.Nodes.Add("+ Weaknesses Strengh");
            treeView1.Nodes.Add("+ Weaknesses Intelligence");
            treeView1.Nodes.Add("+ Weaknesses Agility");
            treeView1.Nodes.Add("+ Weaknesses Luck");
            treeView1.Nodes.Add("+ Action Point");
            treeView1.Nodes.Add("- Action Point");
            treeView1.Nodes.Add("+ Range Point");
            treeView1.Nodes.Add("- Range Point");
            treeView1.Nodes.Add("+ Move Point");
            treeView1.Nodes.Add("- Move Point");
            treeView1.Nodes.Add("+ Damages");
            treeView1.Nodes.Add("- Damages");
            treeView1.Nodes.Add("+ Critical Hit");
            treeView1.Nodes.Add("+ Fail Hit");
            treeView1.Nodes.Add("+ Weight");
            treeView1.Nodes.Add("- Weight");
            treeView1.Nodes.Add("+ Initiative");
            treeView1.Nodes.Add("+ Prospection");
            treeView1.Nodes.Add("+ Heal");
            treeView1.Nodes.Add("+ Creature invocable");
            treeView1.Nodes.Add("Stealed of Luck");
            treeView1.Nodes.Add("Stealed of Strengh");
            treeView1.Nodes.Add("Stealed of Agility");
            treeView1.Nodes.Add("Stealed of Intelligence");
            treeView1.Nodes.Add("Stealed of Neutral");
            treeView1.Nodes.Add("Damages (Luck)");
            treeView1.Nodes.Add("Damages (Strengh)");
            treeView1.Nodes.Add("Damages (Agility)");
            treeView1.Nodes.Add("Damages (Intelligence)");
            treeView1.Nodes.Add("Damages (Neutral)");
        }

        private void ItemsModifier_Load(object sender, EventArgs e)
        {
            Program.m_itemsmodifier.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isTwoHands = !isTwoHands;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            isTargetable = !isTargetable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                value1 = int.Parse(textBox7.Text);
                value2 = int.Parse(textBox8.Text);

                treeView3.Nodes.Add(getType(treeView1.SelectedNode.Text) + 
                    "#" + ToHexa(value1) + "#"  + ToHexa(value2) + "#0#1d" + ((value2 + 1) - value1) + "+" + (value1 - 1));
            }
            catch { }
        }

        string ToHexa(int _deci)
        {
            if (_deci == -1) 
                return "-1";

            return _deci.ToString("x");
        }

        object getType(string _path)
        {
            switch (_path)
            {
                case"+ Life":
                case "+ Life(2)":
                    return "6e" ;

                case "+ Wisdom":
                    return "7c";

                case "+ Strengh":
                    return "76";

                case "+ Intelligence":
                    return "7e";

                case "+ Agility":
                    return "77";

                case "+ Luck":
                    return "7b";

                case "+ Weaknesses Strengh":
                    return "9d";

                case "+ Weaknesses Intelligence":
                    return "9b";

                case "+ Weaknesses Agility":
                    return "9a";

                case "+ Weaknesses Luck":
                    return "98";

                case "+ Action Point":
                    return "6f";

                case "- Action Point":
                    return "65";

                case "+ Range Point":
                    return "75";

                case "- Range Point":
                    return "74";

                case "+ Move Point":
                    return "80";

                case "- Move Point":
                    return "7f";

                case "+ Damages":
                    return "79";

                case "- Damages":
                    return "91";

                case "+ Critical Hit":
                    return "73";

                case "Fail Hit":
                    return "7a";

                case "+ Weight":
                    return "9e";

                case "- Weight":
                    return "da";

                case "+ Initiative":
                    return "ae";

                case "+ Prospection":
                    return "b0";

                case "+ Heal":
                    return "b2";

                case "+ Creature invocable":
                    return "b6";

                case "Stealed of Luck":
                    return "5b";

                case "Stealed of Strengh":
                    return "5c";

                case "Stealed of Agility":
                    return "5d";

                case "Stealed of Intelligence":
                    return "5e";

                case "Stealed of Neutral":
                    return "5f";

                case "Damages (Luck)":
                    return "60";

                case "Damages (Strengh)":
                    return "61";

                case "Damages (Agility)":
                    return "62";

                case "Damages (Intelligence)":
                    return "63";

                case "Damages (Neutral)":
                    return "64";

                default:
                    return -1;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                treeView3.Nodes.Remove(treeView3.SelectedNode);
            }
            catch { }
        }
    }
}
