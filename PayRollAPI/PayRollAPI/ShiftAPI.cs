using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRollAPI
{
    public class ShiftAPI
    {
        public static List<Shift> LoadShifts(Employee employee, string month, string connectionString)
        {
            List<Shift> shifts = new List<Shift>();
            int id = employee.Id;
            string shiftsTable = month + DateTime.Now.Year + "Shifts";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                if (DBTable.CheckExist(shiftsTable, connectionString))
                {
                    SqlCommand sql = new SqlCommand("SELECT * FROM " + shiftsTable + " WHERE EmployeeId=@empId", connection);
                    sql.Parameters.AddWithValue("@empId", id);
                    SqlDataReader dataReader = sql.ExecuteReader();
                    while (dataReader.Read())
                    {
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

                        shifts.Add(new Shift(dataReader.GetInt32(0),
                                        employee,
                                        status,
                                        DateOnly.Parse(dataReader.GetString(3)),
                                        Utils.TimeNull(dataReader.GetString(4)),
                                        Utils.TimeNull(dataReader.GetString(5)),
                                        Utils.TimeNull(dataReader.GetString(6)),
                                        Utils.TimeNull(dataReader.GetString(7)),
                                        bool.Parse(dataReader.GetString(8))));
                    }
                }

                connection.Close();
            }
            return shifts;
        }
    }
}
