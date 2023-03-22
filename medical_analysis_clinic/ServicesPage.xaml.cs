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
                DatePick.Children.Add(newBtn);
            }
        }

        private void GetTicketForMed(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
