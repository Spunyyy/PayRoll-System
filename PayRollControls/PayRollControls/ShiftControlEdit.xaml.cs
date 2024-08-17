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
    /// Interakční logika pro ShiftControlEdit.xaml
    /// </summary>
    public partial class ShiftControlEdit : UserControl
    {
        private Shift shift;
        private string shiftsTable;

        public ShiftControlEdit()
        {
            InitializeComponent();
        }

        public ShiftControlEdit(Shift shift, string month)
        {
            InitializeComponent();
            this.shift = shift;
            shiftsTable = month + DateTime.Now.Year + "Shifts";
            LoadTextBlocks();
            LoadTextBoxes();
        }

        private void LoadTextBlocks()
        {
            nameTextBlock.Text = shift.Employee.Name + " " + shift.Employee.Surname;
            dateTextBlock.Text = shift.Date.ToString();
        }

        private void LoadTextBoxes()
        {
            if(shift.StartShift != null)
            {
                sshTextBox.Text = ((TimeOnly)shift.StartShift).Hour.ToString();
                ssmTextBox.Text = ((TimeOnly)shift.StartShift).Minute.ToString();
            }
            if(shift.EndShift != null)
            {
                eshTextBox.Text = ((TimeOnly)shift.EndShift).Hour.ToString();
                esmTextBox.Text = ((TimeOnly)shift.EndShift).Minute.ToString();
            }
            if(shift.StartBreak != null)
            {
                sbhTextBox.Text = ((TimeOnly)shift.StartBreak).Hour.ToString();
                sbmTextBox.Text = ((TimeOnly)shift.StartBreak).Minute.ToString();
            }
            if(shift.EndBreak != null)
            {
                ebhTextBox.Text = ((TimeOnly)shift.EndBreak).Hour.ToString();
                ebmTextBox.Text = ((TimeOnly)shift.EndBreak).Minute.ToString();
            }
        }

        private void sshTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(int.TryParse(sshTextBox.Text, out int value))
            {
                if(value >= 0 && value <= 23)
                {
                    if(shift.StartShift != null)
                    {
                        int minute = ((TimeOnly)shift.StartShift).Minute;
                        shift.StartShift = new TimeOnly(value, minute);
                        UpdateDatabase("StartShift", shift.StartShift.ToString());
                    }
                    else
                    {
                        shift.StartShift = new TimeOnly(value, 0);
                        UpdateDatabase("StartShift", shift.StartShift.ToString());
                    }
                }
            }
        }

        private void ssmTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ssmTextBox.Text, out int value))
            {
                if (value >= 0 && value <= 59)
                {
                    if (shift.StartShift != null)
                    {
                        int hour = ((TimeOnly)shift.StartShift).Hour;
                        shift.StartShift = new TimeOnly(hour, value);
                        UpdateDatabase("StartShift", shift.StartShift.ToString());
                    }
                    else
                    {
                        shift.StartShift = new TimeOnly(0, value);
                        UpdateDatabase("StartShift", shift.StartShift.ToString());
                    }
                }
            }
        }

        private void eshTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(eshTextBox.Text, out int value))
            {
                if (value >= 0 && value <= 23)
                {
                    if (shift.EndShift != null)
                    {
                        int minute = ((TimeOnly)shift.EndShift).Minute;
                        shift.EndShift = new TimeOnly(value, minute);
                        UpdateDatabase("EndShift", shift.EndShift.ToString());
                    }
                    else
                    {
                        shift.EndShift = new TimeOnly(value, 0);
                        UpdateDatabase("EndShift", shift.EndShift.ToString());
                    }
                }
            }
        }

        private void esmTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(esmTextBox.Text, out int value))
            {
                if (value >= 0 && value <= 59)
                {
                    if (shift.EndShift != null)
                    {
                        int hour = ((TimeOnly)shift.EndShift).Hour;
                        shift.EndShift = new TimeOnly(hour, value);
                        UpdateDatabase("EndShift", shift.EndShift.ToString());
                    }
                    else
                    {
                        shift.EndShift = new TimeOnly(0, value);
                        UpdateDatabase("EndShift", shift.EndShift.ToString());
                    }
                }
            }
        }

        private void sbhTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(sbhTextBox.Text, out int value))
            {
                if (value >= 0 && value <= 23)
                {
                    if (shift.StartBreak != null)
                    {
                        int minute = ((TimeOnly)shift.StartBreak).Minute;
                        shift.StartBreak = new TimeOnly(value, minute);
                        UpdateDatabase("StartBreak", shift.StartBreak.ToString());
                    }
                    else
                    {
                        shift.StartBreak = new TimeOnly(value, 0);
                        UpdateDatabase("StartBreak", shift.StartBreak.ToString());
                    }
                }
            }
        }

        private void sbmTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(sbmTextBox.Text, out int value))
            {
                if (value >= 0 && value <= 59)
                {
                    if (shift.StartBreak != null)
                    {
                        int hour = ((TimeOnly)shift.StartBreak).Hour;
                        shift.StartBreak = new TimeOnly(hour, value);
                        UpdateDatabase("StartBreak", shift.StartBreak.ToString());
                    }
                    else
                    {
                        shift.StartBreak = new TimeOnly(0, value);
                        UpdateDatabase("StartBreak", shift.StartBreak.ToString());
                    }
                }
            }
        }

        private void ebhTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ebhTextBox.Text, out int value))
            {
                if (value >= 0 && value <= 23)
                {
                    if (shift.EndBreak != null)
                    {
                        int minute = ((TimeOnly)shift.EndBreak).Minute;
                        shift.EndBreak = new TimeOnly(value, minute);
                        UpdateDatabase("EndBreak", shift.EndBreak.ToString());
                    }
                    else
                    {
                        shift.EndBreak = new TimeOnly(value, 0);
                        UpdateDatabase("EndBreak", shift.EndBreak.ToString());
                    }
                }
            }
        }

        private void ebmTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ebmTextBox.Text, out int value))
            {
                if (value >= 0 && value <= 59)
                {
                    if (shift.EndBreak != null)
                    {
                        int hour = ((TimeOnly)shift.EndBreak).Hour;
                        shift.EndBreak = new TimeOnly(hour, value);
                        UpdateDatabase("EndBreak", shift.EndBreak.ToString());
                    }
                    else
                    {
                        shift.EndBreak = new TimeOnly(0, value);
                        UpdateDatabase("EndBreak", shift.EndBreak.ToString());
                    }
                }
            }
        }

        private void UpdateDatabase(string column, string value)
        {
            using(SqlConnection connection = new SqlConnection(DBConnection.connectionString))
            {
                connection.Open();

                SqlCommand sql = new SqlCommand("UPDATE " + shiftsTable + " SET " + column + "=@value WHERE Id=@id", connection);
                sql.Parameters.AddWithValue("@value", value);
                sql.Parameters.AddWithValue("@id", shift.Id);
                sql.ExecuteNonQuery();

                connection.Close();
            }
        }

        private void sshTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            sshTextBox.Clear();
        }

        private void ssmTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ssmTextBox.Clear();
        }

        private void eshTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            eshTextBox.Clear();
        }

        private void esmTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            esmTextBox.Clear();
        }

        private void sbhTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            sbhTextBox.Clear();
        }

        private void sbmTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            sbmTextBox.Clear();
        }

        private void ebhTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ebhTextBox.Clear();
        }

        private void ebmTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ebmTextBox.Clear();
        }
    }
}
