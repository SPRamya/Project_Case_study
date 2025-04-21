using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case_study_PayXpert.entity;
using Case_study_PayXpert.UI;

namespace Case_study_PayXpert.dao
{
    public interface IEmployeeService
    {
        Employee GetEmployeeById(int employeeId);
        List<Employee> GetAllEmployees();
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void RemoveEmployee(int employeeId);

      
    }
}

