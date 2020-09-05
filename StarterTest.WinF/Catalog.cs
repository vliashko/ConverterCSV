using StarterTest.BL;
using StarterTest.BL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace StarterTest.WinF
{
    public partial class Catalog : Form
    {
        readonly DBContext db = new DBContext();
        readonly DbSet<User> set;
        public Catalog()
        {
            InitializeComponent();
            set = db.Users;
            LoadDbDate();
        }
        void импортToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<User> users = GetDateFromCsv();
            LoadDateToDatabase(users);
            LoadDbDate();
        }
        void добавитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormAddOrChangeUser();

            if (form.ShowDialog() == DialogResult.OK)
            {
                var user = form.User;
                user.Id = dataGridView.Rows.Count;
                db.Users.Add(user);
                db.SaveChanges();
                dataGridView.Refresh();
            }
        }
        void изменитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = dataGridView.CurrentCell.RowIndex + 1;

            if(set.Find(index) is User user)
            {
                var form = new FormAddOrChangeUser(user);

                if(form.ShowDialog() == DialogResult.OK)
                {
                    _ = form.User;
                    db.SaveChanges();
                    dataGridView.Refresh();
                }
            }
        }
        void удалитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = dataGridView.CurrentCell.RowIndex;

            dataGridView.Rows.RemoveAt(index);

            db.SaveChanges();
            dataGridView.Refresh();
        }
        void экспортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ChooseDateToExport();
            form.ShowDialog();
            if (form.isExcel)
                ExportToExcel(form.User);
            else
                ExportToXml(form.User);
        }
        async void ExportToExcel(User user)
        {
            string FileName;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel файлы (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*",
                Title = "Экспорт в Excel"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = saveFileDialog.FileName;
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkBook = xlApp.Workbooks.Add();
                Excel.Worksheet xlWorkSheet = xlWorkBook.ActiveSheet;

                List<User> users = GetNecessaryData(user);

                List<string[]> excelUsers = users.Select(x => ConventerUser(x)).ToList();

                for (int i = 1; i < excelUsers.Count + 1; i++)
                {
                    for (int j = 1; j < 7; j++)
                    {
                        xlWorkSheet.Rows[i].Columns[j] = excelUsers[i - 1][j - 1];
                        await Task.Delay(50);
                    }
                }

                xlApp.AlertBeforeOverwriting = false;
                xlWorkBook.SaveAs(FileName);
                xlApp.Quit();

                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlApp);

                MessageBox.Show("Данные были успешно добавлены.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Была обнаружена ошибка " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        List<User> GetDateFromCsv()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = @"C:\Users\User\Desktop";
            openFileDialog.Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
            openFileDialog.Title = "Импорт файла .csv";

            List<User> date = new List<User>();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] line = sr.ReadLine().Split(';');
                        var user = new User(line[0], line[1], line[2], line[3], line[4], line[5]);
                        date.Add(user);
                    }
                }
            }
            return date;
        }
        void LoadDateToDatabase(List<User> users)
        {
            foreach (User user in users)
            {
                db.Set<User>().Add(user);
            }
            db.SaveChanges();
            MessageBox.Show("Записи были добавлены в базу данных.\nОшибок не обнаружено.",
                "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        void LoadDbDate()
        {
            set.Load();
            dataGridView.DataSource = set.Local.ToBindingList();
            dataGridView.Sort(dataGridView.Columns["Id"], ListSortDirection.Ascending);
        }
        async void ExportToXml(User user)
        {
            string FileName;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Xml файлы (*.xml)|*.xml|Все файлы (*.*)|*.*",
                Title = "Экспорт в Excel"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = saveFileDialog.FileName;
                List<User> users = GetNecessaryData(user);

                File.AppendAllText(FileName, "<TestProgram>");

                foreach (var xUser in users)
                {
                    File.AppendAllText(FileName, $"\n\t<Record id=\"{xUser.Id}\">");
                    File.AppendAllText(FileName, $"\n\t\t<Date>{xUser.DateTime:dd.MM.yy}</Date>");
                    File.AppendAllText(FileName, $"\n\t\t<FirstName>{xUser.Name}</FirstName>");
                    File.AppendAllText(FileName, $"\n\t\t<LastName>{xUser.Surname}</LastName>");
                    File.AppendAllText(FileName, $"\n\t\t<SurName>{xUser.MiddleName}</SurName>");
                    File.AppendAllText(FileName, $"\n\t\t<City>{xUser.City}</City>");
                    File.AppendAllText(FileName, $"\n\t\t<Country>{xUser.Country}</Country>");
                    File.AppendAllText(FileName, $"\n\t</Record>");
                    await Task.Delay(50);
                }

                File.AppendAllText(FileName, "\n</TestProgram>");

                MessageBox.Show("Данные были успешно добавлены.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private List<User> GetNecessaryData(User user)
        {
            set.Load();
            List<User> users = set.ToList();

            if (user.Name != null)
                users = users.Where(x => x.Name == user.Name).ToList();
            if (user.Surname != null)
                users = users.Where(x => x.Surname == user.Surname).ToList();
            if (user.MiddleName != null)
                users = users.Where(x => x.MiddleName == user.MiddleName).ToList();
            if (user.DateTime != DateTime.MinValue)
                users = users.Where(x => x.DateTime == user.DateTime).ToList();
            if (user.City != null)
                users = users.Where(x => x.City == user.City).ToList();
            if (user.Country != null)
                users = users.Where(x => x.Country == user.Country).ToList();
            return users;
        }
        string[] ConventerUser(User user)
        {
            string[] result = { user.DateTime.ToString("dd.MM.yy"), user.Name, user.Surname, user.MiddleName, 
                user.City, user.Country };
            return result;
        }
    }
}
