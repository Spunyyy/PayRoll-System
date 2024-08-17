using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRollAPI
{
    public class DBConnection
    {
        public static string connectionString;

        public static void ConnectDatabase()
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = @"(LocalDB)\MSSQLLocalDB";
            csb.InitialCatalog = "Payroll";
            csb.IntegratedSecurity = true;
            connectionString = csb.ConnectionString;
        }
    }
}
