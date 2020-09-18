using CsvHelper;
using Ganss.Excel;
using StarterTest.BL;
using StarterTest.BL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Z.EntityFramework.Extensions;

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
            EntityFrameworkManager.DefaultEntityFrameworkPropagationValue = false;
        }
        async void ShowDataMenuItem_Click(object sender, EventArgs e)
        {
            ShowData f = new ShowData();
            if(f.ShowDialog() == DialogResult.OK)
            {
                Loader(true);
                List<User> users = await Task.Run(() => GetNecessaryData(f.User));
                if(MessageBox.Show($"Данных по вашему запросу найдено: {users.Count}.\nХотите экспортировать эти данные?", "Информация",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    var ff = new ChooseDateToExport();

                    ff.textBox2.Text = f.User.Name;
                    ff.textBox5.Text = f.User.Surname;
                    ff.textBox4.Text = f.User.MiddleName;
                    if (f.User.DateTime != DateTime.MinValue)
                        ff.maskedTextBox1.Text = f.User.DateTime.ToString();
                    ff.textBox3.Text = f.User.City;
                    ff.textBox6.Text = f.User.Country;

                    ExportMethod(ff);
                }
                LoadDbDate(users);
                GetRusNameCols();
                Loader(false);
            }     
        }
        void ImportMenuItem_Click(object sender, EventArgs e)
        {
            GetDateFromCsv();
        }
        void AddMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormAddOrChangeUser();

            if (form.ShowDialog() == DialogResult.OK)
            {
                var user = form.User;
                user.Id = db.Users.Count();

                try
                {
                    db.Users.Add(user);
                    db.BulkSaveChanges();
                    dataGridView.Refresh();
                }
                catch
                {
                    db.Users.Remove(user);
                    MessageBox.Show("Вводимая дата должна быть в диапозоне:\n1/1/1753 - 12/31/9999", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        void ChangeMenuItem_Click(object sender, EventArgs e)
        {
            var index = dataGridView.SelectedCells[0].RowIndex;
            int id = (int)dataGridView[0, index].Value;

            if (set.Find(id) is User user)
            {
                var form = new FormAddOrChangeUser(user);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _ = form.User;
                        db.BulkSaveChanges();
                        dataGridView.Refresh();
                    }
                    catch
                    {
                        _ = user;
                        MessageBox.Show("Вводимая дата должна быть в диапозоне:\n1/1/1753 - 12/31/9999", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
        void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            var index = dataGridView.SelectedCells[0].RowIndex;
            if (MessageBox.Show("Вы точно хотите удалить эту запись?", "Удаление",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                dataGridView.Rows.RemoveAt(index);
                db.BulkSaveChanges();
                dataGridView.Refresh();
            }
        }
        void ExportMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ChooseDateToExport();
            ExportMethod(form);
            
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
                Loader(true);
                FileName = saveFileDialog.FileName;

                List<User> users = await Task.Run(() => GetNecessaryData(searchCriterion));
                if (users.Count > 1048576)
                {
                    MessageBox.Show("Данные не могут быть экспортированы в Excel.\nExcel не поддерживает такой большой объем данных.",
                        "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ExcelMapper mapper = new ExcelMapper();
                await Task.Run(() => CreareSaverForExcelExportAsync(FileName, users, mapper));

                MessageBox.Show("Данные были успешно добавлены.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Loader(false);
            }
        }

        void CreareSaverForExcelExportAsync(string FileName, List<User> users, ExcelMapper mapper)
        {
            mapper.Save(FileName, users, "Excel", true);
        }

        async void GetDateFromCsv()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\Users\User\Desktop",
                Filter = "CSV файлы (*.csv)|*.csv",
                Title = "Импорт файла .csv"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Loader(true);
                string fileName = openFileDialog.FileName;

                using(StreamReader sr = new StreamReader(fileName))
                {
                    var csv = new CsvReader(sr, CultureInfo.CurrentCulture);
                    csv.Configuration.HasHeaderRecord = false;
                    csv.Configuration.RegisterClassMap<UserMap>();
                    csv.Configuration.Delimiter = ";";

                    await Task.Run(() => GetDataFromCsvAsync(csv));

                    MessageBox.Show("Данные были успешно добавлены.", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Loader(false);
                }
            }
        }

        void GetDataFromCsvAsync(CsvReader csv)
        {
            List<User> users =  csv.GetRecords<User>().ToList();

            db.BulkInsert(users);
            db.BulkSaveChanges();
        }

        void LoadDbDate(List<User> users)
        {
            set.AddRange(users);
            dataGridView.DataSource = set.Local.ToBindingList();
            dataGridView.Sort(dataGridView.Columns["Id"], ListSortDirection.Ascending);

            работаСТаблицейToolStripMenuItem.Enabled = true;
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
                Loader(true);
                List<User> users = await Task.Run(() => GetNecessaryData(searchCriterion));

                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    using (XmlTextWriter xmlOut = new XmlTextWriter(fs, Encoding.Unicode))
                    {
                        xmlOut.Formatting = Formatting.Indented;

                        xmlOut.WriteStartDocument();
                        xmlOut.WriteStartElement("TestProgram");

                        await Task.Run(() => ForeachExportXmlAsync(users, xmlOut));

                        xmlOut.WriteEndElement();
                        xmlOut.WriteEndDocument();

                        MessageBox.Show("Данные были успешно добавлены.", "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Loader(false);
                        users.Clear();
                    }
                }
            }
        }

        void ForeachExportXmlAsync(List<User> users, XmlTextWriter xmlOut)
        {
            foreach (User xUser in users)
            {
                SaveToFile(xmlOut, xUser);
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
            List<User> users = new List<User>();

            users = db.Users.Where(x => searchCriterion.Name != null ? x.Name == searchCriterion.Name : true)
                            .Where(x => searchCriterion.Surname != null ? x.Surname == searchCriterion.Surname : true)
                            .Where(x => searchCriterion.MiddleName != null ? x.MiddleName == searchCriterion.MiddleName : true)
                            .Where(x => searchCriterion.DateTime != DateTime.MinValue ? x.DateTime == searchCriterion.DateTime : true)
                            .Where(x => searchCriterion.City != null ? x.City == searchCriterion.City : true)
                            .Where(x => searchCriterion.Country != null ? x.Country == searchCriterion.Country : true)
                            .ToList();
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
        void ExportMethod(ChooseDateToExport form)
        {
            form.ShowDialog();
                if (form.ChooseExportStyle == 1)
                    ExportToExcel(form.User);
                else if (form.ChooseExportStyle == 2)
                    ExportToXml(form.User);
        }

        void Loader(bool state)
        {
            if(state)
            {
                string path = Path.GetFullPath("../../Resources/loader.gif");
                pictureBox1.ImageLocation = path;
            }
            else
            {
                pictureBox1.Visible = false;
            }
        }
    }
}
