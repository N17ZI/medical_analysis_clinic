using medical_analysis_clinic.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToastNotifications.Messages;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace medical_analysis_clinic
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }
        public static bool AlreadyUser = false;
        private void BRegister_Click(object sender, RoutedEventArgs e)
        {
            if(Password.Text == Password1.Text && (bool)Agree.IsChecked) // Проверка на совпадение паролей и нажатой галочки
            {
                ControllerDataBase.FindAlreadyClients(EmailBox.Text); // Найти существующего пользователя по почте
                if (AlreadyUser)
                {
                    CreateNewClient(SurnameBox.Text, Name.Text, EmailBox.Text, Password.Text); // Вызов метода

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
        public void CreateNewClient(string Surname, string Name, string Email, string Password)
        {
            Client client = new Client(Surname, Name, Email, Password);
            ControllerDataBase.AddToDB(client);
            NavigationService.Navigate(new Auth());
        }
    }
}
