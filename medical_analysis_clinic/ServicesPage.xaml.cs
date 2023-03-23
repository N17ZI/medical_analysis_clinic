using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ToastNotifications.Messages;
using ToastNotifications;
using ToastNotifications.Lifetime;
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
            GenerateButtons();
        }
        DateTime date1 = new DateTime();
        public void GenerateButtons()
        {
            TextBlock nowDay = new TextBlock();
            nowDay.TextWrapping = TextWrapping.Wrap;
            nowDay.HorizontalAlignment = HorizontalAlignment.Center;
            nowDay.Text = DateTime.Now.ToString("dd MMMM,dddd yyyy");
            DatePick.Children.Add(nowDay);
            
            for (int i = 0; i < 7; i++)
            {
                Button newBtn = new Button();
                newBtn.FontSize = 16;
                newBtn.Height = 75;
                newBtn.Width = 75;
                
                
                newBtn.HorizontalContentAlignment = HorizontalAlignment.Center;
                newBtn.Name = "Button" + i.ToString();
                
                newBtn.VerticalAlignment = VerticalAlignment.Top;
                newBtn.Content = DateTime.Now.AddMinutes(10 * i).ToShortTimeString();


                newBtn.Click += GetTicketForMed;
                DatePick1.Children.Add(newBtn);
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
            }
        }
        
    }
}
