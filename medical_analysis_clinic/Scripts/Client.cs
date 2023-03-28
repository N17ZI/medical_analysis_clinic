using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace medical_analysis_clinic.Scripts
{
    public class Client
    {
        public Client(string surname, string name, string email, string password)
        {
            Surname = surname;
            Name = name;
            Email = email;
            Password = password;
        }
        public Client(string surname, string name, string email, string password,int snils,string middlename,string records,string birthday)
        {
            Surname = surname;
            Name = name;
            Email = email;
            Password = password;
            SNILS = snils;
            MiddleName = middlename;
            Records = records;
            Birthday = birthday;
        }
        [BsonId]
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public int SNILS { get; set; }
        public string Birthday { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Records { get; set; }

    }
}
