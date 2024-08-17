using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PayRollAPI;

namespace PayRoll_System
{
    /// <summary>
    /// Interakční logika pro NewEmployee.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        private Employee employee;

        public EmployeesPage()
        {
            InitializeComponent();
            CreateNewEmployee();
            try{employeesDataGrid.ItemsSource = MainWindow.Employees;}catch {}
        }

        private void saveEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            employee.Password = Utils.HashString(passwordBox.Password);
            switch (positionComboBox.SelectedIndex)
            {
                case 0: employee.Position = EmpPosition.Manager; break;
                case 1: employee.Position = EmpPosition.Employee; break;
                default: employee.Position = EmpPosition.Employee; break;
            } 

            using (SqlConnection connection = new SqlConnection(DBConnection.connectionString))
            {
                connection.Open();

                SqlCommand sql = new SqlCommand("INSERT INTO Employees (Name, Surname, Email, Password, HourlyRate, Position) VALUES (@name, @surname, @email, @password, @hourlyrate, @position)", connection);
                sql.Parameters.AddWithValue("@name", employee.Name);
                sql.Parameters.AddWithValue("@surname", employee.Surname);
                sql.Parameters.AddWithValue("@email", employee.Email);
                sql.Parameters.AddWithValue("@password", employee.Password);
                sql.Parameters.AddWithValue("@hourlyrate", employee.HourlyRate);
                sql.Parameters.AddWithValue("@position", employee.Position.ToString());
                sql.ExecuteNonQuery();

                SqlCommand getId = new SqlCommand("SELECT IDENT_CURRENT('Employees')", connection);
                int id = Convert.ToInt32(getId.ExecuteScalar());
                employee.Id = id;

                connection.Close();
            }

            MainWindow.Employees.Add(employee);

            passwordBox.Password = "";
            positionComboBox.SelectedIndex = -1;

            employeesDataGrid.Items.Refresh();
            CreateNewEmployee();
        }

        private void CreateNewEmployee()
        {
            employee = new Employee();
            DataContext = employee;
        }

        private void employeesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Employee selectedEmployee = (Employee)employeesDataGrid.SelectedItem;

            string newValue = ((TextBox)e.EditingElement).Text;

            string cmd = "UPDATE Employees SET ";
            switch (e.Column.Header.ToString())
            {
                case "Name":
                    cmd += "Name=@value";
                    break;
                case "Surname":
                    cmd += "Surname=@value";
                    break;
                case "Email":
                    cmd += "Email=@value";
                    break;
                case "Hourly Rate":
                    cmd += "HourlyRate=@value";
                    break;
                case "Position":
                    cmd += "Position=@value";
                    break;
            }

            cmd += " WHERE Id=@id";

            using (SqlConnection connection = new SqlConnection(DBConnection.connectionString))
            {
                connection.Open();
                SqlCommand sql = new SqlCommand(cmd, connection);
                sql.Parameters.AddWithValue("@value", newValue);
                sql.Parameters.AddWithValue("@id", selectedEmployee.Id);
                sql.ExecuteNonQuery();

                connection.Close();
            }
        }

        private void deleteEmployeeItem_Click(object sender, RoutedEventArgs e)
        {
            Employee emp = (Employee)employeesDataGrid.SelectedItem;
            MessageBoxResult result = MessageBox.Show("Do you really want delete\n" + emp.Name + " " + emp.Surname, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(DBConnection.connectionString))
                {
                    connection.Open();

                    SqlCommand sql = new SqlCommand("DELETE FROM Employees WHERE Id=@id", connection);
                    sql.Parameters.AddWithValue("@id", emp.Id);
                    sql.ExecuteNonQuery();

                    connection.Close();
                }

                MainWindow.Employees.Remove(emp);
                employeesDataGrid.Items.Refresh();
            }
            
        }

        private void changePasswordItem_Click(object sender, RoutedEventArgs e)
        {
            Employee emp = (Employee)employeesDataGrid.SelectedItem;
            ChangePassword window = new ChangePassword(emp);
            window.ShowDialog();
        }

        private void viewShiftsItem_Click(object sender, RoutedEventArgs e)
        {
            Employee emp = (Employee)employeesDataGrid.SelectedItem;
            SelectMonthWindow window = new SelectMonthWindow(emp);
            window.ShowDialog();
        }
    }
}
