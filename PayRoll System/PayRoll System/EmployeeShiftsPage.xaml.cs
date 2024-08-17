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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PayRollAPI;
using PayRollControls;

namespace PayRoll_System
{
    /// <summary>
    /// Interakční logika pro EmployeeShiftsPage.xaml
    /// </summary>
    public partial class EmployeeShiftsPage : Page
    {
        private List<Shift> Shifts = new List<Shift>();

        public EmployeeShiftsPage(Employee employee, string month)
        {
            InitializeComponent();
            empNameTextBlock.Text = employee.Name + " " + employee.Surname;
            Shifts = ShiftAPI.LoadShifts(employee, month, DBConnection.connectionString);
            foreach (Shift shift in Shifts)
            {
                employeesPanel.Children.Add(new ShiftControlEdit(shift, month));
            }
            shiftsTextBlock.Text += Shifts.Count();
            TimeSpan time = Utils.CalculateHours(Shifts);
            hoursTextBlock.Text += time.Days * 24 + time.Hours + ":" + time.Minutes;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            if (key == Key.Escape || key == Key.Enter)
            {
                focusLabel.Focus();
            }
        }
    }
}
