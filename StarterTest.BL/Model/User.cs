using System;

namespace StarterTest.BL.Model
{
    public class User
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MiddleName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }


        public User() { }
        public User(string dateTime, string name, string surname, string middleName, string city, string country)
        {
            // TODO: Проверка данных
            DateTime = DateTime.Parse(dateTime);
            Name = name;
            Surname = surname;
            MiddleName = middleName;
            City = city;
            Country = country;
        }
    }
}
