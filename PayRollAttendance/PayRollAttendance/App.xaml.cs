using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using Microsoft.Extensions.Configuration;
using PayRollAPI;

namespace PayRollAttendance
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static DateOnly date;

        public static List<Employee> Employees { get; } = EmployeeAPI.LoadEmployees(DBConnection.connectionString);

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
            if (!DBTable.CheckExist(shiftsTable, DBConnection.connectionString))
            {
                DBTable.Create(shiftsTable, DBConnection.connectionString);
            }
            LoadSettings();
        }


        private void LoadSettings()
        {
            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            date = DateOnly.Parse(config["Settings:Date"]);
        }
    }

}
