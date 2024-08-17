using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
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
    /// Interakční logika pro ChangeDateWindow.xaml
    /// </summary>
    public partial class ChangeDateWindow : Window
    {
        public ChangeDateWindow()
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
            if (datePicker.SelectedDate == null)
            {
                return;
            }
            DateOnly date = DateOnly.Parse(((DateTime)datePicker.SelectedDate).ToShortDateString());

            string json = File.ReadAllText("appsettings.json"); 
            JObject jsonObj = JObject.Parse(json);
            jsonObj["Settings"]["Date"] = date.ToString();
            File.WriteAllText("appsettings.json", jsonObj.ToString());

            Close();
        }
    }
}
