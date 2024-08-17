using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using PayRollAPI;

namespace PayRoll_System
{
    /// <summary>
    /// Interakční logika pro ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        private Employee employee;
        public ChangePassword(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            string newPass = Utils.HashString(passwordBox.Password);
            if(newPass == employee.Password)
            {
                MessageBox.Show("New password can't be same as old", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using(SqlConnection connection = new SqlConnection(DBConnection.connectionString))
            {
                connection.Open();

                SqlCommand sql = new SqlCommand("UPDATE Employees SET Password=@password WHERE Id=@id", connection);
                sql.Parameters.AddWithValue("@password", newPass);
                sql.Parameters.AddWithValue("@id", employee.Id);
                sql.ExecuteNonQuery();

                connection.Close();
            }
            employee.Password = newPass;
            MessageBox.Show("Password successfully changed", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void stornoButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
