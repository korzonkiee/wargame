using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

            MaxRounds = roundsCount;
            PlayerName = playerName;
            CPUName = cpuName;

            label9.Text = cpuName;
            label1.Text = playerName;

            label2.Text = $"{playerName} points";
            label3.Text = "0";

            label7.Text = $"{cpuName} points";
            label8.Text = "0";

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
            if (Rounds > MaxRounds)
            {
                string result = "";
                if (PlayerPoints > CPUPoints)
                    result = "Win!";
                else if (PlayerPoints < CPUPoints)
                    result = "Defeat!";
                else result = "Draw!";

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
            int playerRandomNumber = rnd.Next(1, 10);
            int cpuRandomNumber = rnd.Next(1, 10);

            button4.Text = cpuRandomNumber.ToString();
            button3.Text = playerRandomNumber.ToString();

            if (playerRandomNumber == cpuRandomNumber)
            {
                Rounds++;
                button3.BackColor = Color.Orange;
                button4.BackColor = Color.Orange;
            }
            else if (playerRandomNumber > cpuRandomNumber)
            {
                Rounds++;
                PlayerPoints++;
                button3.BackColor = Color.Green;
                button4.BackColor = Color.Red;
            }
            else
            {
                Rounds++;
                CPUPoints++;
                button3.BackColor = Color.Red;
                button4.BackColor = Color.Green;
            }

            label3.Text = PlayerPoints.ToString();
            label8.Text = CPUPoints.ToString();
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
    }
}
