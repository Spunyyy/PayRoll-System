using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRollAPI
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal HourlyRate { get; set; }
        public EmpPosition Position { get; set; }

        public Employee()
        {

        }

        public Employee(int id, string name, string surname, string email, string password, decimal hourlyRate, EmpPosition position)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            HourlyRate = hourlyRate;
            Position = position;
        }
    }
}
