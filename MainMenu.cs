using System.ComponentModel;
using System.Resources;
using System.Globalization;
using System.Reflection.Emit;

namespace SudokuGame
{
    public partial class MainMenu : Form
    {
        private CultureInfo currentCulture;
        private NormalGame normalGameForm;
        int selectedDifficulty = 7;
        ToolStripDropDownButton languageDropdown = new ToolStripDropDownButton();
        public MainMenu()
        {
            InitializeComponent();
            InitializeLocalization();
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            this.Text = Resources.MenuForm;
            label1.Text = Resources.Title;
            label2.Text = Resources.Welcome;
            button1.Text = Resources.GameStart;
            button2.Text = Resources.AutomodeStart;
            languageDropdown.Text = Resources.LanguageSelector;
            label3.Text = string.Format(Resources.Difficulty, selectedDifficulty);
            toolStripStatusLabel1.Text = Resources.AboutAuthor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            normalGameForm = new NormalGame(this);
            int difficulty = trackBar1.Value;
            normalGameForm.SetDifficulty(difficulty);
            normalGameForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SolverMode solverModeForm = new SolverMode(this);
            solverModeForm.Show();
            this.Hide();
        }

        private void InitializeLocalization()
        {
            languageDropdown.Text = Resources.LanguageSelector;
            ToolStripMenuItem englishItem = new ToolStripMenuItem("English");
            englishItem.Click += (sender, e) => ChangeLanguage("en-US");
            ToolStripMenuItem ukrainianItem = new ToolStripMenuItem("Українська");
            ukrainianItem.Click += (sender, e) => ChangeLanguage("uk-UA");
            ToolStripMenuItem russianItem = new ToolStripMenuItem("Русский");
            russianItem.Click += (sender, e) => ChangeLanguage("ru-RU");
            languageDropdown.DropDownItems.Add(englishItem);
            languageDropdown.DropDownItems.Add(ukrainianItem);
            languageDropdown.DropDownItems.Add(russianItem);
            statusStrip1.Items.Add(languageDropdown);
        }

        private void ChangeLanguage(string cultureCode)
        {
            CultureInfo newCulture = CultureInfo.GetCultureInfo(cultureCode);
            Thread.CurrentThread.CurrentUICulture = newCulture;
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainMenu));
            ApplyResources(resources, Controls);
            UpdateTexts();
        }

        private void ApplyResources(ComponentResourceManager resources, Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                resources.ApplyResources(control, control.Name);
                ApplyResources(resources, control.Controls);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            selectedDifficulty = trackBar1.Value;
            label3.Text = string.Format(Resources.Difficulty, selectedDifficulty);
            float fontSize = selectedDifficulty * 1f + 8;
            Font newFont = new Font(label3.Font.FontFamily, fontSize, label3.Font.Style);
            label3.Font = newFont;
            label3.Location = new Point(125 - (selectedDifficulty - 7) * 5, label3.Location.Y);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.AboutAuthorText);
        }
    }
}