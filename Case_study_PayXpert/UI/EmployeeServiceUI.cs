using System;
using System.Collections.Generic;
using System.Globalization;
using Case_study_PayXpert.dao;
using Case_study_PayXpert.entity;
using Case_study_PayXpert.exception;
using Case_study_PayXpert.util;

namespace Case_study_PayXpert.UI
{
    public class EmployeeServiceUI
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeServiceUI(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public void AddEmployee()
        {
            try
            {
                Console.WriteLine("=== Add New Employee ===");
                Employee employee = GetEmployeeInput(false);
                _employeeService.AddEmployee(employee);
                Console.WriteLine(" Employee added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error: {ex.Message}");
            }
        }

        public void ViewAllEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found.");
                return;
            }

            Console.WriteLine("=== Employee List ===");
            foreach (var emp in employees)
            {
                Console.WriteLine($"ID: {emp.EmployeeID}, Name: {emp.FirstName} {emp.LastName}, Position: {emp.Position}");
            }
        }

        public void GetEmployeeById()
        {
            try
            {
                Console.Write("Enter Employee ID: ");
                int id = int.Parse(Console.ReadLine());

                var emp = _employeeService.GetEmployeeById(id);
                if (emp == null)
                {
                    Console.WriteLine("Employee not found.");
                    return;
                }

                Console.WriteLine($"ID: {emp.EmployeeID}");
                Console.WriteLine($"Name: {emp.FirstName} {emp.LastName}");
                Console.WriteLine($"DOB: {emp.DateOfBirth:yyyy-MM-dd}");
                Console.WriteLine($"Gender: {emp.Gender}");
                Console.WriteLine($"Email: {emp.Email}");
                Console.WriteLine($"Phone: {emp.PhoneNumber}");
                Console.WriteLine($"Address: {emp.Address}");
                Console.WriteLine($"Position: {emp.Position}");
                Console.WriteLine($"Joined: {emp.JoiningDate:yyyy-MM-dd}");
                Console.WriteLine($"Termination Date: {(emp.TerminationDate.HasValue ? emp.TerminationDate.Value.ToString("yyyy-MM-dd") : "Still Active")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error: {ex.Message}");
            }
        }

        public void UpdateEmployee()
        {
            try
            {
                Console.Write("Enter Employee ID to update: ");
                int id = int.Parse(Console.ReadLine());
                var employee = _employeeService.GetEmployeeById(id);
                if (employee == null) throw new EmployeeNotFoundException("Employee not found.");

                Console.WriteLine("=== Update Employee Details ===");
                Employee updated = GetEmployeeInput(true);
                updated.EmployeeID = id;
                _employeeService.UpdateEmployee(updated);
                Console.WriteLine("Employee updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error: {ex.Message}");
            }
        }

        public void RemoveEmployee()
        {
            try
            {
                Console.Write("Enter Employee ID to remove: ");
                int id = int.Parse(Console.ReadLine());
                _employeeService.RemoveEmployee(id);
                Console.WriteLine("Employee removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error: {ex.Message}");
            }
        }

        private Employee GetEmployeeInput(bool isUpdate)
        {
            Employee emp = new Employee();

          
            Console.Write("First Name: ");
            emp.FirstName = Console.ReadLine();
            ValidationService.ValidateRequiredField(emp.FirstName, "First Name");

           
            Console.Write("Last Name: ");
            emp.LastName = Console.ReadLine();
            ValidationService.ValidateRequiredField(emp.LastName, "Last Name");

            Console.Write("DOB (yyyy-MM-dd): ");
            emp.DateOfBirth = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            ValidationService.ValidateDate(emp.DateOfBirth, "DOB");

           
            Console.Write("Gender (M/F/O): ");
            emp.Gender = Console.ReadLine();
        
            Console.Write("Email: ");
            emp.Email = Console.ReadLine();
            ValidationService.ValidateEmail(emp.Email);

           
            Console.Write("Phone: ");
            emp.PhoneNumber = Console.ReadLine();
            ValidationService.ValidatePhoneNumber(emp.PhoneNumber);

            
            Console.Write("Address: ");
            emp.Address = Console.ReadLine();
            ValidationService.ValidateRequiredField(emp.Address, "Address");

            
            Console.Write("Position: ");
            emp.Position = Console.ReadLine();
            ValidationService.ValidateRequiredField(emp.Position, "Position");

            Console.Write("Joining Date (yyyy-MM-dd): ");
            emp.JoiningDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            ValidationService.ValidateDate(emp.JoiningDate, "Joining Date");

            if (isUpdate)
            {
                
                Console.Write("Termination Date (yyyy-MM-dd or leave blank): ");
                var term = Console.ReadLine();
                if (!string.IsNullOrEmpty(term))
                {
                    emp.TerminationDate = DateTime.ParseExact(term, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    ValidationService.ValidateDate(emp.TerminationDate.Value, "Termination Date");
                }
            }

            return emp;
        }
    }
}



