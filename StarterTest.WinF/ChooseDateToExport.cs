using StarterTest.BL.Model;
using System;
using System.Windows.Forms;

namespace StarterTest.WinF
{
    public partial class ChooseDateToExport : Form
    {
        public int ChooseExportStyle { get; set; }
        public User User { get; set; }
        public ChooseDateToExport()
        {
            InitializeComponent();
        }

        void button1_Click(object sender, EventArgs e)
        {
            ButtonClick();
            ChooseExportStyle = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ButtonClick();
            ChooseExportStyle = 2;
        }


        void ButtonClick()
        {
            var u = new User();
            if (textBox2.Text != "")
                u.Name = textBox2.Text;
            else
                u.Name = null;
            if (textBox5.Text != "")
                u.Surname = textBox5.Text;
            else
                u.Surname = null;
            if (textBox4.Text != "")
                u.MiddleName = textBox4.Text;
            else
                u.MiddleName = null;
            if (String.IsNullOrEmpty(maskedTextBox1.Text) || String.IsNullOrWhiteSpace(maskedTextBox1.Text))
                u.DateTime = DateTime.Parse(maskedTextBox1.Text);
            else
                u.DateTime = DateTime.MinValue;
            if (textBox3.Text != "")
                u.City = textBox3.Text;
            else
                u.City = null;
            if (textBox6.Text != "")
                u.Country = textBox6.Text;
            else
                u.Country = null;


            User = u;
            Close();
        }
        void textBoxLetter_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != '\b')
            {
                e.Handled = true;
            }
        }

        void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
    }
}
