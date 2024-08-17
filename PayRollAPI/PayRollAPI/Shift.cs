using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRollAPI
{
    public class Shift
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public ShiftStatus Status { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly? StartShift { get; set; }
        public TimeOnly? EndShift { get; set; }
        public TimeOnly? StartBreak { get; set; }
        public TimeOnly? EndBreak { get; set; }
        public bool HadBreak { get; set; }

        public Shift()
        {

        }

        public Shift(int id, Employee employee, ShiftStatus status, DateOnly date, TimeOnly? startShift, TimeOnly? endShift, TimeOnly? startBreak, TimeOnly? endBreak, bool hadBreak)
        {
            Id = id;
            Employee = employee;
            Status = status;
            Date = date;
            StartShift = startShift;
            EndShift = endShift;
            StartBreak = startBreak;
            EndBreak = endBreak;
            HadBreak = hadBreak;
        }
    }
}
