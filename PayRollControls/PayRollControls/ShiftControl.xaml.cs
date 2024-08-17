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

namespace PayRollControls
{
    /// <summary>
    /// Interakční logika pro ShiftControl.xaml
    /// </summary>
    public partial class ShiftControl : UserControl
    {
        private Shift shift;
        private string shiftsTable = Utils.MonthName(DateTime.Now.Month) + DateTime.Now.Year + "Shifts";

        public ShiftControl()
        {
            InitializeComponent();
        }

        public ShiftControl(Shift shift)
        {
            InitializeComponent();
            this.shift = shift;
            LoadTextBlocks();
            LoadButtons();
        }

        public void setShift(Shift shift)
        {
            this.shift = shift;
            LoadTextBlocks();
            LoadButtons();
        }

        private void LoadTextBlocks()
        {
            nameTextBlock.Text = shift.Employee.Name + " " + shift.Employee.Surname;
            dateTextBlock.Text = shift.Date.ToString();
            startShiftTextBlock.Text = shift.StartShift.ToString();
            endShiftTextBlock.Text = shift.EndShift.ToString();
            startBreakTextBlock.Text = shift.StartBreak.ToString();
            endBreakTextBlock.Text = shift.EndBreak.ToString();
        }

        private void LoadButtons()
        {
            startShiftFake.Content.Text = "Start shift";
            endShiftFake.Content.Text = "End shift";
            startBreakFake.Content.Text = "Start break";
            endBreakFake.Content.Text = "End break";
            foreach (Object button in Grid.Children)
            {
                if (button is Button)
                {
                    ((Button)button).Visibility = Visibility.Hidden;
                }
            }
            if (shift.Status == ShiftStatus.NotStarted)
            {
                startShiftButton.Visibility = Visibility.Visible;
                endShiftFake.Visibility = Visibility.Visible;
                startBreakFake.Visibility = Visibility.Visible;
                endBreakFake.Visibility = Visibility.Visible;
            }
            else if (shift.Status == ShiftStatus.Started)
            {
                startShiftFake.Visibility = Visibility.Visible;
                endShiftButton.Visibility = Visibility.Visible;
                startBreakButton.Visibility = Visibility.Visible;
                endBreakFake.Visibility = Visibility.Visible;
            }
            else if (shift.Status == ShiftStatus.Break)
            {
                startShiftFake.Visibility = Visibility.Visible;
                endShiftFake.Visibility = Visibility.Visible;
                startBreakFake.Visibility = Visibility.Visible;
                endBreakButton.Visibility = Visibility.Visible;
            }
            else if (shift.Status == ShiftStatus.End)
            {
                startShiftFake.Visibility = Visibility.Visible;
                endShiftFake.Visibility = Visibility.Visible;
                startBreakFake.Visibility = Visibility.Visible;
                endBreakFake.Visibility = Visibility.Visible;
            }
            if (shift.HadBreak && shift.Status == ShiftStatus.Started)
            {
                startBreakButton.Visibility = Visibility.Hidden;
                startBreakFake.Visibility = Visibility.Visible;
                endBreakFake.Visibility = Visibility.Visible;
            }
        }

        private void startShiftButton_Click(object sender, RoutedEventArgs e)
        {
            TimeOnly time = TimeOnly.Parse(DateTime.Now.ToShortTimeString());

            using (SqlConnection connection = new SqlConnection(DBConnection.connectionString))
            {
                connection.Open();

                SqlCommand sql = new SqlCommand("UPDATE " + shiftsTable + " SET ShiftStatus=@shiftStatus, StartShift=@startShift WHERE Id=@id", connection);
                sql.Parameters.AddWithValue("@shiftStatus", ShiftStatus.Started.ToString());
                sql.Parameters.AddWithValue("@startShift", time.ToString());
                sql.Parameters.AddWithValue("@id", shift.Id);
                sql.ExecuteNonQuery();

                connection.Close();
            }

            shift.Status = ShiftStatus.Started;

            startShiftTextBlock.Text = time.ToString();

            startShiftButton.Visibility = Visibility.Hidden;
            endShiftButton.Visibility = Visibility.Visible;
            endShiftFake.Visibility = Visibility.Hidden;
            startBreakButton.Visibility = Visibility.Visible;
            startBreakFake.Visibility = Visibility.Hidden;
            startShiftFake.Visibility = Visibility.Visible;
        }

