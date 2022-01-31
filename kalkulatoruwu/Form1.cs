using System.Text;

namespace kalkulatoruwu
{
    public partial class Form1 : Form
    {
        Dictionary<string, Label> trackBars = new();
        List<List<TrackBar>> Board = new();
        //List<List<TextBox>> Sizes = new();
        Dictionary<string, TextBox> Size = new();
        Dictionary<string, int> Index = new();
        List<ComboBox> comboBoxes = new();
        Button globalButton = new();
        List<List<int>> storedSizes = new();
        List<CheckBox> checkBoxes = new();
        List<Label> labels = new();
        int globalX = 0;
        int globalY = 0;
        //int boardIndex = 0;
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(int x, int y, int index, int offset)
        {
            Label label = new()
            {
                AutoSize = true,
                Location = new System.Drawing.Point(50 + (x * 100) + (index * 200) + (offset), 200 + (y * 100)),
                Name = RandomString(16),
                Size = new System.Drawing.Size(13, 15),
                TabIndex = 29,
                Text = "0"
            };

            TrackBar trackBar = new()
            {
                Location = new System.Drawing.Point(50 + (x * 100) + (index * 200) + (offset), 220 + (y * 100)),
                Maximum = 99999,
                Minimum = -99999,
                Name = RandomString(16),
                Size = new System.Drawing.Size(104, 45),
                TabIndex = 28
            };
            trackBar.Scroll += new EventHandler(trackBar_Scroll);

            // Przypisuje do nazwy komorki label z liczba
            trackBars.Add(trackBar.Name, label);
            Controls.Add(label);
            Controls.Add(trackBar);
            // Dodaje do matrycy o odpowiednim indexie jedna komorke
            Board[index].Add(trackBar);
        }
        private void trackBar_Scroll(object sender, EventArgs e) 
        {
            if (sender is TrackBar bar)
            {
                trackBars[bar.Name].Text = bar.Value.ToString();
            }
        }
        // Funkcja z neta, nie bede komentowac
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
        // Do usuniecia
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {

            }
        }

