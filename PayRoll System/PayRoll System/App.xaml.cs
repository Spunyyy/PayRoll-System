using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using PayRollAPI;

namespace PayRoll_System
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DBConnection.ConnectDatabase();

            using (SqlConnection connection = new SqlConnection(DBConnection.connectionString))
            {
                try
                {
                    connection.Open();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            string shiftsTable = Utils.MonthName(DateTime.Now.Month) + DateTime.Now.Year + "Shifts";
            if(!DBTable.CheckExist(shiftsTable, DBConnection.connectionString))
            {
                DBTable.Create(shiftsTable, DBConnection.connectionString);
            }

        }
    }
}
