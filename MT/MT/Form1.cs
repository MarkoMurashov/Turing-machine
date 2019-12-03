using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Json;

namespace MT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //машина 
        TuringMachine tm = new TuringMachine();

        public void OpenFile(ref string nameread)
        {
            var opf = new OpenFileDialog();
            if (opf.ShowDialog() == DialogResult.OK)
            {
                nameread = opf.FileName;
            }
        }

        public void ReadFile(string nameread, TuringMachine mt)
        {
            StreamReader reader = File.OpenText(nameread);
           
            string xy = "";

            List<Condition> c = new List<Condition>();

            while ((xy = reader.ReadLine()) != null)
            {
                string[] split = xy.Split(' ');

                int state = Convert.ToInt32(split[0]); //
                bool isFinish = Convert.ToBoolean(split[2]); //
               

                string str = split[1].ToString();

                string[] split2 = str.Split('|');

                List<Changes> listChanges = new List<Changes>();

                if (!isFinish)
                {

                    for (int a = 0; a < split2.Length; a++)
                    {
                        string s = split2[a].ToString();
                        string[] split3 = s.Split(',');

                        char curr = Convert.ToChar(split3[0]);
                        char newsymbol = Convert.ToChar(split3[1]);
                        char shift = Convert.ToChar(split3[2]);
                        int newstate = Convert.ToInt32(split3[3]);

                        listChanges.Add(new Changes(curr, newsymbol, shift, newstate));

                    }
                }

                c.Add(new Condition(state, listChanges, isFinish));                
            }

            mt.Conditions = c;            
        }

        public void ReadTape()
        {
           var items = new List<char>();

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                char item = Convert.ToChar(listView1.Items[i].Text);
                items.Add(item);
            }

            tm.InputTape = null;
            tm.InputTape = items;
            tm.Pointer = tm.FindIndexOfStartLetters();
        }

        public void BlackColorForAllTape()
        {
            for(int i = 0; i <listView1.Items.Count; i++)
            {
                listView1.Items[i].ForeColor = Color.Black;
            }
        }

        public void InputTextToTape()
        {
            string str = textBox1.Text.ToString();

            for(int i = 0; i < str.Length; i++ )
            {
                listView1.Items[i + 3].Text = str[i].ToString();
            }
            ReadTape();
        }

        public void ReWriteTape()
        {
            for (int i = 0; i < tm.InputTape.Count; i++)
            {
                listView1.Items[i].Text = tm.InputTape[i].ToString();
            }
        }

        public void ClearTape()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Text = "_";
            }
            tm.CurrState = 0;
        }

    
        private void howIsWorkingThisShitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. Load turing machine data from a file (*.txt or *.json). Also you can input data." + Environment.NewLine +Environment.NewLine +
                            "2. If the machine converts the tape, then select \"need to change\"" + Environment.NewLine  +Environment.NewLine +
                            "3. Input string" + Environment.NewLine + Environment.NewLine +
                            "4. Click on button \"Run\"" + Environment.NewLine + Environment.NewLine +
                            "5. You can check your word by one click(\"Answer\") or step by step(->). " + Environment.NewLine + Environment.NewLine +
                            "6. You can save your turing machine in json file. Just click on \"Save\""+ Environment.NewLine + Environment.NewLine +
                            "7. To clean tape click on\"Reset\"", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CompletelyRead()
        {
            try
            {
                int lastPointer = tm.Pointer;

                listView1.Items[lastPointer].ForeColor = Color.Black;
                listView1.Items[tm.Pointer].ForeColor = Color.Green;


                bool flag = tm.ChangeTape();
                if (!checkBox1.Checked)
                {
                    if (flag)
                    {
                        ReWriteTape();
                        MessageBox.Show("The tape fits the rule!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ReWriteTape();
                        MessageBox.Show("The tape does not fit the rule", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (flag)
                    {
                        ReWriteTape();
                        MessageBox.Show("Conversion successful!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ReWriteTape();
                        MessageBox.Show("Conversion error occurred", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("There is probably something wrong with the tape!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CharRead()
        {
            try
            {
                int lastPointer = tm.Pointer;

                int result = tm.ChangeTapeByOneSymbol();
                               
                listView1.Items[lastPointer].ForeColor = Color.Black;
                listView1.Items[tm.Pointer].ForeColor = Color.Green;

                if (!checkBox1.Checked)
                {
                    if (result == 1)
                    {
                        ReWriteTape();
                        MessageBox.Show("The tape fits the rule!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tm.CurrState = 0;
                    }
                    if (result == 0)
                    {
                        ReWriteTape();
                        MessageBox.Show("The tape does not fit the rule", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tm.CurrState = 0;
                    }
                    if (result == 2)
                    {
                        ReWriteTape();

                    }

                }
                else
                {
                    if (result == 1)
                    {
                        ReWriteTape();
                        MessageBox.Show("Conversion successful!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tm.CurrState = 0;
                    }
                    if (result == 0)
                    {
                        ReWriteTape();
                        MessageBox.Show("Conversion error occurred", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tm.CurrState = 0;
                    }
                    if (result == 2)
                    {
                        ReWriteTape();
                    }
                }
                lblState.Text = tm.CurrState.ToString();
            }
            catch
            {
                MessageBox.Show("There is probably something wrong with the  tape!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearTape();
            BlackColorForAllTape();
        }

        private void RunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputTextToTape();

            BlackColorForAllTape();
            ReadTape();



            if (tm.InputTape.All(tape => tape == '^'))
            {
                MessageBox.Show("The tape must contain more characters than '_'!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tm.InputTape = null;
            }

            listView1.Items[tm.Pointer].ForeColor = Color.Green;

        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CompletelyRead();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            CharRead();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Создаём сериалайзер
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(TuringMachine));
            // Создаём поток
            FileStream buffer = File.Create("tm.json");
            // Сериализуем объект
            jsonSerializer.WriteObject(buffer, tm);
            buffer.Close();

            MessageBox.Show("Saved");
        }

        private void TxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                tm = new TuringMachine();

                var namefile = String.Empty;
                OpenFile(ref namefile);
                ReadFile(namefile, tm);

                //ReadFile("C:\\Users\\Lenovo\\Desktop\\MT\\Test\\Test2\\Test2 01q.txt", tm);

                MessageBox.Show("File uploaded successfully!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("File Read Error!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void JsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                var namefile = String.Empty;
                OpenFile(ref namefile);

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(TuringMachine));
                // Открываем наш файл
                FileStream buffer = File.OpenRead(namefile);
                tm = jsonSerializer.ReadObject(buffer) as TuringMachine;

                MessageBox.Show("File uploaded successfully!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("File Read Error!", "Waring", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            tm = new TuringMachine();
            var c = new List<Condition>();
            
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if(dataGridView1.Rows[i].Cells[0].Value == null ||
                    dataGridView1.Rows[i].Cells[1].Value == null ||
                    dataGridView1.Rows[i].Cells[2].Value == null ||
                    dataGridView1.Rows[i].Cells[3].Value == null ||
                    dataGridView1.Rows[i].Cells[4].Value == null ||
                    dataGridView1.Rows[i].Cells[5].Value == null)
                {
                    break;
                }

                int state = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value); 
                bool isFinish = Convert.ToBoolean(dataGridView1.Rows[i].Cells[5].Value);

                char curr = Convert.ToChar(dataGridView1.Rows[i].Cells[1].Value);
                char newsymbol = Convert.ToChar(dataGridView1.Rows[i].Cells[2].Value);
                char shift = Convert.ToChar(dataGridView1.Rows[i].Cells[3].Value);
                int newstate = Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value);

                if (!isFinish && c.Where(s => s.State == state).Count() >= 1)
                {
                    int index = c.FindIndex(o => o.State == state);

                    c[index].Changes.Add(new Changes(curr, newsymbol, shift, newstate));
                }
                else if (!isFinish)
                {
                    var listChanges = new List<Changes>();
                    listChanges.Add(new Changes(curr, newsymbol, shift, newstate));
                    c.Add(new Condition(state, listChanges, isFinish));
                }
                else
                {
                    var listChanges = new List<Changes>();                    
                    c.Add(new Condition(state, listChanges, isFinish));
                }

            }

            tm.Conditions = c;

            MessageBox.Show("Added");
        }
    }
}
