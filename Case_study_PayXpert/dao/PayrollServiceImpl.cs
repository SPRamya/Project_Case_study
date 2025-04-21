using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case_study_PayXpert.entity;
using Case_study_PayXpert.exception;
using Case_study_PayXpert.util;

namespace Case_study_PayXpert.dao
{

public class PayrollServiceImpl : IPayrollService
 {
     private readonly string _connectionString;
     private readonly IEmployeeService _employeeService;

     public PayrollServiceImpl(IEmployeeService employeeService)
     {
         _employeeService = employeeService;
         _connectionString = @"Server=LAPTOP-T9L5C12T;Database=case_study;Integrated Security=True;";
     }

     public PayrollServiceImpl(IEmployeeService employeeService, string connectionString)
     {
         _employeeService = employeeService;
         _connectionString = connectionString;
     }

     public decimal CalculateGrossSalary(Employee employee)
     {
         decimal baseSalary = GetBaseSalary(employee.Position);
         decimal overtimePay = CalculateOvertime(employee.EmployeeID, DateTime.Now.AddMonths(-1), DateTime.Now);
         return baseSalary + overtimePay;
     }

     public decimal CalculateNetSalary(decimal gross, decimal deductions)
     {
         return gross - deductions;
     }

     public void GeneratePayroll(int employeeId, DateTime startDate, DateTime endDate)
     {
         if (startDate > endDate)
         {
             throw new InvalidInputException("Start date must be before end date.");
         }

         ValidationService.ValidateDate(startDate, "Start Date");
         ValidationService.ValidateDate(endDate, "End Date");
         if (employeeId <= 0) throw new ArgumentException("Invalid Employee ID");
         if (startDate >= endDate) throw new ArgumentException("End date must be after start date");

         var employee = _employeeService.GetEmployeeById(employeeId);
         if (employee == null) throw new Exception("Employee not found");

         decimal basicSalary = GetBaseSalary(employee.Position);
         decimal overtimePay = CalculateOvertime(employeeId, startDate, endDate);
         decimal deductions = CalculateDeductions(employeeId);
         decimal netSalary = basicSalary + overtimePay - deductions;

         if (netSalary < 0) throw new Exception("Net salary cannot be negative");

         using (var connection = new SqlConnection(_connectionString))
         {
             connection.Open();
             using (var command = new SqlCommand(
                 "INSERT INTO Payroll (EmployeeID, PayPeriodStartDate, PayPeriodEndDate, BasicSalary, OvertimePay, Deductions, NetSalary) " +
                 "VALUES (@EmployeeID, @StartDate, @EndDate, @BasicSalary, @OvertimePay, @Deductions, @NetSalary)",
                 connection))
             {
                 command.Parameters.AddWithValue("@EmployeeID", employeeId);
                 command.Parameters.AddWithValue("@StartDate", startDate);
                 command.Parameters.AddWithValue("@EndDate", endDate);
                 command.Parameters.AddWithValue("@BasicSalary", basicSalary);
                 command.Parameters.AddWithValue("@OvertimePay", overtimePay);
                 command.Parameters.AddWithValue("@Deductions", deductions);
                 command.Parameters.AddWithValue("@NetSalary", netSalary);

                 command.ExecuteNonQuery();
             }
         }
     }

     private decimal GetBaseSalary(string position)
     {
         return position.ToLower() switch
         {
             "manager" => 7000.00m,
             "software engineer" => 5000.00m,
             "data analyst" => 4500.00m,
             "hr manager" => 5500.00m,
             _ => 4000.00m
         };
     }

     private decimal CalculateOvertime(int employeeId, DateTime startDate, DateTime endDate)
     {
         using (var connection = new SqlConnection(_connectionString))
         {
             connection.Open();
             using (var command = new SqlCommand("IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Overtime' AND xtype='U') " +
                                               "BEGIN CREATE TABLE Overtime (EmployeeID INT, Date DATE, Hours DECIMAL(5,2)) END", connection))
             {
                 command.ExecuteNonQuery();
             }

             using (var command = new SqlCommand("SELECT SUM(Hours) FROM Overtime WHERE EmployeeID = @EmployeeID AND Date BETWEEN @StartDate AND @EndDate", connection))
             {
                 command.Parameters.AddWithValue("@EmployeeID", employeeId);
                 command.Parameters.AddWithValue("@StartDate", startDate);
                 command.Parameters.AddWithValue("@EndDate", endDate);

                 var result = command.ExecuteScalar();
                 decimal hours = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                 return hours * 100; 
             }
         }
     }

