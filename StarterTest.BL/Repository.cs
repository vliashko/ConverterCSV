using StarterTest.BL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace StarterTest.BL
{
    public class Repository : IRepository
    {
        readonly static DBContext db = new DBContext();
        readonly DbSet<User> set = db.Users;
        bool disposed = false;

        public void Create(User user)
        {
            db.Users.Add(user);
            Save();
        }

        public void Delete(User user)
        {
            db.Users.Remove(user);
            Save();
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

        public List<User> GetNecessaryUserList(User searchCriterion)
        {
            List<User> users = new List<User>();
            try
            {
                users = db.Users.Where(x => searchCriterion.Name != null ? x.Name == searchCriterion.Name : true)
                                .Where(x => searchCriterion.Surname != null ? x.Surname == searchCriterion.Surname : true)
                                .Where(x => searchCriterion.MiddleName != null ? x.MiddleName == searchCriterion.MiddleName : true)
                                .Where(x => searchCriterion.DateTime != DateTime.MinValue ? x.DateTime == searchCriterion.DateTime : true)
                                .Where(x => searchCriterion.City != null ? x.City == searchCriterion.City : true)
                                .Where(x => searchCriterion.Country != null ? x.Country == searchCriterion.Country : true)
                                .ToList();
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Слишком много данных для экспорта. Нехватка памяти.", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return users;
        }

        public User GetUser(int id)
        {
            return set.Find(id) is User user ? user : null;
        }

        public void Save()
        {
            db.BulkSaveChanges();
        }
    }
}
