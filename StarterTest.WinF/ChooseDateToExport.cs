using StarterTest.BL.Model;
using System;
using System.Windows.Forms;

namespace StarterTest.WinF
{
    public partial class ChooseDateToExport : Form
    {
        public bool isExcel { get; set; }
        public User User { get; set; }
        public ChooseDateToExport()
        {
            InitializeComponent();
        }

        void button1_Click(object sender, EventArgs e)
        {
            ButtonClick();
            isExcel = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ButtonClick();
            isExcel = false;
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
            if (textBox1.Text != "")
                u.DateTime = TryParseDate(textBox1);
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
        DateTime TryParseDate(TextBox textBox)
        {
            bool k = DateTime.TryParse(textBox.Text, out DateTime date);
            if(!k)
            {
                date = DateTime.MinValue;
            }
            return date;
        }
    }
}
