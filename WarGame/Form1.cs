using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarGame
{
    public partial class Form1 : Form
    {
        private int Rounds = 1;
        private int MaxRounds;
        private int Sets;
        private int PlayerPoints = 0;
        private int CPUPoints = 0;
        private string PlayerName;
        private string CPUName;
        private bool AutoGameplay = false;
        private int Deck;
        private int DrawsInRow = 0;
        private bool War = false;

        public Form1()
        {
            InitializeComponent();

            Form2 form2 = new Form2(this);
            form2.Show();

            Shown += new EventHandler(Form1_Shown);

            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);
            FormClosing += new FormClosingEventHandler(OnClosing);
            tableLayoutPanel4.BorderStyle = BorderStyle.FixedSingle;

        }

        private void OnClosing(Object sender, FormClosingEventArgs args)
        {
            var dialogResult = showCloseDialog();
            args.Cancel = (dialogResult == DialogResult.No);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialogResult = showCloseDialog();
            if (dialogResult == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Short info about app");
        }

        public void CreateNewGame(string playerName, string cpuName, int roundsCount, int sets)
        {
            Rounds = 1;
            PlayerPoints = 0;
            CPUPoints = 0;
            button4.Text = "";
            button3.Text = "";
            button4.BackColor = Color.Gray;
            button3.BackColor = Color.Gray;
            button4.BackgroundImage = null;
            button3.BackgroundImage = null;

            MaxRounds = roundsCount;
            PlayerName = playerName;
            CPUName = cpuName;
            Sets = sets;
            Deck = 10 * sets;
            PlayerPoints = Deck / 2;
            CPUPoints = Deck / 2;

            label9.Text = cpuName;
            label1.Text = playerName;

            label2.Text = $"{playerName} points";
            label3.Text = PlayerPoints.ToString();

            label7.Text = $"{cpuName} points";
            label8.Text = CPUPoints.ToString();

            label5.Text = $"Round: {Rounds} of {MaxRounds}";
        }

        public void CreateExitDialog()
        {
            Form form1 = new Form();
            Button button1 = new Button();
            Button button2 = new Button();

            button1.Text = "OK";
            button1.Location = new Point(10, 10);
            button2.Text = "Cancel";
            button2.Location
               = new Point(button1.Left, button1.Height + button1.Top + 10);
            button1.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.Cancel;
            form1.Text = "My Dialog Box";
            form1.FormBorderStyle = FormBorderStyle.FixedDialog;
            form1.AcceptButton = button1;
            form1.CancelButton = button2;
            form1.StartPosition = FormStartPosition.CenterScreen;

            form1.Controls.Add(button1);
            form1.Controls.Add(button2);

            form1.ShowDialog();

            if (form1.DialogResult == DialogResult.OK)
            {
                Application.Exit();
            }
            else
            {
                form1.Dispose();
            }
        }

        //player button
        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Draws " + DrawsInRow);
            if (Rounds > MaxRounds || PlayerPoints <= 0 || CPUPoints <= 0)
            {
                string result = "";
                if (PlayerPoints > CPUPoints)
                    result = "Win!";
                else if (PlayerPoints < CPUPoints)
                    result = "Defeat!";
                else result = "Draw!";
                
                var highscoresLine = $"{PlayerName},{PlayerPoints},{Rounds}";
                var highscoresFilePath = System.Environment.CurrentDirectory + @"\highscores.hscrs";
                File.AppendAllText(highscoresFilePath, highscoresLine);


                DialogResult dialogResult = MessageBox.Show($"{result}. Wanna play again?", "Finish", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    CreateNewGame(PlayerName, CPUName, MaxRounds, Sets);
                    return;
                }
                else if (dialogResult == DialogResult.No)
                {
                    var secondDialogResult = showCloseDialog();
                    if (secondDialogResult == DialogResult.OK)
                    {
                        Application.Exit();
                    }
                }
            }
            label5.Text = $"Round: {Rounds} of {MaxRounds}";

            Random rnd = new Random();
            int playerRandomNumber = rnd.Next(1, 11);
            int cpuRandomNumber = rnd.Next(1, 11);

            drawNumberOnButton(button3, playerRandomNumber);
            drawNumberOnButton(button4, cpuRandomNumber);
            
            PlayerPoints--;
            CPUPoints--;
            if (War)
            {
                button3.BackgroundImage = Properties.Resources.back;
                button3.BackgroundImageLayout = ImageLayout.Stretch;
                button4.BackgroundImage = Properties.Resources.back;
                button4.BackgroundImageLayout = ImageLayout.Stretch;
                War = false;
            }
            else
            {
                if (playerRandomNumber == cpuRandomNumber)
                {
                    DrawsInRow++;
                    War = true;
                    button3.BackColor = Color.Orange;
                    button4.BackColor = Color.Orange;
                }
                else if (playerRandomNumber > cpuRandomNumber)
                {
                    PlayerPoints += (DrawsInRow + 1) * 2;
                    War = false;
                    button3.BackColor = Color.Green;
                    button4.BackColor = Color.Red;
                    DrawsInRow = 0;
                }
                else
                {
                    CPUPoints += (DrawsInRow + 1) * 2;
                    War = false;
                    button3.BackColor = Color.Red;
                    button4.BackColor = Color.Green;
                    DrawsInRow = 0;
                }
            }

            label3.Text = PlayerPoints.ToString();
            label8.Text = CPUPoints.ToString();

            Rounds++;
        }

        //cpu button
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void newGameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 newGameForm = new Form2(this);
            newGameForm.Show();
        }

        private DialogResult showCloseDialog()
        {
            return MessageBox.Show("Close app?", "Closing", MessageBoxButtons.YesNo);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AutoGameplay = !AutoGameplay;
            if (AutoGameplay)
                (sender as Button).Text = "Stop";
            else (sender as Button).Text = "Start";
        }

        private void drawNumberOnButton(Button button, int index)
        {
            Rectangle r = new Rectangle(100 * (index - 1), 0, 100, 147);
            Bitmap src = Properties.Resources.numerki;
            Bitmap target = new Bitmap(r.Width, r.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), r, GraphicsUnit.Pixel);
            }

            button.BackgroundImage = target;
            button.BackgroundImageLayout = ImageLayout.Stretch;
        }
    }
}
