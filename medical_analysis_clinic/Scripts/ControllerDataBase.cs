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
        static MongoClient client = new MongoClient();
        static IMongoDatabase database = client.GetDatabase("Clinic");
        static IMongoCollection<Client> collection = database.GetCollection<Client>("Client");
        public static void AddToDB(Client client)
        {
            collection.InsertOne(client);
        }
        public static void FindOne(string name)
        {
            var one = collection.Find(x => x.Name == name).FirstOrDefault();
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
    }

}
