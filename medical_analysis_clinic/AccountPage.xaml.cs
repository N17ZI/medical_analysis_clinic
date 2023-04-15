using MaterialDesignThemes.Wpf;
using medical_analysis_clinic.Scripts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;

namespace medical_analysis_clinic
{
    /// <summary>
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        public AccountPage()
        {
            InitializeComponent();
        }
        static MongoClient client = new MongoClient();
        static IMongoDatabase database = client.GetDatabase("Clinic");
        static IMongoCollection<Client> collection = database.GetCollection<Client>("Client");
        private void PersonalData(object sender, RoutedEventArgs e)
        {
            MyRecordsPanel.Visibility = Visibility.Collapsed;
            PersonalDataPanel.Visibility = Visibility.Visible;
            TopText.Text = "Персональные данные";
            ControllerDataBase.FindOneId(Auth.IdLog);
            try
            {
                SurnameBox.Text = App.Current.Resources["Surname"].ToString();
                NameBox.Text = App.Current.Resources["Name"].ToString();
                EmailBox.Text = App.Current.Resources["Email"].ToString();
                //Birthday.Text = App.Current.Resources["Birthday"].ToString();
                SnilsBox.Text = App.Current.Resources["Snils"].ToString();
            }
            catch
            {

            }
        }
        private void MyRecords(object sender, RoutedEventArgs e)
        {
            while (MyRecordsPanel.Children.Count>0)
            {
                MyRecordsPanel.Children.RemoveAt(MyRecordsPanel.Children.Count-1);
            }
            PersonalDataPanel.Visibility = Visibility.Collapsed;
            MyRecordsPanel.Visibility = Visibility.Visible;
            TopText.Text = "Мои записи";

            var one = collection.Find(x => x.Id == Auth.IdLog).FirstOrDefault();
            if (one.Record.Count != 0)
            {
                var record = one.Record.ToList();
                foreach (var records in record)
                {
                    Button button = new Button();
                    button.Click += DeleteRecord;
                    button.Content = records.ToString();
                    MyRecordsPanel.Children.Add(button);
                }
            }
        }

        private async void DeleteRecord(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.Visibility = Visibility.Collapsed;

            string str = btn.Content.ToString();
            char[] delimiters = new char[] { '1', '2', '3', '4', '5', '6','7','8','9','0',':' };
            string[] parts = str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            str = "";
            foreach (string s in parts)
            {
                str += s.TrimEnd();
            }

            var update = Builders<Client>.Update.PullFilter(p => p.Record,
                                                f => f.RecordsName == str);
            var result = collection
                .FindOneAndUpdateAsync(p => p.Id == Auth.IdLog, update).Result;
            this.NavigationService.Refresh();
        }



        private void ExitFromAcc(object sender, RoutedEventArgs e)
        {
            Auth.VerifyLog = false;
            NavigationService.Navigate(new Auth());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Client client = new Client(SurnameBox.Text,NameBox.Text,EmailBox.Text, Convert.ToInt32(SnilsBox.Text), Birthday.Text);
            ControllerDataBase.ReplaceByName(EmailBox.Text, client);
        }
    }
}
