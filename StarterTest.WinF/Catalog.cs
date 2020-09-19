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
using Z.EntityFramework.Extensions;

namespace StarterTest.WinF
{
    public partial class Catalog : Form
    {
        readonly DBContext db = new DBContext();
        readonly DbSet<User> set;
        readonly Repository rp = new Repository();
        public Catalog()
        {
            InitializeComponent();
            set = db.Users;
            EntityFrameworkManager.DefaultEntityFrameworkPropagationValue = false;
        }
        async void ShowDataMenuItem_Click(object sender, EventArgs e)
        {
            ShowData f = new ShowData();
            if (f.ShowDialog() == DialogResult.OK)
            {
                Loader(true);
                List<User> users = await Task.Run(() => rp.GetNecessaryUserList(f.User));
                if (MessageBox.Show($"Данных по вашему запросу найдено: {users.Count}.\nХотите экспортировать эти данные?", "Информация",
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
                await Task.Run(() => 
                {
                    LoadDbDate(users);
                    GetRusNameCols();
                });
                Loader(false);
            }
        }
        void LoadDbDate(List<User> users)
        {
            dataGridView.Refresh();
            set.AddRange(users);
            dataGridView.DataSource = set.Local.ToBindingList();
            dataGridView.Sort(dataGridView.Columns["Id"], ListSortDirection.Ascending);

            ChangeMenuItem.Enabled = true;
            DeleteMenuItem.Enabled = true;
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
        void ImportMenuItem_Click(object sender, EventArgs e)
        {
            GetDateFromCsv();
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

                using (ImportCsv importCsv = new ImportCsv())
                {
                    await Task.Run(() => importCsv.ImportFromCsv(fileName));

                    if (importCsv.Result == false)
                    {
                        MessageBox.Show("Файлы должны быть формата .csv", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Loader(false);
                        return;
                    }
                    MessageBox.Show("Данные были успешно добавлены.", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Loader(false);
                }
            }
        }
        void AddMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormAddOrChangeUser();

            if (form.ShowDialog() == DialogResult.Yes)
            {
                if (form.User != null)
                {
                    User user = form.User;
                    user.Id = db.Users.Count();

                    try
                    {
                        rp.Create(user);
                        rp.Save();

                        EndProcess("Данные были успешно добавлены.", false);
                    }
                    catch
                    {
                        MessageBox.Show("Вводимая дата должна быть в диапозоне:\n1/1/1753 - 12/31/9999\nЗапись не будет добавлена.", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
        void ChangeMenuItem_Click(object sender, EventArgs e)
        {
            User user = rp.GetUser(GetId());

            var form = new FormAddOrChangeUser(user);

            if (form.ShowDialog() == DialogResult.Yes)
            {
                try
                {
                    _ = form.User;
                    rp.Save();
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
        void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            User user = rp.GetUser(GetId());
            if (user != null)
            {
                if (MessageBox.Show("Вы точно хотите удалить эту запись?", "Удаление",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    rp.Delete(user);
                    dataGridView.Rows.RemoveAt(dataGridView.SelectedCells[0].RowIndex);
                    dataGridView.Refresh();
                }
            }
        }
        void ExportMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ChooseDateToExport();
            ExportMethod(form);   
        }
        void ExportMethod(ChooseDateToExport form)
        {
            form.ShowDialog();
            if (form.ChooseExportStyle == 1)
                ExportToExcel(form.User);
            else if (form.ChooseExportStyle == 2)
                ExportToXml(form.User);
        }
        async void ExportToExcel(User searchCriterion)
        {
            string fileName;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Экспорт в Excel"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Loader(true);
                fileName = saveFileDialog.FileName;

                using (ExportExcel exportExcel = new ExportExcel())
                {
                    await Task.Run(() => exportExcel.ExportToExcel(searchCriterion, fileName));

                    if (exportExcel.Result == false)
                    {
                        EndProcess("Данные не могут быть экспортированы в Excel.\nExcel не поддерживает такой большой объем данных.", true);
                        return;
                    }
                    EndProcess("Данные были успешно добавлены.", true);
                } 
            }
        }
        async void ExportToXml(User searchCriterion)
        {
            string fileName;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                Title = "Экспорт в XML"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;
                Loader(true);

                using (ExportXml exportXml = new ExportXml())
                {
                    await Task.Run(() => exportXml.ExportToXml(searchCriterion, fileName));

                    EndProcess("Данные были успешно добавлены.", true);
                }
            }
        }
        /// <summary>
        /// Вывод сообщения и выключение загрузчика
        /// </summary>
        /// <param name="message">Сообщения об окончании работы экспорта</param>
        /// <param name="state">Отключить загрузчик?</param>
        void EndProcess(string message, bool state)
        {
            MessageBox.Show($"{message}", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            if(state)
                Loader(false);
        }
        void Loader(bool state)
        {
            if(state)
            {
                string path = Path.GetFullPath("../../Resources/loader.gif");
                pictureBox1.ImageLocation = path;
                pictureBox1.Visible = true;
            }
            else
            {
                pictureBox1.Visible = false;
            }
        }
        int GetId()
        {
            var index = dataGridView.SelectedCells[0].RowIndex;
            int id = (int)dataGridView[0, index].Value;
            return id;
        }
    }
}
