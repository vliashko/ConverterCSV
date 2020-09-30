using System.Data.SqlClient;
using System.Windows.Forms;

namespace StarterTest.WinF
{
    public class SqlQuery
    {
        public void ClearAllDataWithSql()
        {
            if (MessageBox.Show("Вы точно хотите удалить все данные?", "Удаление",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DatabaseInfo;Integrated Security=True";
                SqlConnection sqlConnection = new SqlConnection(connString);

                sqlConnection.Open();

                string sql = "TRUNCATE TABLE dbo.Users";

                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.ExecuteNonQuery();

                sqlConnection.Close();
            }
        }
    }
}
