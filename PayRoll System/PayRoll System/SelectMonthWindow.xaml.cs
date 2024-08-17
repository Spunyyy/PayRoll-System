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

namespace PayRoll_System
{
    /// <summary>
    /// Interakční logika pro SelectMonthWindow.xaml
    /// </summary>
    public partial class SelectMonthWindow : Window
    {
        private Employee employee;
        public SelectMonthWindow(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
        }

        private void stornoButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (monthComboBox.SelectedItem == null)
            {
                return;
            }
            MainWindow.getInstance().frame.Content = new EmployeeShiftsPage(employee, Utils.MonthName(monthComboBox.SelectedIndex + 1));
            Close();
        }
    }
}
