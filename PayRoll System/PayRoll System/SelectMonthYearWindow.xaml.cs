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
    /// Interakční logika pro SelectMonthYearWindow.xaml
    /// </summary>
    public partial class SelectMonthYearWindow : Window
    {
        public string MonthYear { get; set; }
        public SelectMonthYearWindow()
        {
            InitializeComponent();
            yearComboBox.ItemsSource = new int[3] { 2022, 2023, 2024 };
        }

        private void stornoButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (monthComboBox.SelectedItem == null || yearComboBox.SelectedItem == null)
            {
                return;
            }
            MonthYear = Utils.MonthName(monthComboBox.SelectedIndex +1) + yearComboBox.SelectedItem.ToString();
            DialogResult = true;
            Close();
        }
    }
}
