using StarterTest.BL.Model;
using Ganss.Excel;
using System.Collections.Generic;
using System;

namespace StarterTest.BL
{
    public class ExportExcel : IDisposable
    {
        bool disposed = false;
        public bool Result { get; private set; } = true;
        readonly Repository rp = new Repository();
        readonly DBContext db = new DBContext();
        readonly int maxRowsInExcel = 1048576;

        public void ExportToExcel(User searchCriterion, string fileName)
        {
            List<User> users = rp.GetNecessaryUserList(searchCriterion);
            if (users.Count > maxRowsInExcel)
            {
                Result = false;
                return;
            }

            ExcelMapper mapper = new ExcelMapper();
            mapper.Save(fileName, users, "Excel", true);

            users.Clear();
        }
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
