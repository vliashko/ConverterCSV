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
using NLog;

namespace StarterTest.WinF
{
    public partial class Catalog : Form
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        readonly DBContext db = new DBContext();
        readonly DbSet<User> set;
        readonly Repository rp = new Repository();
        public Catalog()
        {
            InitializeComponent();
            set = db.Users;
            EntityFrameworkManager.DefaultEntityFrameworkPropagationValue = false;
            logger.Info("Запущена программа.");
        }
        async void ShowDataMenuItem_Click(object sender, EventArgs e)
        {
            logger.Info("Запрос на отображение данных.");
            ShowData f = new ShowData();
            if (f.ShowDialog() == DialogResult.OK)
            {
                Loader(true);
                List<User> users = await Task.Run(() => rp.GetNecessaryUserList(f.User));
                logger.Info($"Данных по запросу: {users.Count}");
                if (MessageBox.Show($"Данных по вашему запросу найдено: {users.Count}.\nХотите экспортировать эти данные?", "Информация",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    logger.Info("Экспорт отображенных данных.");
                    var ff = new ChooseDateToExport();

                    ff.textBox2.Text = f.User.Name;
                    ff.textBox5.Text = f.User.Surname;
                    ff.textBox4.Text = f.User.MiddleName;
                    ff.dateTimePicker1.Text = f.User.DateTime.ToString();
                    ff.textBox3.Text = f.User.City;
                    ff.textBox6.Text = f.User.Country;

                    ExportMethod(ff);
                }
                else
                {
                    logger.Info("Пользователь отказался от экспорта.");
                }
                await Task.Run(() => 
                {
                    if(users.Count > 0)
                    {
                        logger.Info("Загрузка данных для DBGrid начата.");
                        try
                        {
                            LoadDbDate(users);
                            GetRusNameCols();
                        }
                        catch (Exception ex)
                        {
                            logger.Error($"Ошибка загрузки данных. Текст ошибки: {ex.Message}");
                            EndProcess("Ошибка добавления данных.", false);
                        }
                    }
                    else
                    {
                        logger.Info("Отображение данных не сделано. Слишком мало данных.");
                    }
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
            logger.Info("Загрузка данных для DBGrid завершена.");
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
            logger.Info("Импорт данных из .csv.");
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
                        logger.Error("Данные не добавлены. Формат файла не .csv.");
                        EndProcess("Файлы должны быть формата .csv", true);
                        return;
                    }
                    EndProcess("Данные были успешно добавлены.", true);
                    logger.Info("Импорт данных из .csv. Успешно завершено.");
                }
            }
        }
        async void AddMenuItem_Click(object sender, EventArgs e)
        {
            logger.Info("Запрос на добавление записи вручную.");
            var form = new FormAddOrChangeUser();

            if (form.ShowDialog() == DialogResult.Yes)
            {
                if (form.User != null)
                {              
                    await Task.Run(() => {
                        User user = form.User;
                        user.Id = db.Users.Count();
                        try
                        {
                            rp.Create(user);
                            rp.Save();

                            EndProcess("Данные были успешно добавлены.", false);
                        }
                        catch(Exception ex)
                        {
                            logger.Error($"Ошибка добавления. Текст ошибки: {ex.Message}");
                            EndProcess("Запись не будет добавлена.", false);
                            return;
                        }
                    });
                    logger.Info("Добавление записи вручную завершено.");
                    return;
                }
            }
            logger.Info("Отмена добавления записи.");
        }
        void ChangeMenuItem_Click(object sender, EventArgs e)
        {
            logger.Info("Запрос на изменение записи.");

            User user;
            try
            {
                user = rp.GetUser(GetId());
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка изменения. Текст ошибки: {ex.Message}");
                EndProcess("Запись не была найдена.", false);
                return;
            }

            var form = new FormAddOrChangeUser(user);

            if (form.ShowDialog() == DialogResult.Yes)
            {
                try
                {
                    _ = form.User;
                    rp.Save();
                    dataGridView.Refresh();
                    logger.Info("Изменение записи произошло успешно.");
                    return;
                }
                catch(Exception ex)
                {
                    _ = user;
                    logger.Error($"Ошибка изменения. Текст ошибки: {ex.Message}");
                    EndProcess("Запись не будет изменена.", false);
                    return;
                }
            }
            logger.Info("Отмена изменения записи.");
        }
        void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            logger.Info("Запрос на удаление записи");

            User user;
            try
            {
                user = rp.GetUser(GetId());
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка удаления. Текст ошибки: {ex.Message}");
                EndProcess("Запись не была найдена.", false);
                return;
            }

            if (user != null)
            {
                if (MessageBox.Show("Вы точно хотите удалить эту запись?", "Удаление",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    rp.Delete(user);
                    dataGridView.Rows.RemoveAt(dataGridView.SelectedCells[0].RowIndex);
                    dataGridView.Refresh();
                    logger.Info("Удаление произошло успешно.");
                    return;
                }
                logger.Info("Отмена удаления записи.");
                return;
            }
            logger.Warn("User = null. Удаление не произошло.");
        }
        void ExportMenuItem_Click(object sender, EventArgs e)
        {
            logger.Info("Запрос на экспорт данных.");
            var form = new ChooseDateToExport();
            ExportMethod(form);   
        }
        void ExportMethod(ChooseDateToExport form)
        {
            form.ShowDialog();
            if (form.ChooseExportStyle == 1)
            {
                logger.Info("Был выбран экспорт в Excel.");
                ExportToExcel(form.User);
            }
            else if (form.ChooseExportStyle == 2)
            {
                logger.Info("Был выбран экспорт в Xml.");
                ExportToXml(form.User);
            }
        }
        async void ExportToExcel(User searchCriterion)
        {
            logger.Info("Начало экспорта в Excel.");
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
                        logger.Error("Слишком много данных для экспорта в Excel. Экспорт отменен.");
                        EndProcess("Данные не могут быть экспортированы в Excel.\nExcel не поддерживает такой большой объем данных.", true);
                        return;
                    }
                    logger.Info("Экспорт в Excel завершен успешно.");
                    EndProcess("Данные были успешно добавлены.", true);
                    return;
                } 
            }
            logger.Info("Отмена экспорта в Excel.");
        }
        async void ExportToXml(User searchCriterion)
        {
            logger.Info("Начало экспорта в Xml");
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

                    logger.Info("Экспорт в Xml завершен успешно.");
                    EndProcess("Данные были успешно добавлены.", true);
                }
            }
        }
        void ClearAllDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logger.Info("Запрос на очистку БД.");
            SqlQuery sqlQuery = new SqlQuery();
            sqlQuery.ClearAllDataWithSql();
            logger.Info("Данные были удалены.");
            EndProcess("Данные были успешно удалены.", false);
            dataGridView.DataSource = null;

            ChangeMenuItem.Enabled = false;
            DeleteMenuItem.Enabled = false;
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

                ImportToolStripMenuItem.Enabled = false;
                WorkWithTableToolStripMenuItem.Enabled = false;
                ShowDataMenuItem.Enabled = false;
                ClearAllDatabaseToolStripMenuItem.Enabled = false;
            }
            else
            {
                pictureBox1.Visible = false;

                ImportToolStripMenuItem.Enabled = true;
                WorkWithTableToolStripMenuItem.Enabled = true;
                ShowDataMenuItem.Enabled = true;
                ClearAllDatabaseToolStripMenuItem.Enabled = true;
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
