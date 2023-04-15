using medical_analysis_clinic.Models;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace medical_analysis_clinic.Scripts
{

    internal class ControllerDataBase
    {
        public static string s;
        public static string name;
        public static string Email;
        static MongoClient client = new MongoClient();
        static IMongoDatabase database = client.GetDatabase("Clinic");
        static IMongoCollection<Client> collection = database.GetCollection<Client>("Client");
        public static void AddToDB(Client client)
        {
            FindAlreadyClients(Email);
            if (RegisterPage.AlreadyUser)
            {
                collection.InsertOne(client);
            }
        }
        public static void FindOne(string name)
        {
            var one = collection.Find(x => x.Name == name).FirstOrDefault();
        }
        // Найти по Пользователя по Id
        public static void FindOneId(string id)
        {
            var one = collection.Find(x => x.Id == id).FirstOrDefault();
            App.Current.Resources["Surname"] = one.Surname;
            App.Current.Resources["Name"] = one.Name;
            App.Current.Resources["Email"] = one.Email;
            App.Current.Resources["Snils"] = one.SNILS;
            App.Current.Resources["Birthday"] = one.Birthday;
        }
        // Редактирование личных данных
        public static void ReplaceByName(string email, Client client)
        {
            collection.ReplaceOne(x => x.Email == email, client);
        }

        // Вход в приложение
        public static void Login(string email)
        {
            try
            {
                var one = collection.Find(x => x.Email == email).FirstOrDefault();
                if (email == one.Email)
                {
                    Auth.password = one.Password;
                    Auth.IdLog = one.Id;
                    Auth.email = one.Email;
                }
                else if (one.Email == null)
                {

                }
            }
            catch
            {

            }
            
        }

        // Регистариция(проверка на существующие акки)
        public static void FindAlreadyClients(string email) 
        {
            var one = collection.Find(x => x.Email == email).FirstOrDefault();
            if(one == null)
            {
                RegisterPage.AlreadyUser = true;
            }
        }
        public static void UpdateOne(string Name,string Time)
        {
            Time = Time.ToString();
            Records records = new Records(Name, Time);
            var update = Builders<Client>.Update.Push("Record", records);
            collection.UpdateOne(x => x.Id == Auth.IdLog, update);
        }
        public static void FindAll()
        {
            var list = collection.Find(x => true).ToList();
            foreach (var item in list)
            {
                if(item.Record.Count >= 1)
                {
                    var one = collection.Find(x => x.Id == item.Id).FirstOrDefault();
                    var record = one.Record.ToList();
                    foreach (var records in record)
                    {
                        WriteData(Convert.ToString(records));
                    }
                }
            }
        }

        public static void WriteData(string Name)
        {
            string str = Name;
            var result = Regex.Replace(str, @"[а-яА-ЯёЁ]", "");
            s += result.TrimEnd();
        }
    }

}
