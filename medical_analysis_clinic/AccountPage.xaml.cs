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
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        public AccountPage()
        {
            InitializeComponent();
        }

        private void PersonalData(object sender, RoutedEventArgs e)
        {
            MyRecordsPanel.Visibility = Visibility.Collapsed;
            PersonalDataPanel.Visibility = Visibility.Visible;
            TopText.Text = "Персональные данные";
        }
        private void MyRecords(object sender, RoutedEventArgs e)
        {
            PersonalDataPanel.Visibility = Visibility.Collapsed;
            MyRecordsPanel.Visibility = Visibility.Visible;
            TopText.Text = "Мои записи";
        }
        private void ExitFromAcc(object sender, RoutedEventArgs e)
        {
            Auth.VerifyLog = false;
            NavigationService.Navigate(new Auth());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
