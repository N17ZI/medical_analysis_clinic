using medical_analysis_clinic.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace medical_analysis_clinic.Scripts
{
    internal class Client
    {
        public Client() { }
        public Client(string surname, string name, string email, string password)
        {
            Surname = surname;
            Name = name;
            Email = email;
            Password = password;
            Record = new List<Records>();
        }
        public Client(string surname, string name, string email, int snils, string birthday)
        {
            Surname = surname;
            Name = name;
            Email = email;
            SNILS = snils;
            Birthday = birthday;
            Record = new List<Records>();
        }
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set ; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public int SNILS { get; set; }
        public string Birthday { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [BsonIgnoreIfNull]
        public List<Records> Record { get; set; }
    }
}
