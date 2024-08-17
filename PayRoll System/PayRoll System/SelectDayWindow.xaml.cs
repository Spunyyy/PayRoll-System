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
    /// Interakční logika pro SelectDayWindow.xaml
    /// </summary>
    public partial class SelectDayWindow : Window
    {
        public SelectDayWindow()
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Today;
        }

        private void stornoButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if(datePicker.SelectedDate == null)
            {
                return;
            }
            DateOnly date = DateOnly.Parse(((DateTime)datePicker.SelectedDate).ToShortDateString());
            MainWindow.getInstance().frame.Content = new AttendancePage(date);
            Close();
        }
    }
}
