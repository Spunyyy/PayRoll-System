using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PayRollAPI
{
    public class Utils
    {
        public static string HashString(string inputString)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Konvertujte vstupní řetězec na bajtové pole a hashujte ho
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(inputString));

                // Konvertujte hash na řetězec a vraťte ho
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string MonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "Error";
            }
        }

        public static TimeOnly? TimeNull(string dataTime)
        {
            if (dataTime == "--:--")
            {
                return null;
            }
            else
            {
                return TimeOnly.Parse(dataTime);
            }
        }

        public static TimeSpan CalculateHours(List<Shift> shifts)
        {
            TimeSpan time = new TimeSpan();
            foreach (Shift shift in shifts)
            {
                if(shift.Status == ShiftStatus.End && shift.HadBreak)
                {
                    time += (TimeSpan)(shift.EndShift - shift.StartShift) - (TimeSpan)(shift.EndBreak - shift.StartBreak);
                }
                else if(shift.Status == ShiftStatus.End && !shift.HadBreak)
                {
                    time += (TimeSpan)(shift.EndShift - shift.StartShift);
                }
            }

            return time;
        }
    }
}
