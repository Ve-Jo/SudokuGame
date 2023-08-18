using System.Windows.Forms;

namespace SudokuGame
{
    public partial class NormalGame : Form
    {
        private MainMenu MainMenu;
        private int[,] puzzle;
        private int[,] finishedPuzzle;
        private int difficulty = 7;
        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
        private int currentSeconds = 0;
        private int numbersChanged = 0;
        private int checksPressed = 0;
        public NormalGame(MainMenu mainMenu)
        {
            InitializeComponent();
            UpdateTexts();
            MainMenu = mainMenu;
        }

        private void UpdateTexts()
        {
            this.Text = Resources.NormalGameForm;
            Close.Text = Resources.EndBtn;
            button1.Text = Resources.CheckBtn;
        }
        public void SetDifficulty(int newDifficulty)
        {
            difficulty = newDifficulty;
            SudokuUtility.GeneratePuzzle(ref puzzle, ref finishedPuzzle, difficulty);
        }

        private void GenerateGrid()
        {
            tableLayoutPanel.Location = new Point(10, 65);
            tableLayoutPanel.Size = new Size(360, 360);
            tableLayoutPanel.Padding = new Padding(0);
            tableLayoutPanel.Margin = new Padding(0);
            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();

            for (int i = 0; i < 9; i++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            }

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Text = puzzle[row, col] == 0 ? "" : puzzle[row, col].ToString();
                    textBox.Dock = DockStyle.Fill;
                    textBox.TextAlign = HorizontalAlignment.Center;
                    textBox.TextChanged += TextBox_TextChanged;
                    textBox.Margin = new Padding(0);
                    textBox.Padding = new Padding(0);
                    textBox.Font = new Font(textBox.Font.FontFamily, 12f);

                    tableLayoutPanel.Controls.Add(textBox, col, row);
                }
            }

            Controls.Add(tableLayoutPanel);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SudokuUtility.GeneratePuzzle(ref puzzle, ref finishedPuzzle, difficulty);
            GenerateGrid();
        }


        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int row = tableLayoutPanel.GetRow(textBox);
            int col = tableLayoutPanel.GetColumn(textBox);

            if (int.TryParse(textBox.Text, out int value))
            {
                string sanitizedInput = value.ToString();
                if (sanitizedInput.Length > 0 && sanitizedInput[0] != ' ')
                {
                    int sanitizedValue = int.Parse(sanitizedInput);
                    if (sanitizedValue >= 1 && sanitizedValue <= 9)
                    {
                        if (puzzle[row, col] == 0)
                        {
                            numbersChanged++;
                            toolStripStatusLabel1.Text = string.Format(Resources.ChangesText, numbersChanged);
                        }
                        puzzle[row, col] = sanitizedValue;
                        textBox.Text = sanitizedInput;
                        textBox.Parent.Focus();
                    }
                    else
                    {
                        puzzle[row, col] = 0;
                        textBox.Text = " ";
                    }
                }
                else
                {
                    puzzle[row, col] = 0;
                    textBox.Text = " ";
                }
            }
            else
            {
                puzzle[row, col] = 0;
                textBox.Text = " ";
            }
        }

        private bool IsCellValid(int row, int col)
        {
            int value = puzzle[row, col];
            if (value < 1 || value > 9)
            {
                return false;
            }

            // Check row, column, and 3x3 subgrid for duplicates
            for (int i = 0; i < 9; i++)
            {
                if (i != col && puzzle[row, i] == value)
                {
                    return false; // Duplicate value in the same row
                }
                if (i != row && puzzle[i, col] == value)
                {
                    return false; // Duplicate value in the same column
                }
                int subgridStartRow = 3 * (row / 3);
                int subgridStartCol = 3 * (col / 3);
                if ((subgridStartRow + i / 3 != row || subgridStartCol + i % 3 != col) && puzzle[subgridStartRow + i / 3, subgridStartCol + i % 3] == value)
                {
                    return false; // Duplicate value in the same 3x3 subgrid
                }
            }
            return true;
        }
        public bool IsWin()
        {
            for (int row = 0; row < puzzle.GetLength(0); row++)
            {
                for (int col = 0; col < puzzle.GetLength(1); col++)
                {
                    if (!IsCellValid(row, col))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void HighlightIncorrectTextBoxes()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    TextBox textBox = tableLayoutPanel.GetControlFromPosition(col, row) as TextBox;
                    if (textBox != null)
                    {
                        if (puzzle[row, col] != finishedPuzzle[row, col])
                        {
                            textBox.BackColor = Color.Red;
                        }
                        else
                        {
                            textBox.BackColor = SystemColors.Window;
                        }
                    }
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            checksPressed++;
            toolStripStatusLabel3.Text = string.Format(Resources.ChecksText, checksPressed);
            HighlightIncorrectTextBoxes();

            if (IsWin())
            {
                MessageBox.Show(Resources.Win);
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            MainMenu.Show();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            currentSeconds++;
            toolStripStatusLabel2.Text = string.Format(Resources.SecondsTimer, currentSeconds);
        }
    }
}
