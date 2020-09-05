using StarterTest.BL.Model;
using System;
using System.Windows.Forms;

namespace StarterTest.WinF
{
    public partial class FormAddOrChangeUser : Form
    {
        public User User { get; set; }
        public FormAddOrChangeUser()
        {
            InitializeComponent();
        }

        public FormAddOrChangeUser(User user) : this()
        {
            User = user;
            textBox1.Text = user.DateTime.ToString("dd.MM.yy");
            textBox2.Text = user.Name;
            textBox5.Text = user.Surname;
            textBox4.Text = user.MiddleName;
            textBox3.Text = user.City;
            textBox6.Text = user.Country;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var u = User ?? new User();

            u.Name = textBox2.Text;
            u.Surname = textBox5.Text;
            u.MiddleName = textBox4.Text;
            u.DateTime = DateTime.Parse(textBox1.Text);
            u.City = textBox3.Text;
            u.Country = textBox6.Text;

            User = u;

            Close();
        }
    }
}
