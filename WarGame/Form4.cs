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
        private IList<RoundState> roundsStates;

        public Form4(List<RoundState> roundsStates)
        {
            this.roundsStates = roundsStates;

            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            DrawChart();
        }

        private void DrawChart()
        {
            foreach (var roundState in roundsStates)
            {
                chart1.Series["Player"].Points.AddXY(roundState.RoundNumber, roundState.PlayerPoints);
                chart1.Series["CPU"].Points.AddXY(roundState.RoundNumber, roundState.CPUPoints);
            }
        }

        private IList<Highscore> GetHighscoresFromFile()
        {
            var highscoresFilePath = System.Environment.CurrentDirectory + @"\highscores.hscrs";
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
