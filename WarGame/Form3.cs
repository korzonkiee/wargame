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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            listView1.BorderStyle = BorderStyle.FixedSingle;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            PopulateListView();
        }

        private void PopulateListView()
        {
            var listViewItems = new List<ListViewItem>();
            var highscores = GetHighscoresFromFile();
            var sortedHighscores = highscores
                .OrderByDescending(o => o.Score)
                .Select(x => new Highscore()
                {
                    PlayerName = x.PlayerName,
                    Score = x.Score,
                    RoundsPlayed = x.RoundsPlayed
                });
                

            foreach (var highscore in sortedHighscores)
            {
                var listViewItem = new ListViewItem(highscore.PlayerName);
                listViewItem.SubItems.Add(highscore.Score);
                listViewItem.SubItems.Add(highscore.RoundsPlayed);

                listViewItems.Add(listViewItem);
            }

            listView1.Items.AddRange(listViewItems.ToArray());
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

    public class Highscore
    {
        public string PlayerName { get; set; }
        public string Score { get; set; }
        public string RoundsPlayed { get; set; }

        public static Highscore CreateFromStringData(string data)
        {
            if (!String.IsNullOrEmpty(data))
            {
                var fields = data.Split(',');
                var highscore = new Highscore()
                {
                    PlayerName = fields[0],
                    Score = fields[1],
                    RoundsPlayed = fields[2]
                };

                return highscore;
            }

            return null;
        }
    }
}
