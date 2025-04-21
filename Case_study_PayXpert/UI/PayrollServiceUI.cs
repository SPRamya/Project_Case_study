using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case_study_PayXpert.dao;
using Case_study_PayXpert.entity;

namespace Case_study_PayXpert.UI
{
    public class PayrollServiceUI
    {
        private readonly IPayrollService _payrollService;

        public PayrollServiceUI(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        public void GeneratePayroll()
        {
            try
            {
                Console.Write("Enter Employee ID: ");
                int empId = int.Parse(Console.ReadLine());
                Console.Write("Start Date (yyyy-MM-dd): ");
                DateTime start = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Console.Write("End Date (yyyy-MM-dd): ");
                DateTime end = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                _payrollService.GeneratePayroll(empId, start, end);
                Console.WriteLine("Payroll generated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void ViewEmployeePayroll()
        {
            Console.Write("Enter Employee ID: ");
            int empId = int.Parse(Console.ReadLine());
            var payrolls = _payrollService.GetPayrollsForEmployee(empId);

            if (payrolls.Count == 0)
            {
                Console.WriteLine("No payrolls found.");
                return;
            }

            foreach (var p in payrolls)
            {
                Console.WriteLine($"Payroll ID: {p.PayrollID} | Period: {p.PayPeriodStartDate:yyyy-MM-dd} to {p.PayPeriodEndDate:yyyy-MM-dd}");
                Console.WriteLine($"Net Salary: {p.NetSalary:C}");
                Console.WriteLine("-----------------------------------");
            }
        }

        public void GetPayrollById()
        {
            try
            {
                Console.Write("Enter Payroll ID: ");
                int payrollId = int.Parse(Console.ReadLine());

                var payroll = _payrollService.GetPayrollById(payrollId);
                if (payroll == null)
                {
                    Console.WriteLine("Payroll not found.");
                    return;
                }

                Console.WriteLine($"Payroll ID: {payroll.PayrollID}");
                Console.WriteLine($"Employee ID: {payroll.EmployeeID}");
                Console.WriteLine($"Pay Period: {payroll.PayPeriodStartDate:yyyy-MM-dd} to {payroll.PayPeriodEndDate:yyyy-MM-dd}");
                Console.WriteLine($"Basic Salary: {payroll.BasicSalary:C}");
                Console.WriteLine($"Overtime Pay: {payroll.OvertimePay:C}");
                Console.WriteLine($"Deductions: {payroll.Deductions:C}");
                Console.WriteLine($"Net Salary: {payroll.NetSalary:C}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void ViewPayrollsForEmployee()
        {
            try
            {
                Console.Write("Enter Employee ID: ");
                int empId = int.Parse(Console.ReadLine());
                var payrolls = _payrollService.GetPayrollsForEmployee(empId);

                if (payrolls.Count == 0)
                {
                    Console.WriteLine("No payrolls found for this employee.");
                    return;
                }

                foreach (var p in payrolls)
                {
                    Console.WriteLine($"Payroll ID: {p.PayrollID} | Period: {p.PayPeriodStartDate:yyyy-MM-dd} to {p.PayPeriodEndDate:yyyy-MM-dd}");
                    Console.WriteLine($"Basic Salary: {p.BasicSalary:C} | Overtime Pay: {p.OvertimePay:C} | Deductions: {p.Deductions:C} | Net Salary: {p.NetSalary:C}");
                    Console.WriteLine("-----------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void ViewPayrollsForPeriod()
        {
            try
            {
                Console.Write("Enter Start Date (yyyy-MM-dd): ");
                DateTime start = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Console.Write("Enter End Date (yyyy-MM-dd): ");
                DateTime end = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var payrolls = _payrollService.GetPayrollsForPeriod(start, end);

                if (payrolls.Count == 0)
                {
                    Console.WriteLine("No payrolls found for this period.");
                    return;
                }

                foreach (var p in payrolls)
                {
                    Console.WriteLine($"Payroll ID: {p.PayrollID} | Employee ID: {p.EmployeeID} | Period: {p.PayPeriodStartDate:yyyy-MM-dd} to {p.PayPeriodEndDate:yyyy-MM-dd}");
                    Console.WriteLine($"Basic Salary: {p.BasicSalary:C} | Overtime Pay: {p.OvertimePay:C} | Deductions: {p.Deductions:C} | Net Salary: {p.NetSalary:C}");
                    Console.WriteLine("-----------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public decimal CalculateGrossSalary(Payroll payroll)
        {
            if (payroll == null)
                throw new ArgumentNullException(nameof(payroll));

            return payroll.BasicSalary + payroll.OvertimePay;
        }

        public decimal CalculateNetSalary(Payroll payroll)
        {
            if (payroll == null)
                throw new ArgumentNullException(nameof(payroll));

            decimal grossSalary = CalculateGrossSalary(payroll);
            decimal netSalary = grossSalary - payroll.Deductions;

            payroll.NetSalary = netSalary; 
            return netSalary;
        }

    }
}