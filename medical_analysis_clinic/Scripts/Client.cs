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
        [BsonId]
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
