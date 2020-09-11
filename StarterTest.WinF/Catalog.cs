using Ganss.Excel;
using StarterTest.BL;
using StarterTest.BL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;


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
            Action LoadDb = new Action(LoadDbDate);
            LoadDb += GetRusNameCols;
            Thread thread = new Thread(new ThreadStart(LoadDb));
            thread.Start();
        }
        void импортToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GetDateFromCsv();
            MessageBox.Show("Данные были успешно добавлены.", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            GC.Collect();
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

            if (set.Find(id) is User user)
            {
                var form = new FormAddOrChangeUser(user);

                if (form.ShowDialog() == DialogResult.OK)
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
            if (form.ChooseExportStyle == 1)
                ExportToExcel(form.User);
            else if (form.ChooseExportStyle == 2)
                ExportToXml(form.User);
            
        }
        void ExportToExcel(User searchCriterion)
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

                List<User> users = GetNecessaryData(searchCriterion);
                if(users.Count > 1048576)
                {
                    MessageBox.Show("Данные не могут быть экспортированы в Excel.\nExcel не поддерживает такой большой объем данных.", 
                        "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ExcelMapper mapper = new ExcelMapper();

                mapper.Save(FileName, users, "Excel", true);

                MessageBox.Show("Данные были успешно добавлены.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        void GetDateFromCsv()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\Users\User\Desktop",
                Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*",
                Title = "Импорт файла .csv"
            };

            List<User> data = new List<User>();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                using (var reader = new StreamReader(fileName))
                {
                    string line;
                    int counter = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        counter++;
                        data.Add(new User(line));
                        if (counter % 18000 == 0)
                        {
                            ProcessLines(data);
                            data.Clear();
                        }
                    }
                    ProcessLines(data);
                    data.Clear();
                    //data = File.ReadAllLines(fileName).Select(line => new User(line)).ToList();
                }
            }
        }
        void LoadDbDate()
        {
            set.Load();
            dataGridView.DataSource = set.Local.ToBindingList();
            dataGridView.Sort(dataGridView.Columns["Id"], ListSortDirection.Ascending);

            работаСТаблицейToolStripMenuItem.Enabled = true;
            отобразитьДанныеможетЗанятьНекотороеВремяToolStripMenuItem.Enabled = false;
        }
        void ExportToXml(User searchCriterion)
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
                    using (XmlTextWriter xmlOut = new XmlTextWriter(fs, Encoding.Unicode))
                    {
                        xmlOut.Formatting = Formatting.Indented;

                        xmlOut.WriteStartDocument();
                        xmlOut.WriteStartElement("TestProgram");

                        foreach (User xUser in users)
                        {
                            SaveToFile(xmlOut, xUser);
                        }

                        xmlOut.WriteEndElement();
                        xmlOut.WriteEndDocument();

                        MessageBox.Show("Данные были успешно добавлены.", "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        users.Clear();
                    }
                }
            }
        }
        void SaveToFile(XmlTextWriter xmlOut, User xUser)
        {
            xmlOut.WriteStartElement("Record");
            xmlOut.WriteAttributeString("id", xUser.Id.ToString());
            xmlOut.WriteElementString("Date", xUser.DateTime.ToString("dd.MM.yyyy"));
            xmlOut.WriteElementString("FirstName", xUser.Name);
            xmlOut.WriteElementString("LastName", xUser.Surname);
            xmlOut.WriteElementString("SurName", xUser.MiddleName);
            xmlOut.WriteElementString("City", xUser.City);
            xmlOut.WriteElementString("Country", xUser.Country);
            xmlOut.WriteEndElement();
        }
        List<User> GetNecessaryData(User searchCriterion)
        {
            List<User> users = db.Users.ToList();

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
        void ProcessLines(List<User> data)
        {
            db.Set<User>().AddRange(data);
            db.SaveChanges();
        }
    }
}
