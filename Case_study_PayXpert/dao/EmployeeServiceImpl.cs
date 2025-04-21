using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case_study_PayXpert.entity;
using Case_study_PayXpert.util;

namespace Case_study_PayXpert.dao
{

    public class EmployeeServiceImpl : IEmployeeService
    {
        public Employee GetEmployeeById(int employeeId)
        {
            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null) return null;

                var command = new SqlCommand("SELECT * FROM Employee WHERE EmployeeID = @EmployeeID", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Employee
                    {
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Gender = reader["Gender"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Position = reader["Position"].ToString(),
                        JoiningDate = Convert.ToDateTime(reader["JoiningDate"]),
                        TerminationDate = reader["TerminationDate"] as DateTime?
                    };
                }
                return null;
            }
        }

        
        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();
            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null) return employees;

                var command = new SqlCommand("SELECT * FROM Employee", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Gender = reader["Gender"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Position = reader["Position"].ToString(),
                        JoiningDate = Convert.ToDateTime(reader["JoiningDate"]),
                        TerminationDate = reader["TerminationDate"] as DateTime?
                    });
                }
            }
            return employees;
        }

        
        public void AddEmployee(Employee employee)
        {
            ValidationService.ValidateRequiredField(employee.FirstName, "First Name");
            ValidationService.ValidateRequiredField(employee.LastName, "Last Name");
            ValidationService.ValidateEmail(employee.Email);
            ValidationService.ValidatePhoneNumber(employee.PhoneNumber);
            ValidationService.ValidateDate(employee.JoiningDate, "Join Date");

            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null) return;

                var command = new SqlCommand(@"INSERT INTO Employee (FirstName, LastName, DateOfBirth, Gender, Email, PhoneNumber, Address, Position, JoiningDate) 
                                              VALUES (@FirstName, @LastName, @DateOfBirth, @Gender, @Email, @PhoneNumber, @Address, @Position, @JoiningDate)", connection);

                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", employee.Gender);
                command.Parameters.AddWithValue("@Email", employee.Email);
                command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                command.Parameters.AddWithValue("@Address", employee.Address);
                command.Parameters.AddWithValue("@Position", employee.Position);
                command.Parameters.AddWithValue("@JoiningDate", employee.JoiningDate);

                command.ExecuteNonQuery();
            }
        }

     
        public void UpdateEmployee(Employee employee)
        {
            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null) return;

                var command = new SqlCommand(@"UPDATE Employee SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, 
                                              Gender = @Gender, Email = @Email, PhoneNumber = @PhoneNumber, Address = @Address, 
                                              Position = @Position, JoiningDate = @JoiningDate, TerminationDate = @TerminationDate 
                                              WHERE EmployeeID = @EmployeeID", connection);

                command.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", employee.Gender);
                command.Parameters.AddWithValue("@Email", employee.Email);
                command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                command.Parameters.AddWithValue("@Address", employee.Address);
                command.Parameters.AddWithValue("@Position", employee.Position);
                command.Parameters.AddWithValue("@JoiningDate", employee.JoiningDate);
                command.Parameters.AddWithValue("@TerminationDate", (object?)employee.TerminationDate ?? DBNull.Value);

                command.ExecuteNonQuery();
            }
        }

       
        public void RemoveEmployee(int employeeId)
        {
            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null) return;

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                       
                        var deletePayroll = new SqlCommand("DELETE FROM Payroll WHERE EmployeeID = @EmployeeID", connection, transaction);
                        deletePayroll.Parameters.AddWithValue("@EmployeeID", employeeId);
                        deletePayroll.ExecuteNonQuery();

                        var deleteTax = new SqlCommand("DELETE FROM Tax WHERE EmployeeID = @EmployeeID", connection, transaction);
                        deleteTax.Parameters.AddWithValue("@EmployeeID", employeeId);
                        deleteTax.ExecuteNonQuery();

                        var deleteFinancial = new SqlCommand("DELETE FROM FinancialRecord WHERE EmployeeID = @EmployeeID", connection, transaction);
                        deleteFinancial.Parameters.AddWithValue("@EmployeeID", employeeId);
                        deleteFinancial.ExecuteNonQuery();

                      
                        var deleteEmployee = new SqlCommand("DELETE FROM Employee WHERE EmployeeID = @EmployeeID", connection, transaction);
                        deleteEmployee.Parameters.AddWithValue("@EmployeeID", employeeId);
                        deleteEmployee.ExecuteNonQuery();

                      
                        transaction.Commit();
                        Console.WriteLine("Employee and related records deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}

   
