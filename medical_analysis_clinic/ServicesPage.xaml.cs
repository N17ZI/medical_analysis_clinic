using medical_analysis_clinic.Scripts;
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
    /// <summary>
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public ServicesPage()
        {
            
            InitializeComponent();
            GenerateTextBlock();
            GenerateButtons();
        }
        string[] AnyDates = new string[8];
        string buttondata;
        DateTime date1 = new DateTime(2023, 7, 20, 17, 0, 0);
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
            string[] Dates = ControllerDataBase.s.Split(' ');
            FillTheArray();
            

            while (u < 8)
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
                notifier.ShowSuccess($"Вы были записаны на Анализ кала в {btn.Content}");
                btn.Visibility = Visibility.Collapsed;
                btn.IsEnabled= false;

                ControllerDataBase.UpdateOne("Анализ Кала", btn.Content.ToString());
                buttondata = btn.Content.ToString();
                RefreshData();
            }
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
            for(int i = 0;i< 8;i++) {
                AnyDates[i] = date1.AddMinutes(10 * i).ToShortTimeString();
            }
        }
        private void GetData()
        {
            ControllerDataBase.FindAll();
            ControllerDataBase.s = Regex.Replace(ControllerDataBase.s.TrimStart(), @"\s+", " ");
        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountPage());
        }
    }
}
