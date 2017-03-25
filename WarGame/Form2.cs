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
    public partial class Form2 : Form
    {
        private Form1 Context { get; set; }
        public string PlayerName { get; set; }
        public string CPUName { get; set; }
        public int RoundCount { get; set; }

        public Form2(Form1 context)
        {
            this.Context = context;
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(OnClosing);
        }
        private void OnClosing(Object sender, FormClosingEventArgs args)
        {
            var dialogResult = showCloseDialog();
            args.Cancel = (dialogResult == DialogResult.No);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ValidateDataForm())
            {
                Context.CreateNewGame(PlayerName, CPUName, RoundCount);
                Context.Visible = true;
                Dispose();
            }
        }

        private DialogResult showCloseDialog()
        {
            return MessageBox.Show("Close app?", "Closing", MessageBoxButtons.YesNo);
        }

        private bool ValidateDataForm()
        {
            if (textBox4.Text.Count() <= 0 ||
                textBox5.Text.Count() <= 0 ||
                textBox6.Text.Count() <= 0)
            {
                MessageBox.Show("Wypełnij wszystkie pola");
                return false;
            }

            if (!IsDigitsOnly(textBox6.Text.ToString()))
            {
                MessageBox.Show("Wprowadź poprawne dane");
                return false;
            }

            PlayerName = textBox4.Text.ToString();
            CPUName = textBox5.Text.ToString();
            try
            {
                RoundCount = Int32.Parse(textBox6.Text.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                RoundCount = 3;
            }

            return true;
        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}
