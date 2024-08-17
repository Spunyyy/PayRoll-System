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
    /// Interakční logika pro AttendancePage.xaml
    /// </summary>
    public partial class AttendancePage : Page
    {
        private List<Shift> Shifts = new List<Shift>();
        private DateOnly date;

        public AttendancePage(DateOnly date)
        {
            InitializeComponent();
            dateLabel.Content = date.ToString();
            this.date = date;
            LoadShifts();
            foreach (Shift shift in Shifts)
            {
                employeesPanel.Children.Add(new ShiftControl(shift));

            }
            employeesTextBlock.Text += Shifts.Count();
            TimeSpan time = Utils.CalculateHours(Shifts);
            hoursTextBlock.Text += time.Days * 24 + time.Hours + ":" + time.Minutes;
        }

        private void LoadShifts()
        {
            using (SqlConnection connection = new SqlConnection(DBConnection.connectionString))
            {
                connection.Open();

                string tableName = Utils.MonthName(date.Month) + date.Year + "Shifts";

                if(DBTable.CheckExist(tableName, DBConnection.connectionString))
                {
                    SqlCommand sql = new SqlCommand("SELECT * FROM " + tableName, connection);
                    SqlDataReader dataReader = sql.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Employee emp = EmployeeAPI.FindEmployee(dataReader.GetInt32(1), MainWindow.Employees);

                        ShiftStatus status = ShiftStatus.NotStarted;
                        if (dataReader.GetString(2).TrimEnd() == "Started")
                        {
                            status = ShiftStatus.Started;
                        }
                        else if (dataReader.GetString(2).TrimEnd() == "Break")
                        {
                            status = ShiftStatus.Break;
                        }
                        else if (dataReader.GetString(2).TrimEnd() == "End")
                        {
                            status = ShiftStatus.End;
                        }

                        if (DateOnly.Parse(dataReader.GetString(3)) == date)
                        {
                            Shifts.Add(new Shift(dataReader.GetInt32(0),
                                emp,
                            status,
                            DateOnly.Parse(dataReader.GetString(3)),
                            Utils.TimeNull(dataReader.GetString(4)),
                            Utils.TimeNull(dataReader.GetString(5)),
                            Utils.TimeNull(dataReader.GetString(6)),
                            Utils.TimeNull(dataReader.GetString(7)),
                            bool.Parse(dataReader.GetString(8))));
                        }
                    }
                }
                connection.Close();
            }
        }
    }
}
