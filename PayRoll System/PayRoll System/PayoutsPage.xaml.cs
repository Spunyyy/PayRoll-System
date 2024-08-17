using PayRollAPI;
using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PayRoll_System
{
    /// <summary>
    /// Interakční logika pro PayoutsPage.xaml
    /// </summary>
    public partial class PayoutsPage : Page
    {
        private List<Payout> payouts = new List<Payout>();

        public PayoutsPage()
        {
            InitializeComponent();
            empListView.ItemsSource = MainWindow.Employees;
            HiddenSingleEmpVisibility();
            LoadPayouts();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Employee> empsSearch = new List<Employee>();
            foreach (Employee emp in MainWindow.Employees)
            {
                if (emp.Surname.ToLower().StartsWith(searchTextBox.Text.ToLower()))
                {
                    empsSearch.Add(emp);
                }
            }
            empListView.ItemsSource = empsSearch;
        }

        private void singleEmployeeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            VisibleSingleEmpVisibility();
        }

        private void singleEmployeeRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            HiddenSingleEmpVisibility();
        }

        private void employeeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            VisibleSingleEmpVisibility();
        }

        private void employeeRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            HiddenSingleEmpVisibility();
        }

        private void HiddenSingleEmpVisibility()
        {
            empListView.Visibility = Visibility.Hidden;
            searchTextBox.Visibility = Visibility.Hidden;
            searchTextBlock.Visibility = Visibility.Hidden;
        }
        private void VisibleSingleEmpVisibility()
        {
            empListView.Visibility = Visibility.Visible;
            searchTextBox.Visibility = Visibility.Visible;
            searchTextBlock.Visibility = Visibility.Visible;
        }

        private void createPayoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (allEmployeesRadioButton.IsChecked == true)
            {
                SelectMonthYearWindow window = new SelectMonthYearWindow();
                window.ShowDialog();
                if (window.DialogResult == true)
                {
                    List<string> empsName = new List<string>();
                    foreach (Employee emp in MainWindow.Employees)
                    {
                        Payout payout = Payout.CreatePayout(window.MonthYear, emp);
                        bool exist = false;
                        foreach (Payout po in payouts)
                        {
                            if (po.Equals(payout))
                            {
                                empsName.Add(emp.Name + " " + emp.Surname);
                                exist = true;
                                break;
                            }
                        }
                        if (!exist && payout.Time != "0,0")
                        {
                            payouts.Add(payout);
                            DBInsertPayout(payout);
                        }
                    }
                    string text = "Payouts for this employees already exist: \n";
                    for (int i = 0; i < empsName.Count; i++)
                    {
                        text += empsName[i] + ", ";
                        if (i % 3 == 0 && i != 0)
                        {
                            text += "\n";
                        }
                    }
                    if (empsName.Count >= 1)
                    {
                        MessageBox.Show(text);
                    }
                }

            }
            else if (singleEmployeeRadioButton.IsChecked == true)
            {
                if (empListView.SelectedItem != null)
                {
                    SelectMonthYearWindow window = new SelectMonthYearWindow();
                    window.ShowDialog();
                    if (window.DialogResult == true)
                    {
                        Employee emp = (Employee)empListView.SelectedItem;
                        Payout payout = Payout.CreatePayout(window.MonthYear, emp);
                        bool exist = false;
                        foreach (Payout po in payouts)
                        {
                            if (po.Equals(payout))
                            {
                                MessageBox.Show("Payout already exist");
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                        {
                            payouts.Add(payout);
                            DBInsertPayout(payout);
                        }
                    }
                }
            }
        }

        private void viewPayoutsButton_Click(object sender, RoutedEventArgs e)
        {
            if (monthRadioButton.IsChecked == true)
            {
                SelectMonthYearWindow window = new SelectMonthYearWindow();
                window.ShowDialog();
                if (window.DialogResult == true)
                {
                    List<Payout> pos = new List<Payout>();
                    foreach (Payout payout in payouts)
                    {
                        if(payout.Month == window.MonthYear)
                        {
                            pos.Add(payout);
                        }
                    }
                    MainWindow.getInstance().frame.Content = new ViewPayoutsPage(pos);
                }
            }
            else if (employeeRadioButton.IsChecked == true)
            {
                if(empListView.SelectedItem != null)
                {
                    SelectMonthYearWindow window = new SelectMonthYearWindow();
                    window.ShowDialog();
                    if (window.DialogResult == true)
                    {
                        Employee emp = (Employee)empListView.SelectedItem;
                        List<Payout> pos = new List<Payout>();
                        foreach (Payout payout in payouts)
                        {
                            if (payout.Month == window.MonthYear && emp.Equals(payout.Employee))
                            {
                                pos.Add(payout);
                            }
                        }
                        MainWindow.getInstance().frame.Content = new ViewPayoutsPage(pos);
                    }
                }
                
            }
        }

        private void DBInsertPayout(Payout payout)
        {
            using (SqlConnection connection = new SqlConnection(DBConnection.connectionString))
            {
                connection.Open();

                SqlCommand sql = new SqlCommand("INSERT INTO Payouts (Month, Time, EmpId, Sum, Tax, SocialEmployee, SocialEmployer, MedicalEmployee, MedicalEmployer, NoOfShifts) VALUES (@month, @time, @empId, @sum, @tax, @socialEmployee, @socialEmployer, @medicalEmployee, @medicalEmployer, @noOfShifts)", connection);
                sql.Parameters.AddWithValue("@month", payout.Month);
                sql.Parameters.AddWithValue("@time", payout.Time);
                sql.Parameters.AddWithValue("@empId", payout.Employee.Id);
                sql.Parameters.AddWithValue("@sum", payout.Sum);
                sql.Parameters.AddWithValue("@tax", payout.Tax);
                sql.Parameters.AddWithValue("@socialEmployee", payout.SocialEmployee);
                sql.Parameters.AddWithValue("@socialEmployer", payout.SocialEmployer);
                sql.Parameters.AddWithValue("@medicalEmployee", payout.MedicalEmployee);
                sql.Parameters.AddWithValue("@medicalEmployer", payout.MedicalEmployer);
                sql.Parameters.AddWithValue("@noOfShifts", payout.NoOfShifts);
                sql.ExecuteNonQuery();

                connection.Close();
            }
        }

        private void LoadPayouts()
        {
            using (SqlConnection connection = new SqlConnection(DBConnection.connectionString))
            {
                connection.Open();

                SqlCommand sql = new SqlCommand("SELECT * FROM Payouts", connection);
                SqlDataReader dataReader = sql.ExecuteReader();
                while (dataReader.Read())
                {
                    Employee emp = EmployeeAPI.FindEmployee(dataReader.GetInt32(3), MainWindow.Employees);

                    payouts.Add(new Payout(dataReader.GetString(1).TrimEnd(),
                        emp,
                        dataReader.GetString(2).TrimEnd(),
                        dataReader.GetDecimal(4),
                        dataReader.GetDecimal(5),
                        dataReader.GetDecimal(6),
                        dataReader.GetDecimal(7),
                        dataReader.GetDecimal(8),
                        dataReader.GetDecimal(9),
                        dataReader.GetInt32(10)));
                }

                connection.Close();
            }
        }

    }
}
