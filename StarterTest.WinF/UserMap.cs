using CsvHelper.Configuration;
using StarterTest.BL.Model;

namespace StarterTest.WinF
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Map(m => m.DateTime).Index(0).TypeConverterOption.Format("dd.mm.yyyy");
            Map(m => m.Name);
            Map(m => m.Surname);
            Map(m => m.MiddleName);
            Map(m => m.City);
            Map(m => m.Country);
        }
    }
}
