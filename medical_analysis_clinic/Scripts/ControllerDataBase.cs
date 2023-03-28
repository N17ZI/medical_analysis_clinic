using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace medical_analysis_clinic.Scripts
{

    public class ControllerDataBase
    {
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
        public static void ReplaceByName(string name, Client client)
        {
            collection.ReplaceOne(x => x.Name == name, client);
        }
        public static void FindAll()
        {
            var list = collection.Find(x => true).ToList();
            foreach (var item in list)
            {
                if (name == item.Name)
                {
                    Auth.password =  item.Password;
                }
            }
        }
        public static void FindAlreadyClients(string email)
        {
            var one = collection.Find(x => x.Email == email).FirstOrDefault();
            if(one == null)
            {
                RegisterPage.AlreadyUser = true;
            }
        }
    }

}
