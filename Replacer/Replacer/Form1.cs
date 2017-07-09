using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Replacer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            richTextBox2.Text = replace(richTextBox1.Text, textBox3.Text, textBox4.Text);
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.Text = replace(richTextBox1.Text, textBox3.Text, textBox4.Text);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.Text = replace(richTextBox1.Text, textBox3.Text, textBox4.Text);
        }

        public string replace(string source, string replace, string with)
        {
            button1.Text = "Find";
            if (replace != "" && with != "")
            {
                if (with != "-1")
                {
                    string output = source.Replace(replace, with);
                    return output;
                }
                else
                {
                    string output = source.Replace(replace, "");
                    return output;
                }

            }
            else
            {
                return source;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(richTextBox2.Text);
            resetAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox4.Text.Length == 0)
            {
                richTextBox2.SelectionBackColor = Color.Empty;
                int index = 0;
                while (index < richTextBox1.Text.LastIndexOf(textBox3.Text))
                {
                    richTextBox2.Find(textBox3.Text, index, richTextBox2.TextLength, RichTextBoxFinds.None);
                    richTextBox2.SelectionBackColor = Color.Yellow;
                    index = richTextBox2.Text.IndexOf(textBox3.Text, index) + 1;
                }
                setint();

            }
            else
            {
                button1.Text = "0 matches";
            }
            
        }

        public void setint()
        {
            int counted = Regex.Matches(richTextBox1.Text, textBox3.Text).Count;
            button1.Text = counted.ToString() + " matches";
        }
        public void resetAll()
        {
            richTextBox1.Text = null;
            richTextBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            resetAll();
        }
    }
}
