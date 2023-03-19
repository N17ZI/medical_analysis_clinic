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

        private void BRegister_Click(object sender, RoutedEventArgs e)
        {
            if(Password.Text == Password1.Text && (bool)Agree.IsChecked)
            {
                CreateNewClient(SurnameBox.Text, Name.Text, EmailBox.Text, Password.Text);
            }
        }
        public void CreateNewClient(string Surname, string Name, string Email, string Password)
        {
            Client client = new Client(Surname, Name, Email, Password);
            ControllerDataBase.AddToDB(client);
        }
    }
}
