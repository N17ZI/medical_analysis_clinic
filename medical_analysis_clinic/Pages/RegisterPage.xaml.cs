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
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }
        public static bool AlreadyUser = false;
        private void BRegister_Click(object sender, RoutedEventArgs e)
        {
            if(Password.Text == Password1.Text && (bool)Agree.IsChecked && EmailBox!=null) // Проверка на совпадение паролей и нажатой галочки
            {
                ControllerDataBase.FindAlreadyClients(EmailBox.Text); // Найти существующего пользователя по почте
                if (AlreadyUser)
                {
                    CreateNewClient(SurnameBox.Text, Name.Text, EmailBox.Text,SNILS.Text,Birthday.SelectedDate.ToString(),Password.Text); // Вызов метода
                }
                else
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
                    notifier.ShowWarning("Пользователь с такой почтой уже существует.");
                }
                AlreadyUser = false; // Возврат по умолчанию
                
            }
        }
        // string surname, string name, string email, int snils, string birthday,string password
        public void CreateNewClient(string Surname, string Name, string Email, string Snils,string Birthday,string Password)
        {
            try
            {
                string[] arrBirthday2 = Birthday.Split(' ');
                Client client = new Client(Surname, Name, Email, Convert.ToInt32(Snils), arrBirthday2[0], Password);
                ControllerDataBase.AddToDB(client);
                NavigationService.Navigate(new Auth());
            }
            catch(Exception ex) { }
            
        }
    }
}
