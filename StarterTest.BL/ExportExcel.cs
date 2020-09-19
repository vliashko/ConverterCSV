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

        public void ExportToExcel(User searchCriterion, string fileName)
        {
            List<User> users = rp.GetNecessaryUserList(searchCriterion);
            if (users.Count > 1048576)
            {
                Result = false;
                return;
            }
            ExcelMapper mapper = new ExcelMapper();
            CreateSaverForExcelExport(fileName, users, mapper);
            users.Clear();
        }
        void CreateSaverForExcelExport(string FileName, List<User> users, ExcelMapper mapper)
        {
            mapper.Save(FileName, users, "Excel", true);
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
