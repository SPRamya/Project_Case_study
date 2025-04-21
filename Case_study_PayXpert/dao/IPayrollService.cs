using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case_study_PayXpert.entity;

namespace Case_study_PayXpert.dao
{
    public interface IPayrollService
    {
        decimal CalculateGrossSalary(Employee employee);
        decimal CalculateNetSalary(decimal gross, decimal deductions);
        void GeneratePayroll(int employeeId, DateTime startDate, DateTime endDate);
            Payroll GetPayrollById(int payrollId);
            List<Payroll> GetPayrollsForEmployee(int employeeId);
            List<Payroll> GetPayrollsForPeriod(DateTime startDate, DateTime endDate);

        decimal CalculateGrossSalary(Employee employee, decimal overtimePay);
        object CalculateNetSalary(Payroll payroll);
        decimal CalculateGrossSalary(Payroll payroll);
       
      
    }
    }