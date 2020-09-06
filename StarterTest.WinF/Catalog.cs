using StarterTest.BL;
using StarterTest.BL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
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
        }
        void отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDbDate();
            GetRusNameCols();
            работаСТаблицейToolStripMenuItem.Enabled = true;
            отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem.Enabled = false;
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
            var index = dataGridView.SelectedCells[0].RowIndex;
            int id = (int)dataGridView[0, index].Value;

            if(set.Find(id) is User user)
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
            var index = dataGridView.SelectedCells[0].RowIndex;
            if (MessageBox.Show("Вы точно хотите удалить эту запись?", "Удаление", 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                dataGridView.Rows.RemoveAt(index);
                db.SaveChanges();
                dataGridView.Refresh();
            }
        }
        void экспортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ChooseDateToExport();
            form.ShowDialog();
            if (form.IsExcel)
                ExportToExcel(form.User);
            else
                ExportToXml(form.User);
        }
        async void ExportToExcel(User searchCriterion)
        {
            string FileName;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*",
                Title = "Экспорт в Excel"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = saveFileDialog.FileName;
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkBook = xlApp.Workbooks.Add();
                Excel.Worksheet xlWorkSheet = xlWorkBook.ActiveSheet;

                List<User> users = GetNecessaryData(searchCriterion);

                List<string[]> excelUsers = users.Select(x => ConventerUser(x)).ToList();

                for (int i = 1; i < excelUsers.Count + 1; i++)
                {
                    for (int j = 1; j < 7; j++)
                    {
                        xlWorkSheet.Rows[i].Columns[j] = excelUsers[i - 1][j - 1];
                    }
                    await Task.Delay(20);
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
        async void ExportToXml(User searchCriterion)
        {
            string fileName;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|Все файлы (*.*)|*.*",
                Title = "Экспорт в XML"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;
                List<User> users = GetNecessaryData(searchCriterion);

                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    XmlTextWriter xmlOut = new XmlTextWriter(fs, Encoding.Unicode);

                    xmlOut.Formatting = Formatting.Indented;

                    xmlOut.WriteStartDocument();
                    xmlOut.WriteStartElement("TestProgram");

                    foreach (User xUser in users)
                    {
                        SaveToFile(xmlOut, xUser);
                        await Task.Delay(50);
                    }

                    xmlOut.WriteEndElement();
                    xmlOut.WriteEndDocument();
                    xmlOut.Close();

                    MessageBox.Show("Данные были успешно добавлены.", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        static void SaveToFile(XmlTextWriter xmlOut, User xUser)
        {
            xmlOut.WriteStartElement("Record");
            xmlOut.WriteAttributeString("id", xUser.Id.ToString());
            xmlOut.WriteElementString("Date", xUser.DateTime.ToString("dd.MM.yy"));
            xmlOut.WriteElementString("FirstName", xUser.Name);
            xmlOut.WriteElementString("LastName", xUser.Surname);
            xmlOut.WriteElementString("SurName", xUser.MiddleName);
            xmlOut.WriteElementString("City", xUser.City);
            xmlOut.WriteElementString("Country", xUser.Country);
            xmlOut.WriteEndElement();
        }
        List<User> GetNecessaryData(User searchCriterion)
        {
            set.Load();
            List<User> users = set.ToList();

            if (searchCriterion.Name != null)
                users = users.Where(x => x.Name == searchCriterion.Name).ToList();
            if (searchCriterion.Surname != null)
                users = users.Where(x => x.Surname == searchCriterion.Surname).ToList();
            if (searchCriterion.MiddleName != null)
                users = users.Where(x => x.MiddleName == searchCriterion.MiddleName).ToList();
            if (searchCriterion.DateTime != DateTime.MinValue)
                users = users.Where(x => x.DateTime == searchCriterion.DateTime).ToList();
            if (searchCriterion.City != null)
                users = users.Where(x => x.City == searchCriterion.City).ToList();
            if (searchCriterion.Country != null)
                users = users.Where(x => x.Country == searchCriterion.Country).ToList();
            return users;
        }
        string[] ConventerUser(User user)
        {
            string[] result = { user.DateTime.ToString("dd.MM.yy"), user.Name, user.Surname, user.MiddleName, 
                user.City, user.Country };
            return result;
        }
        void GetRusNameCols()
        {
            dataGridView.Columns[0].HeaderText = "АйДи";
            dataGridView.Columns[1].HeaderText = "Дата";
            dataGridView.Columns[2].HeaderText = "Имя";
            dataGridView.Columns[3].HeaderText = "Фамилия";
            dataGridView.Columns[4].HeaderText = "Отчество";
            dataGridView.Columns[5].HeaderText = "Город";
            dataGridView.Columns[6].HeaderText = "Страна";
        }
    }
}
