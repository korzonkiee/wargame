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
    public partial class Form4 : Form
    {
        private GameState gameState;
        private Form context;

        public Form4(Form context, GameState gameState)
        {
            this.gameState = gameState;
            this.context = context;
            this.Owner = context;
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            FormClosing += new FormClosingEventHandler(OnClosing);
            DrawChart();
        }

        private void OnClosing(Object sender, FormClosingEventArgs args)
        {
            var dialogResult = showPlayAgainDialog();
            if (dialogResult == DialogResult.Yes)
            {
                (context as Form1).CreateNewGame(gameState.PlayerName, gameState.CPUName, gameState.MaxRounds, gameState.Sets);
            }

        }

        private DialogResult showCloseDialog()
        {
            return MessageBox.Show("Close app?", "Closing", MessageBoxButtons.YesNo);
        }

        private DialogResult showPlayAgainDialog()
        {
            return MessageBox.Show("Play again?", "Again", MessageBoxButtons.YesNo);
        }

        private void DrawChart()
        {
            foreach (var roundState in gameState.RoundsStates)
            {
                chart1.Series["Player"].Points.AddXY(roundState.RoundNumber, roundState.PlayerPoints);
                chart1.Series["CPU"].Points.AddXY(roundState.RoundNumber, roundState.CPUPoints);
            }
        }

        private IList<Highscore> GetHighscoresFromFile()
        {
            var highscoresFilePath = System.Environment.CurrentDirectory + @"\highscores.hs";
            var highscores = new List<Highscore>();

            var content = File.ReadAllText(highscoresFilePath);
            content = content.Remove(content.Length - 1);

            var rows = content.Split(';');
            foreach (var row in rows)
            {
                var highscore = Highscore.CreateFromStringData(row);
                highscores.Add(highscore);
            }

            return highscores;
        }
    }
}
