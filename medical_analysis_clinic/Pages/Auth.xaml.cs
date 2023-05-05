using medical_analysis_clinic.Scripts;
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
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();
        }
        public static string password;
        public static string email;
        public static bool VerifyLog = false;
        public static string IdLog;
        private void eventVisible_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked.Value)
            {
                pwdTextBox.Text = pwdPasswordBox.Password; // скопируем в TextBox из PasswordBox
                pwdTextBox.Visibility = Visibility.Visible; // TextBox - отобразить
                pwdPasswordBox.Visibility = Visibility.Hidden; // PasswordBox - скрыть
            }
            else
            {
                pwdPasswordBox.Password = pwdTextBox.Text; // скопируем в PasswordBox из TextBox 
                pwdTextBox.Visibility = Visibility.Hidden; // TextBox - скрыть
                pwdPasswordBox.Visibility = Visibility.Visible; // PasswordBox - отобразить
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }

        private void AuthButton(object sender, RoutedEventArgs e)
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
            if (LoginBox.Text != null && pwdPasswordBox.Password != null)
            {
                try
                {
                    string passwordInput = pwdPasswordBox.Password;
                    ControllerDataBase.name = LoginBox.Text;
                    ControllerDataBase.Login(LoginBox.Text);
                    if (pwdPasswordBox != null && password == passwordInput)
                    {
                        NavigationService.Navigate(new ServicesPage());
                        VerifyLog = true;
                    }
                    else
                    {
                        notifier.ShowWarning("Неправильный Логин/Пароль!");
                    }
                }
                catch(Exception ex) {}
            }
        }
    }
}