     private decimal CalculateDeductions(int employeeId)
     {
        
         using (var connection = new SqlConnection(_connectionString))
         {
             connection.Open();
             using (var command = new SqlCommand("IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Deductions' AND xtype='U') " +
                                               "BEGIN CREATE TABLE Deductions (EmployeeID INT, Amount DECIMAL(10,2)) END", connection))
             {
                 command.ExecuteNonQuery();
             }

             using (var command = new SqlCommand("SELECT SUM(Amount) FROM Deductions WHERE EmployeeID = @EmployeeID", connection))
             {
                 command.Parameters.AddWithValue("@EmployeeID", employeeId);

                 var result = command.ExecuteScalar();
                 return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
             }
         }
     }

     public Payroll GetPayrollById(int payrollId)
     {
         using (var connection = new SqlConnection(_connectionString))
         {
             connection.Open();
             using (var command = new SqlCommand("SELECT * FROM Payroll WHERE PayrollID = @PayrollID", connection))
             {
                 command.Parameters.AddWithValue("@PayrollID", payrollId);

                 using (var reader = command.ExecuteReader())
                 {
                     if (reader.Read())
                     {
                         return new Payroll
                         {
                             PayrollID = Convert.ToInt32(reader["PayrollID"]),
                             EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                             PayPeriodStartDate = Convert.ToDateTime(reader["PayPeriodStartDate"]),
                             PayPeriodEndDate = Convert.ToDateTime(reader["PayPeriodEndDate"]),
                             BasicSalary = Convert.ToDecimal(reader["BasicSalary"]),
                             OvertimePay = Convert.ToDecimal(reader["OvertimePay"]),
                             Deductions = Convert.ToDecimal(reader["Deductions"]),
                             NetSalary = Convert.ToDecimal(reader["NetSalary"])
                         };
                     }
                 }
             }
         }
         return null;
     }

     public List<Payroll> GetPayrollsForEmployee(int employeeId)
     {
         var payrolls = new List<Payroll>();
         using (var connection = new SqlConnection(_connectionString))
         {
             connection.Open();
             using (var command = new SqlCommand("SELECT * FROM Payroll WHERE EmployeeID = @EmployeeID", connection))
             {
                 command.Parameters.AddWithValue("@EmployeeID", employeeId);

                 using (var reader = command.ExecuteReader())
                 {
                     while (reader.Read())
                     {
                         payrolls.Add(new Payroll
                         {
                             PayrollID = Convert.ToInt32(reader["PayrollID"]),
                             EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                             PayPeriodStartDate = Convert.ToDateTime(reader["PayPeriodStartDate"]),
                             PayPeriodEndDate = Convert.ToDateTime(reader["PayPeriodEndDate"]),
                             BasicSalary = Convert.ToDecimal(reader["BasicSalary"]),
                             OvertimePay = Convert.ToDecimal(reader["OvertimePay"]),
                             Deductions = Convert.ToDecimal(reader["Deductions"]),
                             NetSalary = Convert.ToDecimal(reader["NetSalary"])
                         });
                     }
                 }
             }
         }
         return payrolls;
     }

     public List<Payroll> GetPayrollsForPeriod(DateTime startDate, DateTime endDate)
     {
         var payrolls = new List<Payroll>();
         using (var connection = new SqlConnection(_connectionString))
         {
             connection.Open();
             using (var command = new SqlCommand(
                 "SELECT * FROM Payroll WHERE PayPeriodStartDate >= @StartDate AND PayPeriodEndDate <= @EndDate",
                 connection))
             {
                 command.Parameters.AddWithValue("@StartDate", startDate);
                 command.Parameters.AddWithValue("@EndDate", endDate);

                 using (var reader = command.ExecuteReader())
                 {
                     while (reader.Read())
                     {
                         payrolls.Add(new Payroll
                         {
                             PayrollID = Convert.ToInt32(reader["PayrollID"]),
                             EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                             PayPeriodStartDate = Convert.ToDateTime(reader["PayPeriodStartDate"]),
                             PayPeriodEndDate = Convert.ToDateTime(reader["PayPeriodEndDate"]),
                             BasicSalary = Convert.ToDecimal(reader["BasicSalary"]),
                             OvertimePay = Convert.ToDecimal(reader["OvertimePay"]),
                             Deductions = Convert.ToDecimal(reader["Deductions"]),
                             NetSalary = Convert.ToDecimal(reader["NetSalary"])
                         });
                     }
                 }
             }
         }
         return payrolls;
     }

     public decimal CalculateGrossSalary(Employee employee, decimal overtimePay)
     {
         throw new NotImplementedException();
     }

     public object CalculateNetSalary(Payroll payroll)
     {
         if (payroll == null)
             throw new ArgumentNullException(nameof(payroll));

         decimal grossSalary = payroll.BasicSalary + payroll.OvertimePay;
         return grossSalary;

     }

     public decimal CalculateGrossSalary(Payroll payroll)
     {
         if (payroll == null)
             throw new ArgumentNullException(nameof(payroll));

         decimal grossSalary = CalculateGrossSalary(payroll);
         payroll.NetSalary = grossSalary - payroll.Deductions;

         return payroll.NetSalary;
     }
 }
}

 