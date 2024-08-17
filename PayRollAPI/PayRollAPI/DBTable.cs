using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PayRollAPI
{
    public class DBTable
    {
        public static void Create(string tableName, string connectionString)
        {
            string createTableQuery = @"
            CREATE TABLE " + tableName + @" (
                Id INT NOT NULL PRIMARY KEY IDENTITY, 
                EmployeeId INT NOT NULL, 
                ShiftStatus NCHAR(15) NULL, 
                Date NCHAR(10) NULL, 
                StartShift NCHAR(5) NULL,
                EndShift NCHAR(5) NULL,
                StartBreak NCHAR(5) NULL,
                EndBreak NCHAR(5) NULL,
                HadBreak NCHAR(5) NULL
            );";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    SqlCommand sql = new SqlCommand(createTableQuery, connection);
                    sql.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }

                connection.Close();
            }
        }

        public static bool CheckExist(string tableName, string connectionString)
        {
            string str = @"IF EXISTS(
                                    SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
                                    WHERE TABLE_NAME = @table) 
                                    SELECT 1 ELSE SELECT 0";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sql = new SqlCommand(str, connection);
                sql.Parameters.AddWithValue("@table", tableName);
                int exists = (int)sql.ExecuteScalar();
                if (exists == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
