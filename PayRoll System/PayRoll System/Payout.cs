using PayRollAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PayRoll_System
{
    public class Payout
    {
        public string Month { get; set; }
        public Employee Employee { get; set; }
        public string Time { get; set; }
        
        public decimal Sum { get; set; }
        public decimal Tax { get; set; }
        public decimal SocialEmployee { get; set; }
        public decimal SocialEmployer { get; set; }
        public decimal MedicalEmployee { get; set; }
        public decimal MedicalEmployer { get; set; }

        public int NoOfShifts { get; set; }

        public Payout(string month, Employee employee, string time, decimal sum, decimal tax, decimal socialEmployee, decimal socialEmployer, decimal medicalEmployee, decimal medicalEmployer, int noOfShifts)
        {
            Month = month;
            Employee = employee;
            Time = time;
            Sum = sum;
            Tax = tax;
            SocialEmployee = socialEmployee;
            SocialEmployer = socialEmployer;
            MedicalEmployee = medicalEmployee;
            MedicalEmployer = medicalEmployer;
            NoOfShifts = noOfShifts;
        }

        public Payout()
        {
            
        }

        public static Payout CreatePayout(string month, Employee employee)
        {
            Payout payout = new Payout();

            payout.Month = month;
            payout.Employee = employee;

            List<Shift> shifts = ShiftAPI.LoadShifts(employee, month.Substring(0, month.Length - 4), DBConnection.connectionString);

            TimeSpan shiftsTime = Utils.CalculateHours(shifts);
            string minutes = "";
            if(shiftsTime.Minutes <= 14)
            {
                minutes = "0";
            }
            else if (shiftsTime.Minutes <= 29)
            {
                minutes = "25";
            }
            else if (shiftsTime.Minutes <= 44)
            {
                minutes = "5";
            }
            else if (shiftsTime.Minutes <= 59)
            {
                minutes = "75";
            }

            payout.Time = shiftsTime.Days * 24 + shiftsTime.Hours + "," + minutes;

            decimal cash = employee.HourlyRate * decimal.Parse(payout.Time);

            payout.SocialEmployee = Math.Round(cash * (decimal.Parse(Settings.ReturnSetting("SocialEmployee")) / 100));
            payout.MedicalEmployee = Math.Round(cash * (decimal.Parse(Settings.ReturnSetting("MedicalEmployee")) / 100));
            payout.SocialEmployer = Math.Round(cash * (decimal.Parse(Settings.ReturnSetting("SocialEmployer")) / 100));
            payout.MedicalEmployer = Math.Round(cash * (decimal.Parse(Settings.ReturnSetting("MedicalEmployer")) / 100));

            decimal tax = Math.Round((cash * (decimal.Parse(Settings.ReturnSetting("Tax")) / 100)) - decimal.Parse(Settings.ReturnSetting("TaxDiscount")));
            if(tax  <= 0)
            {
                tax = 0;
            }

            payout.Tax = tax;

            payout.Sum = Math.Round(cash - payout.SocialEmployee - payout.MedicalEmployee - payout.Tax);

            payout.NoOfShifts = shifts.Count;

            return payout;
        }

        public override bool Equals(object? obj)
        {
            return obj is Payout payout &&
                   Month == payout.Month &&
                   EqualityComparer<Employee>.Default.Equals(Employee, payout.Employee);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Month, Employee);
        }
    }
}