        private TextBox createTextbox(int x)
        {
            TextBox textBox = new()
            {
                Location = new Point(50 + 93 * (x), 180),
                Name = RandomString(16),
                Size = new Size(33, 23),
                TabIndex = 30
            };
            Controls.Add(textBox);
           
            textBox.TextChanged += new System.EventHandler(createMatrix);
           
            return textBox;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                clearSizes();
                clearBoard();
                clearComboBoxes();
                clearCheckboxes();
                clearLabel();
                Controls.Remove(globalButton);
                int input = int.TryParse(textBox.Text, out input) ? input : 0;
                for (int i = 0; i < input * 2; i+=2)
                {
                    // To jest bezsensu, ale nie bede tego juz zmienial
                    var temp1 = createTextbox(i);
                    var temp2 = createTextbox(i+1);

                    Size.Add(temp1.Name, temp2);
                    Size.Add(temp2.Name, temp1);

                    createLabel(i % 2 == 0);
                    createLabel((i+1) % 2 == 0);

                    storedSizes.Add(new List<int>());

                    Index.Add(temp1.Name, i/2);
                    Index.Add(temp2.Name, i/2);

                    // Przygotowuje miejsce na matryce
                    List<TrackBar> temp = new();
                    Board.Add(temp);
                }
            }
        }
        private void clearSizes()
        {
            foreach (TextBox value in Size.Values)
            {
                Controls.Remove(value);
            }
            Index.Clear();
            Size.Clear();
        }
        private void createMatrix(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Czysci matryce, w ktorej ulegly zmiany
                clearMatrix(Index[textBox.Name]);
                storedSizes[Index[textBox.Name]].Clear();
                clearCheckboxes();
                Controls.Remove(globalButton);
                if (textBox.Text != "" && Size[textBox.Name].Text != "")
                {
                    if (Index[textBox.Name] == Index.Count / 2 - 1) createSubmit();
                    int offset = 0;
                    int x = int.TryParse(textBox.Text, out x) ? x : 0;
                    int y = int.TryParse(Size[textBox.Name].Text, out y) ? y : 0;

                    storedSizes[Index[textBox.Name]].Add(x);
                    storedSizes[Index[textBox.Name]].Add(y);

                    for (int i = 0; i < x; i++)
                    {
                        if (Index[textBox.Name] > 0)
                        {
                            if (Board[Index[textBox.Name] - 1].Capacity != 0)
                            {
                                // Nie jestem w stanie powiedziec, dlaczego to dziala
                                offset = Board[Index[textBox.Name] - 1].Last().Location.X - ((Index[textBox.Name] - 1) * 200);
                            }
                        }
                        for (int j = 0; j < y; j++)
                        {
                            button1_Click(i, j, Index[textBox.Name], offset);
                        }
                        //if (i > 0) createComboBox(Index[textBox.Name] - 1, offset);
                    }
                    if (Index[textBox.Name] > 0) createComboBox(Index[textBox.Name]-1, offset);
                }
            }
        }
        private void clearMatrix(int index)
        {
            foreach (var cell in Board[index])
            {
                Controls.Remove(cell);
                Controls.Remove(trackBars[cell.Name]);
            }
            Board[index].Clear();
        }
        private void clearBoard()
        {
            foreach (var cols in Board)
            {
                foreach (var cell in cols)
                {
                    Controls.Remove(cell);
                    Controls.Remove(trackBars[cell.Name]);
                }
            }
            Board.Clear();
        }
        private ComboBox createComboBox(int index, int offset)
        {
            ComboBox comboBox = new()
            {
                FormattingEnabled = true,
                Text = "Znak",
                Name = RandomString(16),
                Location = new Point(150 + offset + (index * 200), 300),
                Size = new Size(50, 23)
            };
            comboBox.Items.Add("+");
            comboBox.Items.Add("-");
            comboBox.Items.Add("*");
            Controls.Add(comboBox);
            comboBoxes.Add(comboBox);
            return comboBox;
        }
        private void clearComboBoxes()
        {
            foreach (ComboBox comboBox in comboBoxes)
            {
                Controls.Remove(comboBox);
            }
            comboBoxes.Clear();
        }
        private void createSubmit()
        {
            Button button = new()
            {
                Location = new Point(250, 114),
                Name = RandomString(16),
                Text = "Calculate",
                Size = new Size(75, 23),
                TabIndex = 20,
            };
            button.Click += new System.EventHandler(calculate);
            globalButton = button;
            Controls.Add(button);
        }
        private void calculate(object sender, EventArgs e)
        {
            clearCheckboxes();
            List < ComboBox > c = Controls.OfType<ComboBox>().ToList();
            //this.richTextBox1.Text = c[0].SelectedIndex.ToString();
            List<TrackBar> final = Board[0];
            globalX = storedSizes[0][1];
            globalY = storedSizes[0][0];
            for (int i = 1; i <= comboBoxes.Count; i++)
            {
                switch (c[i - 1].SelectedIndex)
                {
                    case 0:
                        if (globalX == storedSizes[i][1] && globalY == storedSizes[i][0])
                            final = Add(final, Board[i]);
                        break;
                    case 1:
                        if (globalX == storedSizes[i][1] && globalY == storedSizes[i][0])
                            final = Subst(final, Board[i]);
                        break;
                    case 2:
                        if (globalY == storedSizes[i][1])
                            final = Mul(final, Board[i], storedSizes[i][1], storedSizes[i][0]);
                        break;
                }
            }
            int z = 0;
            for (int i = 0; i < globalY; i++)
            {
                for (int j = 0; j < globalX; j++)
                {
                    CheckBox checkbox = new()
                    {
                        AutoSize = true,
                        Location = new Point(50 + 100 * i, 600 + (j * 100)),
                        Size = new Size(83, 19),
                        TabIndex = 29,
                        Text = "",
                        UseVisualStyleBackColor = true
                    };
                    this.helpProvider1.SetShowHelp(checkbox, true);
                    this.helpProvider1.SetHelpString(checkbox, final[z].Value.ToString());
                    checkBoxes.Add(checkbox);
                    Controls.Add(checkbox);
                    z++;
                }
            }
        }
        private List<TrackBar> Add(List<TrackBar> textBox1, List<TrackBar> textBox2)
        {
            List<TrackBar> temp = new();
            for (int i = 0; i < textBox1.Count; i++)
            {
                int sum = textBox1[i].Value + textBox2[i].Value;
                TrackBar temp2 = new()
                {
                    Text = sum.ToString(),
                    Name = RandomString(16),
                    Size = new Size(33, 23),
                    TabIndex = 30,
                    Minimum = -999999,
                    Maximum = 999999,
                    Value = sum
                };
                temp.Add(temp2);
            }
            return temp;
        }
        private List<TrackBar> Subst(List<TrackBar> textBox1, List<TrackBar> textBox2)
        {
            List<TrackBar> temp = new();
            for (int i = 0; i < textBox1.Count; i++)
            {
                int sum = textBox1[i].Value - textBox2[i].Value;
                TrackBar temp2 = new()
                {
                    Text = sum.ToString(),
                    Name = RandomString(16),
                    Size = new Size(33, 23),
                    Minimum = -999999,
                    Maximum = 999999,
                    TabIndex = 30,
                    Value = sum
                };
                temp.Add(temp2);
            }
            return temp;
        }
        private List<TrackBar> Mul(List<TrackBar> textBox1, List<TrackBar> textBox2, int x, int y)
        {
            // Przepraszam za to
            //this.richTextBox1.Text = "yyy";
            TrackBar[,] _temp1 = new TrackBar[globalX, globalY];
            TrackBar[,] _temp2 = new TrackBar[x, y];
            int[,] _temp3 = new int[globalX, y];
            int z = 0;
            for (int i = 0; i < globalX; i++)
            {
                for (int j = 0; j < globalY; j++)
                {
                    _temp1[i,j] = textBox2[z];
                    z++;
                }
            }
            z = 0;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    _temp2[i, j] = textBox1[z];
                    z++;
                }
            }
            for (int i = 0; i < globalX; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    for (int k = 0; k < x; k++)
                    {
                        _temp3[i, j] += _temp1[i, k].Value * _temp2[k, j].Value;
                        //this.richTextBox1.Text += _temp1[i, k].Value.ToString() + " " + _temp2[k, j].Value.ToString();
                    }
                    //this.richTextBox1.Text += "\n";
                }
            }
            List<TrackBar> temp = new();
            for (int i = 0; i < globalX; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    // Jezeli wyrzucilo tutaj blad to znaczy, ze wynik nie 
                    // zmiescil sie w przedziale od Minimum do Maximum
                    // Zeby to naprawic, wystarczy zmienic te wartosci
                    TrackBar temp2 = new()
                    {
                        Text = "",
                        Name = RandomString(16),
                        Size = new Size(33, 23),
                        Minimum = -99999999,
                        Maximum = 99999999,
                        TabIndex = 30,
                        Value = _temp3[i, j] > -99999999 && _temp3[i, j] < 99999999 ? _temp3[i, j] : 99999999
                    };
                    temp.Add(temp2);
                    //this.richTextBox1.Text += _temp3[i, j].ToString() + " ";
                }
            }
            globalY = y;
            return temp;
        }
        private void clearCheckboxes()
        {
            foreach (var checkbox in checkBoxes)
            {
                Controls.Remove(checkbox);
            }
            checkBoxes.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void TimerEventProcessor(Object myObject,
                                           EventArgs myEventArgs)
        {
            this.timer1.Stop();

            // Displays a message box asking whether to continue running the timer.
            if (MessageBox.Show("Continue running?", "Count is: ",
               MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Restarts the timer and increments the counter.
                timer1.Enabled = true;
            }
        }
        private void createLabel(bool x)
        {
            Label label = new()
            {
                Name = RandomString(16),
                Location = new Point(10 + 93 * labels.Count, 180),
                Text = x ? "x" : "y",
                TabIndex = 100,
                Size = new Size(90, 15),
            };
            label.Text += (labels.Count/2).ToString() + ":";
            Controls.Add(label);
            labels.Add(label);
        }
        private void clearLabel()
        {
            foreach (var label in labels)
            {
                Controls.Remove(label);
            }
            labels.Clear();
        }
    }
}