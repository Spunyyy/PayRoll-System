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

namespace PayRollAttendance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Employee employee;

        private static MainWindow instance;
        public MainWindow(Employee emp)
        {
            InitializeComponent();
            instance = this;
            employee = emp;
            if(employee.Position != EmpPosition.Manager)
            {
                menu.Items.Remove(mngMenuItem);
            }
        }

        public static MainWindow getInstance()
        {
            return instance;
        }

        private void menuItem_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new MainPage();
        }

        private void shiftsItem_Click(object sender, RoutedEventArgs e)
        {
            SelectMonthWindow window = new SelectMonthWindow(employee);
            window.ShowDialog();
        }

        private void attendanceItem_Click(object sender, RoutedEventArgs e)
        {
            List<Shift> shifts = ShiftAPI.LoadShifts(employee, Utils.MonthName(DateTime.Now.Month), DBConnection.connectionString);
            foreach(Shift shift in shifts)
            {
                if(shift.Date == App.date)
                {
                    AttendanceWindow window = new AttendanceWindow(shift);
                    window.ShowDialog();
                    break;
                }
            }
        }

        private void changeDateItem_Click(object sender, RoutedEventArgs e)
        {
            ChangeDateWindow window = new ChangeDateWindow();
            window.ShowDialog();
        }
    }
}