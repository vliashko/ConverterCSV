using CsvHelper;
using StarterTest.BL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;

namespace StarterTest.BL
{
    public class ImportCsv : IDisposable
    {
        bool disposed = false;
        public bool Result { get; private set; } = true;
        readonly Repository rp = new Repository();
        readonly DBContext db = new DBContext();
        readonly DbSet<User> set;
        public ImportCsv()
        {
            set = db.Users;
        }
        public void ImportFromCsv(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                var csv = new CsvReader(sr, CultureInfo.CurrentCulture);
                csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.RegisterClassMap<UserMap>();
                csv.Configuration.Delimiter = ";";
                try
                {
                    GetDataFromCsv(csv);
                }
                catch (ReaderException)
                {
                    Result = false;
                    return;
                }
            }
        }
        void GetDataFromCsv(CsvReader csv)
        {
            List<User> users = csv.GetRecords<User>().ToList();

            db.BulkInsert(users);
            rp.Save();
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
            GC.Collect();
        }
    }
}