        private void endShiftButton_Click(object sender, RoutedEventArgs e)
        {
            TimeOnly time = TimeOnly.Parse(DateTime.Now.ToShortTimeString());

            using (SqlConnection connection = new SqlConnection(DBConnection.connectionString))
            {
                connection.Open();

                SqlCommand sql = new SqlCommand("UPDATE " + shiftsTable + " SET ShiftStatus=@shiftStatus, EndShift=@endShift WHERE Id=@id", connection);
                sql.Parameters.AddWithValue("@shiftStatus", ShiftStatus.End.ToString());
                sql.Parameters.AddWithValue("@endShift", time.ToString());
                sql.Parameters.AddWithValue("@id", shift.Id);
                sql.ExecuteNonQuery();

                connection.Close();
            }

            shift.Status = ShiftStatus.End;

            endShiftTextBlock.Text = time.ToString();

            endShiftButton.Visibility = Visibility.Hidden;
            endShiftFake.Visibility = Visibility.Visible;
        }

        private void startBreakButton_Click(object sender, RoutedEventArgs e)
        {
            TimeOnly time = TimeOnly.Parse(DateTime.Now.ToShortTimeString());

            using (SqlConnection connection = new SqlConnection(DBConnection.connectionString))
            {
                connection.Open();

                SqlCommand sql = new SqlCommand("UPDATE " + shiftsTable + " SET ShiftStatus=@shiftStatus, StartBreak=@startBreak, HadBreak=@hadBreak WHERE Id=@id", connection);
                sql.Parameters.AddWithValue("@shiftStatus", ShiftStatus.Break.ToString());
                sql.Parameters.AddWithValue("@startBreak", time.ToString());
                sql.Parameters.AddWithValue("@hadBreak", "true");
                sql.Parameters.AddWithValue("@id", shift.Id);
                sql.ExecuteNonQuery();

                connection.Close();
            }

            shift.Status = ShiftStatus.Break;

            startBreakTextBlock.Text = time.ToString();

            startBreakButton.Visibility = Visibility.Hidden;
            endShiftButton.Visibility = Visibility.Hidden;
            endBreakButton.Visibility = Visibility.Visible;
            endBreakFake.Visibility = Visibility.Hidden;
            startBreakFake.Visibility = Visibility.Visible;
            endShiftFake.Visibility = Visibility.Visible;

        }

        private void endBreakButton_Click(object sender, RoutedEventArgs e)
        {
            TimeOnly time = TimeOnly.Parse(DateTime.Now.ToShortTimeString());

            using (SqlConnection connection = new SqlConnection(DBConnection.connectionString))
            {
                connection.Open();

                SqlCommand sql = new SqlCommand("UPDATE " + shiftsTable + " SET ShiftStatus=@shiftStatus, EndBreak=@endBreak WHERE Id=@id", connection);
                sql.Parameters.AddWithValue("@shiftStatus", ShiftStatus.Started.ToString());
                sql.Parameters.AddWithValue("@endBreak", time.ToString());
                sql.Parameters.AddWithValue("@id", shift.Id);
                sql.ExecuteNonQuery();

                connection.Close();
            }

            shift.Status = ShiftStatus.Started;

            endBreakTextBlock.Text = time.ToString();

            endBreakButton.Visibility = Visibility.Hidden;
            endBreakFake.Visibility = Visibility.Visible;
            endShiftFake.Visibility = Visibility.Hidden;
            endShiftButton.Visibility = Visibility.Visible;
        }
    }
}