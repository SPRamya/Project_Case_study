using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case_study_PayXpert.entity
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? TerminationDate { get; set; }

        public int CalculateAge()
        {
            var age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateOfBirth > DateTime.Now.AddYears(-age)) age--;
            return age;
        }

        public override string ToString()
        {
            return $"Employee ID: {EmployeeID}, Name: {FirstName} {LastName}, Age: {CalculateAge()}, Email: {Email}, Phone: {PhoneNumber}, Position: {Position}";
        }
    }
}