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
        private bool isTwoHands = false;
        private bool isInLine = false;

        private List<string> m_jets;
        private List<string> m_conditions;

        Class.Item m_item;

        #region base

        public ItemsModifier()
        {
            InitializeComponent();

            m_jets = new List<string>();
            m_conditions = new List<string>();

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

            treeView2.Nodes.Add("Character's base of Agility >");
            treeView2.Nodes.Add("Character's base of Agility <");
            treeView2.Nodes.Add("Character's base of Intelligence >");
            treeView2.Nodes.Add("Character's base of Intelligence <");
            treeView2.Nodes.Add("Character's base of Luck >");
            treeView2.Nodes.Add("Character's base of Luck <");
            treeView2.Nodes.Add("Character's base of Strenght >");
            treeView2.Nodes.Add("Character's base of Strenght <");
            treeView2.Nodes.Add("Character's base of Life >");
            treeView2.Nodes.Add("Character's base of Life <");
            treeView2.Nodes.Add("Character's base of Wisdom >");
            treeView2.Nodes.Add("Character's base of Wisdom <");
            treeView2.Nodes.Add("Character's total of Agility >");
            treeView2.Nodes.Add("Character's total of Agility <");
            treeView2.Nodes.Add("Character's total of Intelligence >");
            treeView2.Nodes.Add("Character's total of Intelligence <");
            treeView2.Nodes.Add("Character's total of Luck >");
            treeView2.Nodes.Add("Character's total of Luck <");
            treeView2.Nodes.Add("Character's total of Strenght >");
            treeView2.Nodes.Add("Character's total of Strenght <");
            treeView2.Nodes.Add("Character's total of Life >");
            treeView2.Nodes.Add("Character's total of Life <");
            treeView2.Nodes.Add("Character's total of Wisdom >");
            treeView2.Nodes.Add("Character's total of Wisdom <");
            treeView2.Nodes.Add("Character's Class =");
            treeView2.Nodes.Add("Character's Level >");
            treeView2.Nodes.Add("Character's Level <");
            treeView2.Nodes.Add("Character's Kamas >");
            treeView2.Nodes.Add("Character's Kamas <");

            m_item = new Class.Item();
        }

        public void UpdateItem()
        {
            if (Program.willOpen == -1)
                return;

            m_item = Class.DatabaseHandler.GetItemDB(Program.willOpen);

            if (m_item == null)
            {
                m_item = new Class.Item();
                return;
            }

            textBox1.Text = m_item.m_id.ToString();
            textBox2.Text = m_item.m_name;

            textBox3.Text = m_item.m_type.ToString();
            textBox4.Text = m_item.m_level.ToString();
            textBox5.Text = m_item.m_weight.ToString();

            textBox6.Text = m_item.m_price.ToString();

            textBox10.Text = m_item.m_gfxid.ToString();

            if (m_item.m_costAP != -1)
                textBox11.Text = m_item.m_costAP.ToString();

            if (m_item.m_minRP != -1)
                textBox12.Text = m_item.m_minRP.ToString();

            if (m_item.m_maxRP != -1)
                textBox13.Text = m_item.m_maxRP.ToString();

            if (m_item.m_critical != -1)
                textBox14.Text = m_item.m_critical.ToString();

            if (m_item.m_fail != -1)
                textBox15.Text = m_item.m_fail.ToString();

            if (m_item.isTwohands == true)
                checkBox1.Checked = true;

            if (m_item.isInline == true)
                checkBox3.Checked = true;

            treeView3.Nodes.Clear();
            foreach (var datas in m_item.m_jets)
                treeView3.Nodes.Add(datas);

            treeView4.Nodes.Clear();
            foreach (var datas in m_item.m_conditions)
                treeView4.Nodes.Add(datas);

            Program.willOpen = -1;
        }

        private void me_willClose(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void ItemsModifier_Load(object sender, EventArgs e)
        {
            Program.m_itemsModifier.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isTwoHands = !isTwoHands;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            isInLine = !isInLine;
        }

        string ToHexa(int _deci)
        {
            if (_deci == -1) 
                return "-1";

            return _deci.ToString("x");
        }

        #endregion

        #region getfunction

        string getConditions(string _condi)
        {
            switch (_condi)
            {
                case "Character's base of Agility >":
                    return "Ca>" ;

                case "Character's base of Agility <":
                    return "Ca<";

                case "Character's base of Intelligence >":
                    return "Ci>";

                case "Character's base of Intelligence <":
                    return "Ci<";

                case "Character's base of Luck >":
                    return "Cc>";

                case "Character's base of Luck <":
                    return "Cc<";

                case "Character's base of Strenght >":
                    return "Cs>";

                case "Character's base of Strenght <":
                    return "Cs<";

                case "Character's base of Life >":
                    return "Cv>";

                case "Character's base of Life <":
                    return "Cv<";

                case "Character's base of Wisdom >":
                    return "Cw>";

                case "Character's base of Wisdom <":
                    return "Cw<";

                case "Character's total of Agility >":
                    return "CA>";

                case "Character's total of Agility <":
                    return "CA<";

                case "Character's total of Intelligence >":
                    return "CI>";

                case "Character's total of Intelligence <":
                    return "CI<";

                case "Character's total of Luck >":
                    return "CC>";

                case "Character's total of Luck <":
                    return "CC<";

                case "Character's total of Strenght >":
                    return "CS>";

                case "Character's total of Strenght <":
                    return "CS<";

                case "Character's total of Life >":
                    return "CV>";

                case "Character's total of Life <":
                    return "CV<";

                case "Character's total of Wisdom >":
                    return "CW>";

                case "Character's total of Wisdom <":
                    return "CW<";

                case "Character's Class =":
                    return "PG=";

                case "Character's Level >":
                    return "PL>=";

                case "Character's Level <":
                    return "PL<";

                case "Character's Kamas >":
                    return "PK>";

                case "Character's Kamas <":
                    return "PK<";

                default:
                    return "";
            }
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

        #endregion

        #region button

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Stats
                var value1 = int.Parse(textBox7.Text);
                var value2 = int.Parse(textBox8.Text);

                var jet = getType(treeView1.SelectedNode.Text) +
                    "#" + ToHexa(value1) + "#" + ToHexa(value2) + "#0#1d" + ((value2 + 1) - value1) + "+" + (value1 - 1);

                treeView3.Nodes.Add(jet);
            }
            catch { }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                //Stats
                treeView3.Nodes.Remove(treeView3.SelectedNode);
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //Condi
                var value1 = int.Parse(textBox9.Text);

                treeView4.Nodes.Add(getConditions(treeView2.SelectedNode.Text) + value1);
            }
            catch { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                //Condi
                treeView4.Nodes.Remove(treeView4.SelectedNode);
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                button3.Enabled = false;

                m_item.m_conditions = new List<string>();
                m_item.m_jets = new List<string>();

                foreach (TreeNode name in treeView3.Nodes)
                    m_item.m_jets.Add(name.Text);

                foreach (TreeNode name in treeView4.Nodes)
                    m_item.m_conditions.Add(name.Text);

                m_item.m_id = int.Parse(textBox1.Text);
                m_item.m_type = int.Parse(textBox3.Text);

                m_item.m_name = textBox2.Text;
                m_item.m_level = int.Parse(textBox4.Text);
                m_item.m_weight = int.Parse(textBox5.Text);
                m_item.m_price = int.Parse(textBox6.Text);
                m_item.m_gfxid = int.Parse(textBox10.Text);

                //If is a Weapon

                if(textBox11.Text != "")
                    m_item.m_costAP = int.Parse(textBox11.Text);

                if (textBox12.Text != "")
                    m_item.m_minRP = int.Parse(textBox12.Text);

                if (textBox13.Text != "")
                    m_item.m_maxRP = int.Parse(textBox13.Text);

                if (textBox14.Text != "")
                    m_item.m_critical = int.Parse(textBox14.Text);

                if (textBox15.Text != "")
                    m_item.m_fail = int.Parse(textBox15.Text);

                m_item.isTwohands = isTwoHands;
                m_item.isInline = isInLine;

                if (Class.DatabaseHandler.isConnected)
                {
                    if (m_item.isCreated == false)
                        Class.DatabaseHandler.CreateItemDB(m_item);
                    else if (m_item.isCreated == true)
                        Class.DatabaseHandler.UpdateItemDB(m_item);
                }
                else
                    Class.DatabaseHandler.CreateItemSQLFile(m_item);

                button3.Enabled = true;
            }
            catch (Exception test)
            {
                MessageBox.Show("Error Format : " + test.ToString());
                button3.Enabled = true;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";

            treeView3.Nodes.Clear();

            treeView4.Nodes.Clear();
        }

        #endregion
    }
}
