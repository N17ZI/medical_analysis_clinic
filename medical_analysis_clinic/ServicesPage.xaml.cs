using medical_analysis_clinic.Scripts;
using System;
using System.Drawing;
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
        public ServicesPage()
        {
            InitializeComponent();
            GenerateTextBlock();
            GenerateButtons();
            GenerateTypesService(Typesofservice.Count());

        }
        string[] Typesofservice = { "Анализ Кала", "Анализ ДНК", "Анализ на гармоны", "Анализ Мочи", "Анализ Крови" };
        string currenttype = "Анализ Кала";
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

        string[] AnyDates = new string[8];
        string[] Dates;
        string buttondata;
        DateTime date1 = new DateTime(2023, 7, 20, 8, 0, 0);
        DateTime date2 = new DateTime(2023, 7, 20, 9, 20, 0);
        DateTime date3 = new DateTime(2023, 7, 20, 10, 40, 0);
        DateTime date4 = new DateTime(2023, 7, 20, 12, 0, 0);
        DateTime date5 = new DateTime(2023, 7, 20, 13, 20, 0);
        int u = 0, i = 0;
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
            GetData();
            FillTheArray();

            while (u < 8 && ControllerDataBase.CanRead == true)
            {
                foreach (string str in AnyDates)
                {
                    if (str == Dates[u])
                    {
                        AnyDates = AnyDates.Where(e => e != Dates[u]).ToArray();
                    }
                }
                u++;
            }

            while (i < AnyDates.Length)
            {
                Button newBtn = new Button();
                newBtn.FontSize = 16;
                newBtn.Height = 75;
                newBtn.Width = 75;
                newBtn.VerticalAlignment = VerticalAlignment.Top;
                newBtn.HorizontalContentAlignment = HorizontalAlignment.Center;
                newBtn.Name = "Button" + i.ToString();
                newBtn.Content = AnyDates[i];


                newBtn.Click += GetTicketForMed;
                DatePick1.Children.Add(newBtn);
                i++;
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
                RefreshData();
            }
        }

        private void GetName(object sender, RoutedEventArgs e)
        {
            Button btn2 = (Button)sender;
            currenttype = btn2.Content.ToString();
        }
        public void RefreshData()
        {
            while (u < 8)
            {
                foreach (string str in AnyDates)
                {
                    if (str == buttondata)
                    {
                        AnyDates = AnyDates.Where(e => e != buttondata).ToArray();
                    }
                }
                u++;
            }
        }
        public void FillTheArray()
        {
            for (int i = 0; i< 8; i++)
            {
                AnyDates[i] = date1.AddMinutes(10 * i).ToShortTimeString();
            }
        }
        private void GetData()
        {
            ControllerDataBase.FindAll();
            if (ControllerDataBase.CanRead == true)
            {
                ControllerDataBase.s = Regex.Replace(ControllerDataBase.s.TrimStart(), @"\s+", " ");
                Dates = ControllerDataBase.s.Split(' ');
            }
            else { }
        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountPage());
        }
    }
}