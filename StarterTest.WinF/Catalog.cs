using StarterTest.BL;
using StarterTest.BL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
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

        void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToExcel();
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
        void ExportToExcel()
        {
            string FileName;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel файлы (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Title = "Экспорт в Excel"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = saveFileDialog.FileName;
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkBook = xlApp.Workbooks.Add();
                Excel.Worksheet xlWorkSheet = xlWorkBook.ActiveSheet;

                for (int i = 1; i < dataGridView.RowCount + 1; i++)
                {
                    for (int j = 1; j < dataGridView.ColumnCount + 1; j++)
                    {
                        xlWorkSheet.Rows[i].Columns[j] = dataGridView.Rows[i - 1].Cells[j - 1].Value;
                    }
                }

                xlApp.AlertBeforeOverwriting = false;
                xlWorkBook.SaveAs(FileName);
                xlApp.Quit();

                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlApp);

                MessageBox.Show("Файл был успешно создан.", "Информация",
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
            openFileDialog.Title = "Импорт файла из .csv";

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
    }
}
