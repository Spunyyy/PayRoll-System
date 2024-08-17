using PayRollAPI;
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
using System.Windows.Shapes;

namespace PayRollAttendance
{
    /// <summary>
    /// Interakční logika pro LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(emailTextBox.Text))
            {
                MessageBox.Show("Missing email", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(empPasswordBox.Password))
            {
                MessageBox.Show("Missing password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool badData = true;

            foreach (Employee emp in App.Employees)
            {
                if ((emailTextBox.Text == emp.Email) && (Utils.HashString(empPasswordBox.Password) == emp.Password))
                {
                    MainWindow window = new MainWindow(emp);
                    window.Show();
                    badData = false;
                    Close();
                    break;
                }
            }
            if (badData)
            {
                MessageBox.Show("Bad password or email", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
