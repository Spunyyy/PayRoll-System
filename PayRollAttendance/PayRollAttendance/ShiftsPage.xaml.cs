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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PayRollControls;

namespace PayRollAttendance
{
    /// <summary>
    /// Interakční logika pro ShiftsPage.xaml
    /// </summary>
    public partial class ShiftsPage : Page
    {
        private List<Shift> Shifts = new List<Shift>();

        public ShiftsPage(Employee employee, string month)
        {
            InitializeComponent();
            empNameTextBlock.Text = employee.Name + " " + employee.Surname;
            Shifts = ShiftAPI.LoadShifts(employee, month, DBConnection.connectionString);
            foreach (Shift shift in Shifts)
            {
                employeesPanel.Children.Add(new ShiftControlView(shift));
            }
            shiftsTextBlock.Text += Shifts.Count();
            CalculateHours();
        }

        private void CalculateHours()
        {
            TimeSpan time = new TimeSpan();
            foreach (Shift shift in Shifts)
            {
                if (shift.Status == ShiftStatus.End && shift.HadBreak)
                {
                    time += (TimeSpan)(shift.EndShift - shift.StartShift) - (TimeSpan)(shift.EndBreak - shift.StartBreak);
                }
                else if (shift.Status == ShiftStatus.End && !shift.HadBreak)
                {
                    time += (TimeSpan)(shift.EndShift - shift.StartShift);
                }
                else if (shift.Status == ShiftStatus.Break)
                {
                    time += (TimeSpan)(TimeOnly.Parse(DateTime.Now.ToShortTimeString()) - shift.StartShift) - (TimeSpan)(TimeOnly.Parse(DateTime.Now.ToShortTimeString()) - shift.StartBreak);
                }
                else if (shift.Status == ShiftStatus.Started && shift.HadBreak)
                {
                    time += (TimeSpan)(TimeOnly.Parse(DateTime.Now.ToShortTimeString()) - shift.StartShift) - (TimeSpan)(shift.EndBreak - shift.StartBreak);
                }
                else if (shift.Status == ShiftStatus.Started && !shift.HadBreak)
                {
                    time += (TimeSpan)(TimeOnly.Parse(DateTime.Now.ToShortTimeString()) - shift.StartShift);
                }

            }

            hoursTextBlock.Text += time.Days * 24 + time.Hours + ":" + time.Minutes;
        }
    }
}
