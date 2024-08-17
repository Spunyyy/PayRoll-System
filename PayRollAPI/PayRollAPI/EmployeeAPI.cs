
using System.Data.SqlClient;

namespace PayRollAPI
{
    public class EmployeeAPI
    {
        public static Employee FindEmployee(int id, List<Employee> employees)
        {
            foreach (Employee emp in employees)
            {
                if (emp.Id == id)
                {
                    return emp;
                }
            }
            return null;
        }

        public static List<Employee> LoadEmployees(string connectionString)
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand sql = new SqlCommand("SELECT * FROM Employees", connection);
                SqlDataReader dataReader = sql.ExecuteReader();
                while (dataReader.Read())
                {
                    EmpPosition position = EmpPosition.Employee;
                    if(dataReader.GetString(6).TrimEnd() == "Manager")
                    {
                        position = EmpPosition.Manager;
                    }

                    employees.Add(new Employee(dataReader.GetInt32(0),
                        dataReader.GetString(1).TrimEnd(),
                        dataReader.GetString(2).TrimEnd(),
                        dataReader.GetString(3).TrimEnd(),
                        dataReader.GetString(4).TrimEnd(),
                        dataReader.GetDecimal(5),
                        position));
                }

                connection.Close();
            }
            return employees;
        }
    }

}
