using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SudokuGame
{
    public partial class SolverMode : Form
    {
        private MainMenu MainMenu;
        private int[,] puzzle = new int[9, 9];
        private int[,] finishedPuzzle;
        private int selectedDifficulty = 7;
        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
        public SolverMode(MainMenu mainMenu)
        {
            InitializeComponent();
            UpdateTexts();
            GenerateEmptyPuzzle();
            GenerateGrid();
            MainMenu = mainMenu;
        }

        private void UpdateTexts()
        {
            this.Text = Resources.SolverModeForm;
            button1.Text = Resources.SolveBtn;
            button2.Text = Resources.EndBtn;
            button3.Text = Resources.GenerateBtn;
            label1.Text = string.Format(Resources.Difficulty, selectedDifficulty);
        }

        private void GenerateEmptyPuzzle()
        {
            puzzle = new int[9, 9];
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

        private void UpdateGridWithSolution()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    TextBox textBox = tableLayoutPanel.GetControlFromPosition(col, row) as TextBox;
                    if (textBox != null)
                    {
                        textBox.Text = puzzle[row, col].ToString();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!SudokuUtility.IsInvalidConfiguration(puzzle))
            {
                SudokuUtility.SolvePuzzle(puzzle);
                UpdateGridWithSolution();
            }
            else
            {
                MessageBox.Show(Resources.Unsolvable);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainMenu.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SudokuUtility.GeneratePuzzle(ref puzzle, ref finishedPuzzle, selectedDifficulty);
            UpdateGridWithSolution();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            selectedDifficulty = trackBar1.Value;
            label1.Text = string.Format(Resources.Difficulty, selectedDifficulty);
        }
    }
}