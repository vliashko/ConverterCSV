using StarterTest.BL.Model;
using System.Data.Entity;

namespace StarterTest.BL
{
    public class DBContext : DbContext
    {
        public DBContext() : base("MyConnection") { }
        public DbSet<User> Users { get; set; }
    }
}
