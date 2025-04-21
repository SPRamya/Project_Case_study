using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case_study_PayXpert.entity
{
    public class Payroll
    {
        public int PayrollID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime PayPeriodStartDate { get; set; }
        public DateTime PayPeriodEndDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }


        public override string ToString()
        {
            return $"Payroll ID: {PayrollID}, Employee ID: {EmployeeID}, Period: {PayPeriodStartDate.ToShortDateString()} to {PayPeriodEndDate.ToShortDateString()}, Basic: {BasicSalary:C}, Overtime: {OvertimePay:C}, Deductions: {Deductions:C}, Net Salary: {NetSalary:C}";
        }
    }
}
