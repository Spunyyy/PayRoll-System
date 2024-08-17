using System.Data.SqlClient;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Employee> Employees { get; } = EmployeeAPI.LoadEmployees(DBConnection.connectionString);

        private static MainWindow instance;

        public MainWindow()
        {
            InitializeComponent();
            instance = this;
        }

        public static MainWindow getInstance()
        {
            return instance;
        }

        private void employeesItem_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new EmployeesPage();
        }

        private void menuItem_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new MainPage();
        }

        private void attendanceItem_Click(object sender, RoutedEventArgs e)
        {
            SelectDayWindow window = new SelectDayWindow();
            window.ShowDialog();
        }

        private void payoutsItem_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new PayoutsPage();
        }
    }
}