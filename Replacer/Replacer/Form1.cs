using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Replacer
{
    public partial class Form1 : Form
    {
        private int ReplaceMode = 0;
        private string input1 = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            textChanged();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textChanged();
            input1 = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void textChanged()
        {
            if (ReplaceMode == 0)
            {
                richTextBox2.Text = replace(richTextBox1.Text, textBox3.Text, textBox4.Text);
            }
            else if (ReplaceMode == 1)
            {
                using (StringReader reader = new StringReader(richTextBox1.Text))
                {
                    string output = "";
                    string line;
                    int orgnr = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        orgnr++;
                        output += replace(replace(textBox1.Text, "$l", line), "$n", orgnr.ToString()) + Environment.NewLine;
                    }
                    richTextBox2.Text = output;
                }
            }
            else if (ReplaceMode == 2)
            {
                string output = GetStringInBetween(textBox3.Text, textBox4.Text, richTextBox1.Text, false, false);

                if (output.Length > 0)
                {
                    richTextBox2.Text = output;
                }
                else
                {
                    richTextBox2.Text = "No match";
                }

            }
            else if (ReplaceMode == 3)
            {
                if (textBox3.Text.Length > 0 && textBox4.Text.Length > 0)
                {
                    using (StringReader reader = new StringReader(richTextBox1.Text))
                    {
                        string output = "";
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string temp = GetStringInBetween(textBox3.Text, textBox4.Text, line, false, false);
                            if(temp.Length > 0)
                            {
                                output += temp + Environment.NewLine;
                            }
                            
                        }

                        if(output.Length > 0)
                        {
                            richTextBox2.Text = output;
                        }
                        else
                        {
                            richTextBox2.Text = "No match";
                        }
                        
                    }
                }
                else
                {
                    richTextBox2.Text = richTextBox1.Text;
                }
            }
            else if (ReplaceMode == 4)
            {
                using (StringReader reader = new StringReader(richTextBox1.Text))
                {
                    string output = "";
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains(textBox1.Text))
                        {
                            output += line + Environment.NewLine;
                        }
                    }

                    if (output.Length > 0)
                    {
                        richTextBox2.Text = output;
                    }
                    else
                    {
                        richTextBox2.Text = "No match";
                    }
                }
            }
            else if (ReplaceMode == 5)
            {
                using (StringReader reader = new StringReader(richTextBox1.Text))
                {
                    string output = "";
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!(line.Contains(textBox1.Text)))
                        {
                            output += line + Environment.NewLine;
                        }
                    }

                    if (output.Length > 0)
                    {
                        richTextBox2.Text = output;
                    }
                    else
                    {
                        richTextBox2.Text = "No match";
                    }
                }
            }
        }

        public string replace(string source, string replace, string with)
        {
            button1.Text = "Find";
            if (replace != "")
            {
                string output = source.Replace(replace, with);
                return output;

            }
            else
            {
                return source;
            }


        }

        public static string GetStringInBetween(string strBegin, string strEnd, string strSource, bool includeBegin, bool includeEnd)
        {
            string[] result = { string.Empty, string.Empty };
            int iIndexOfBegin = strSource.IndexOf(strBegin);

            if (iIndexOfBegin != -1)
            {
                if (includeBegin)
                    iIndexOfBegin -= strBegin.Length;

                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);

                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    if (includeEnd)
                        iEnd += strEnd.Length;
                    result[0] = strSource.Substring(0, iEnd);
                    if (iEnd + strEnd.Length < strSource.Length)
                        result[1] = strSource.Substring(iEnd + strEnd.Length);
                }
            }
            else
                result[1] = strSource;
            return result[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(richTextBox2.Text.Length > 0)
            {
                System.Windows.Forms.Clipboard.SetText(richTextBox2.Text);
                resetAll();
            }
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
            textBox1.Text = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            resetAll();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form1_Resize(sender, e);
            comboBox1.SelectedIndex = comboBox1.FindStringExact("Replace with");

            ToolTip toolTip1 = new ToolTip();
            toolTip1.SetToolTip(label6, "Read from Textfile");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReplaceMode = comboBox1.SelectedIndex;
            textBox3.Text = input1;
            textBox1.Text = input1;

            if (ReplaceMode == 0)
            {
                label3.Text = "Replace / Find";
                label4.Show();
                label4.Text = "with";
                textBox3.Show();
                textBox4.Show();
                textBox1.Hide();
            }
            else if (ReplaceMode == 1)
            {
                label3.Text = "Line Syntax (click for variables)";
                label4.Hide();
                textBox3.Hide();
                textBox4.Hide();
                textBox1.Show();
            }
            else if (ReplaceMode == 2)
            {
                label3.Text = "Between this";
                label4.Show();
                label4.Text = "and that";
                textBox3.Show();
                textBox4.Show();
                textBox1.Hide();
            }
            else if (ReplaceMode == 3)
            {
                label3.Text = "Between this";
                label4.Show();
                label4.Text = "and that";
                textBox3.Show();
                textBox4.Show();
                textBox1.Hide();
            }
            else if (ReplaceMode == 4)
            {
                label3.Text = "Show line only if contains";
                label4.Hide();
                textBox3.Hide();
                textBox4.Hide();
                textBox1.Show();
            }
            else if (ReplaceMode == 5)
            {
                label3.Text = "Show line only if not contains";
                label4.Hide();
                textBox3.Hide();
                textBox4.Hide();
                textBox1.Show();
            }

            textChanged();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            input1 = textBox1.Text;
            textChanged();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int width = this.Width;
            int height = this.Height;

            this.MinimumSize = new Size(350, 400);

            label3.Location = new Point(label3.Location.X, (height - 120) / 2);
            textBox3.Location = new Point(textBox3.Location.X, label3.Location.Y + 15);
            textBox3.Width = (width - 171) / 2;
            textBox1.Location = new Point(textBox3.Location.X, label3.Location.Y + 15);
            richTextBox1.Height = label3.Location.Y - richTextBox1.Location.Y - 3;
            richTextBox1.Width = width - 40;
            label2.Location = new Point(label2.Location.X, textBox3.Location.Y + textBox3.Height + 5);
            richTextBox2.Location = new Point(richTextBox2.Location.X, label2.Location.Y + label2.Height + 5);
            richTextBox2.Height = height - richTextBox2.Location.Y - 90;
            richTextBox2.Width = width - 40;
            comboBox1.Location = new Point(width - comboBox1.Width - 30, textBox1.Location.Y);
            label5.Location = new Point(comboBox1.Location.X, comboBox1.Location.Y - 15);
            textBox4.Location = new Point(textBox3.Location.X + textBox3.Width + 5, textBox3.Location.Y);
            label4.Location = new Point(textBox4.Location.X, textBox4.Location.Y - 15);
            textBox4.Width = comboBox1.Location.X - textBox4.Location.X - 5;
            textBox1.Width = comboBox1.Location.X - textBox1.Location.X - 5;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "Text files|*.txt|All files|*.*";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = theDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            StreamReader reader = new StreamReader(myStream);
                            richTextBox1.Text = reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text.Length > 0)
            {
                string temp = richTextBox2.Text;
                resetAll();
                richTextBox1.Text = temp;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            if(ReplaceMode == 1)
            {
                string info = "$l - Original Line" + Environment.NewLine +
                              "$n - Line Nr." + Environment.NewLine;
                MessageBox.Show(info, "Variables");
            }
        }
    }
}
