using medical_analysis_clinic.Scripts;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace medical_analysis_clinic
{
    public partial class ServicesPage : Page
    {
        static MongoClient client = new MongoClient();
        static IMongoDatabase database = client.GetDatabase("Clinic");
        static IMongoCollection<Client> collection = database.GetCollection<Client>("Client");
        string[] Typesofservice = { "Анализ Кала", "Анализ ДНК", "Анализ на гармоны", "Анализ Мочи", "Анализ Крови" };
        public ServicesPage()
        {
            InitializeComponent();
            GenerateTypesService(Typesofservice.Count()); 
            GenerateTextBlock();
            GetData();
            GenerateButtons();
        }
        public void GenerateTypesService(int Count)
        {
            for(int a = 0;a<Count;a++)
            { 
                Button newBtn2 = new Button();
                newBtn2.HorizontalContentAlignment = HorizontalAlignment.Left;
                newBtn2.Name = "Button2" + i.ToString();
                newBtn2.Content= Typesofservice[a];

                newBtn2.Click += GetName;
                StackPanelUp.Children.Add(newBtn2);
            }
        }
        // static time
        DateTime[] date1 = new DateTime[]
            {
                new DateTime(2023, 7, 20, 8, 0, 0),
                new DateTime(2023, 7, 20, 9, 20, 0),
                new DateTime(2023, 7, 20, 10, 40, 0),
                new DateTime(2023, 7, 20, 12, 0, 0),
                new DateTime(2023, 7, 20, 13, 20, 0),
            };
        // Dynamic data
        string[] AnyDates = new string[8];
        string[] Dates;
        string buttondata;
        string currenttype = "Анализ Кала";
        int i = 0;
        int index;

        public void GenerateTextBlock()
        {
            TextBlock nowDay = new TextBlock();
            nowDay.TextWrapping = TextWrapping.Wrap;
            nowDay.HorizontalAlignment = HorizontalAlignment.Center;
            nowDay.Text = DateTime.Now.ToString("dd MMMM,dddd yyyy");
            DatePick.Children.Add(nowDay);
        }
        public void GenerateButtons()
        {
            var uniqueNumbers =
            from n in AnyDates
            group n by n into nGroup
            where nGroup.Count() == 1
            select nGroup.Key;

            if (ControllerDataBase.CanRead == true)
            {
                foreach (var item in uniqueNumbers)
                {
                    Button newBtn = new Button();
                    newBtn.FontSize = 16;
                    newBtn.Height = 75;
                    newBtn.Width = 75;
                    newBtn.VerticalAlignment = VerticalAlignment.Top;
                    newBtn.HorizontalContentAlignment = HorizontalAlignment.Center;
                    newBtn.Name = "Button" + i.ToString();
                    newBtn.Content = item;


                    newBtn.Click += GetTicketForMed;
                    DatePick1.Children.Add(newBtn);
                    i++;
                }
            }
            else
            {
                foreach (var item in AnyDates)
                {
                    Button newBtn = new Button();
                    newBtn.FontSize = 16;
                    newBtn.Height = 75;
                    newBtn.Width = 75;
                    newBtn.VerticalAlignment = VerticalAlignment.Top;
                    newBtn.HorizontalContentAlignment = HorizontalAlignment.Center;
                    newBtn.Name = "Button" + i.ToString();
                    newBtn.Content = item;


                    newBtn.Click += GetTicketForMed;
                    DatePick1.Children.Add(newBtn);
                    i++;
                }
            }
        }

        private void GetTicketForMed(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (MessageBox.Show($"Вы уверены, что хотите записаться на {btn.Content}?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Notifier notifier = new Notifier(cfg =>
                {
                    cfg.PositionProvider = new WindowPositionProvider(
                        parentWindow: Application.Current.MainWindow,
                        corner: Corner.TopRight,
                        offsetX: 10,
                        offsetY: 10);

                    cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                        notificationLifetime: TimeSpan.FromSeconds(3),
                        maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                    cfg.Dispatcher = Application.Current.Dispatcher;
                });
                notifier.ShowSuccess($"Вы были записаны на {currenttype} в {btn.Content}");
                btn.Visibility = Visibility.Collapsed;
                btn.IsEnabled= false;

                ControllerDataBase.UpdateOne(currenttype, btn.Content.ToString());
                buttondata = btn.Content.ToString();
                GetClientRecordsCheck(currenttype);
            }
        }

        private void GetName(object sender, RoutedEventArgs e)
        {
            Button btn2 = (Button)sender;
            currenttype = btn2.Content.ToString();
            index = Array.IndexOf(Typesofservice, currenttype);
            GetClientRecordsCheck(currenttype);
            DatePick1.Children.Clear();
            GetData();
            GenerateButtons();
        }
        public void FillTheArray()
        {
            for (int i = 0; i< 8; i++)
            {
                AnyDates[i] = date1[index].AddMinutes(10 * i).ToShortTimeString();
            }
            if (ControllerDataBase.CanRead == true)
            {
                AnyDates = AnyDates.Concat(Dates).ToArray();
            }
            
        }
        public void GetData()
        {
            ControllerDataBase.FindAll();
            if (ControllerDataBase.CanRead == true)
            {
                ControllerDataBase.s = Regex.Replace(ControllerDataBase.s.TrimStart(), @"\s+", " ");
                Dates = ControllerDataBase.s.Split(' ');
            }
            else { }
            FillTheArray();
        }
        private void GetClientRecordsCheck(string name)
        {
            var one = collection.Find(x => x.Name == ControllerDataBase.name).FirstOrDefault();
            int tabcount = one.Record.Count; 
            TextBlock WarningText = new TextBlock();
            WarningText.Text = $"У вас уже есть запись на {name},проверьте ее в своём личном кабинете.Нажав на логотип клиники.";
            MainStackPanel.Children.Remove(WarningText);
            for (int i = 0; i < tabcount ; i++)
            {
                if (one.Record[i].RecordsName == name)
                {
                    DatePick1.Visibility= Visibility.Collapsed;
                    WarningText.TextAlignment = TextAlignment.Center;
                    MainStackPanel.Children.Add(WarningText); 
                    break;
                }
                else
                {
                    MainStackPanel.Children.Remove(WarningText);
                    DatePick1.Visibility= Visibility.Visible;
                }
            }
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AccountPage());
        }
    }
}