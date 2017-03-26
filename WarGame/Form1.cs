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
        private GameState CurrentGameState;

        private static Timer timer;

        public Form1()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 2000;
            timer.Tick += new EventHandler(TimerEventProcessor);

            Form2 form2 = new Form2(this);
            form2.Show();

            Shown += new EventHandler(Form1_Shown);

            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);
            FormClosing += new FormClosingEventHandler(OnClosing);
            tableLayoutPanel4.BorderStyle = BorderStyle.FixedSingle;

        }

        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            if (CurrentGameState.CurrentRound > CurrentGameState.MaxRounds ||
                CurrentGameState.CurrentPlayerPoints <= 0 || CurrentGameState.CurrentCPUPoints <= 0)
                return;
            button2.PerformClick();
        }

        private void OnClosing(Object sender, FormClosingEventArgs args)
        {
            var dialogResult = showCloseDialog();
            if (dialogResult == DialogResult.Yes)
            {
                timer.Tick -= TimerEventProcessor;
            }
            else
            {
                args.Cancel = true;
            }
            
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
                timer.Tick -= TimerEventProcessor;
                Application.Exit();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Short info about app");
        }

        public void CreateNewGame(string playerName, string cpuName, int roundsCount, int sets)
        {
            CurrentGameState = new GameState()
            {
                MaxRounds = roundsCount,
                Sets = sets,
                CurrentPlayerPoints = (10 * sets) / 2,
                CurrentCPUPoints = (10 * sets) / 2,
                PlayerName = playerName,
                CPUName = cpuName,
                Deck = 10 * sets,
                CurrentRound = 1,
                AutoGameplay = false,
                DrawsInRow = 0,
                War = false,
                RoundsStates = new List<RoundState>()
            };

            button4.Text = "";
            button3.Text = "";
            button4.BackColor = Color.Gray;
            button3.BackColor = Color.Gray;
            button4.BackgroundImage = null;
            button3.BackgroundImage = null;

            button5.Text = "Start";
            timer.Stop();

            label9.Text = CurrentGameState.CPUName;
            label1.Text = CurrentGameState.PlayerName;

            label2.Text = $"{CurrentGameState.PlayerName} points";
            label3.Text = CurrentGameState.CurrentPlayerPoints.ToString();

            label7.Text = $"{CurrentGameState.CPUName} points";
            label8.Text = CurrentGameState.CurrentCPUPoints.ToString();

            label5.Text = $"Round: 1 of {CurrentGameState.MaxRounds}";
        }

        //player button
        private void button2_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int playerRandomNumber = rnd.Next(1, 11);
            int cpuRandomNumber = rnd.Next(1, 11);

            label5.Text = $"Round: {CurrentGameState.CurrentRound} of {CurrentGameState.MaxRounds}";

            drawNumberOnButton(button3, playerRandomNumber);
            drawNumberOnButton(button4, cpuRandomNumber);
            
            CurrentGameState.CurrentPlayerPoints--;
            CurrentGameState.CurrentCPUPoints--;
            if (CurrentGameState.War)
            {
                button3.BackgroundImage = Properties.Resources.back;
                button3.BackgroundImageLayout = ImageLayout.Stretch;
                button4.BackgroundImage = Properties.Resources.back;
                button4.BackgroundImageLayout = ImageLayout.Stretch;
                CurrentGameState.War = false;
            }
            else
            {
                if (playerRandomNumber == cpuRandomNumber)
                {
                    CurrentGameState.DrawsInRow++;
                    CurrentGameState.War = true;
                    button3.BackColor = Color.Orange;
                    button4.BackColor = Color.Orange;
                }
                else if (playerRandomNumber > cpuRandomNumber)
                {
                    CurrentGameState.CurrentPlayerPoints += (CurrentGameState.DrawsInRow + 1) * 2;
                    CurrentGameState.War = false;
                    button3.BackColor = Color.Green;
                    button4.BackColor = Color.Red;
                    CurrentGameState.DrawsInRow = 0;
                }
                else
                {
                    CurrentGameState.CurrentCPUPoints += (CurrentGameState.DrawsInRow + 1) * 2;
                    CurrentGameState.War = false;
                    button3.BackColor = Color.Red;
                    button4.BackColor = Color.Green;
                    CurrentGameState.DrawsInRow = 0;
                }
            }

            label3.Text = CurrentGameState.CurrentPlayerPoints.ToString();
            label8.Text = CurrentGameState.CurrentCPUPoints.ToString();


            var roundState = new RoundState()
            {
                CPUPoints = CurrentGameState.CurrentCPUPoints,
                PlayerPoints = CurrentGameState.CurrentPlayerPoints,
                RoundNumber = CurrentGameState.CurrentRound
            };

            CurrentGameState.RoundsStates.Add(roundState);
            CurrentGameState.CurrentRound++;

            if (CurrentGameState.CurrentRound > CurrentGameState.MaxRounds ||
                CurrentGameState.CurrentPlayerPoints <= 0 ||
                CurrentGameState.CurrentCPUPoints <= 0)
            {
                string result = "";
                if (CurrentGameState.CurrentPlayerPoints > CurrentGameState.CurrentCPUPoints)
                    result = "Win!";
                else if (CurrentGameState.CurrentPlayerPoints < CurrentGameState.CurrentCPUPoints)
                    result = "Defeat!";
                else result = "Draw!";

                var highscoresLine = $"{CurrentGameState.PlayerName},{CurrentGameState.CurrentPlayerPoints},{CurrentGameState.CurrentRound - 1};";
                var highscoresFilePath = System.Environment.CurrentDirectory + @"\highscores.hscrs";
                File.AppendAllText(highscoresFilePath, highscoresLine);

                DialogResult dialogResult1 = MessageBox.Show($"{result}! Show postgame stats?", "Game over", MessageBoxButtons.YesNo);
                if(dialogResult1 == DialogResult.Yes)
                {
                    var chart = new Form4(CurrentGameState.RoundsStates);
                    chart.Show();
                } else
                {
                    DialogResult dialogResult2 = MessageBox.Show($"{result}. Wanna play again?", "Finish", MessageBoxButtons.YesNo);
                    if (dialogResult2 == DialogResult.Yes)
                    {
                        CreateNewGame(CurrentGameState.PlayerName, CurrentGameState.CPUName, CurrentGameState.MaxRounds, CurrentGameState.Sets);
                        return;
                    }
                    else if (dialogResult2 == DialogResult.No)
                    {
                        var secondDialogResult = showCloseDialog();
                        if (secondDialogResult == DialogResult.Yes)
                        {
                            timer.Tick -= TimerEventProcessor;
                            Application.Exit();
                        }
                    }
                }
            }
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
            CurrentGameState.AutoGameplay = !CurrentGameState.AutoGameplay;
            if (CurrentGameState.AutoGameplay)
            {
                timer.Start();
                (sender as Button).Text = "Stop";
            }
            else
            {
                timer.Stop();
                (sender as Button).Text = "Start";
            }
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

        private void highscoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 highscores = new Form3();
            highscores.Show();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            timer.Interval = (trackBar1.Maximum - trackBar1.Value) * 10 + 1;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JsonConvert
        }
    }

    class GameState
    {
        public int MaxRounds { get; set; }
        public int CurrentRound { get; set; }
        public int CurrentPlayerPoints { get; set; }
        public int CurrentCPUPoints { get; set; }
        public int Sets { get; set; }
        public string PlayerName { get; set; }
        public string CPUName { get; set; }
        public int Deck { get; set; }
        public bool AutoGameplay { get; set; }
        public int DrawsInRow { get; set; }
        public bool War { get; set; }

        public List<RoundState> RoundsStates { get; set; }
    }

    public class RoundState
    {
        public int RoundNumber { get; set; }
        public int PlayerPoints { get; set; }
        public int CPUPoints { get; set; }
    }
}
