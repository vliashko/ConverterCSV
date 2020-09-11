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
        public User(string line)
        {
            var data = line.Split(';');
            // TODO: Проверка данных
            DateTime = DateTime.Parse(data[0]);
            Name = data[1];
            Surname = data[2];
            MiddleName = data[3];
            City = data[4];
            Country = data[5];
        } }
}
