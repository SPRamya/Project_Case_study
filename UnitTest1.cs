using NUnit.Framework;
using System;
using Case_study_PayXpert.dao;
using Case_study_PayXpert.entity;
using Case_study_PayXpert.exception;


namespace testing
{
   
  [TestFixture]
public class Tests
{
private SalaryCalculator _salaryCalculator;

[SetUp]
public void Setup()
{
    _salaryCalculator = new SalaryCalculator();
}

[Test]
public void CalculateGrossSalary_WithAllComponents_ReturnsCorrectSum()
{
    var employee = new Employee
    {
        EmployeeId = "EMP001",
        BaseSalary = 50000m,
        HousingAllowanceRate = 0.10m,
        TransportationAllowance = 2400m,
        BonusRate = 0.05m
    };

  
    decimal expected = 59900m;

    
    decimal actual = _salaryCalculator.CalculateGrossSalary(employee);

    
    Assert.AreEqual(expected, actual);
}

[Test]
public void CalculateGrossSalary_WithNoAllowances_ReturnsBaseSalaryOnly()
{
    var employee = new Employee
    {
        EmployeeId = "EMP002",
        BaseSalary = 40000m,
        HousingAllowanceRate = 0m,
        TransportationAllowance = 0m,
        BonusRate = 0m
    };

    
    decimal expected = 40000m;

  
    decimal actual = _salaryCalculator.CalculateGrossSalary(employee);

    Assert.AreEqual(expected, actual);
}

[Test]
public void CalculateGrossSalary_WithOnlyHousingAllowance_ReturnsCorrectSum()
{
            var employee = new Employee
    {
        EmployeeId = "EMP003",
        BaseSalary = 30000m,
        HousingAllowanceRate = 0.15m,
        TransportationAllowance = 0m,
        BonusRate = 0m
    };

    
    decimal expected = 34500m;

    
    decimal actual = _salaryCalculator.CalculateGrossSalary(employee);

    
    Assert.AreEqual(expected, actual);
}

[Test]
public void CalculateGrossSalary_WithZeroBaseSalary_ThrowsArgumentException()
{
    var employee = new Employee
    {
        EmployeeId = "EMP004",
        BaseSalary = 0m,
        HousingAllowanceRate = 0.10m,
        TransportationAllowance = 2400m,
        BonusRate = 0.05m
    };

   
    Assert.Throws<ArgumentException>(() => _salaryCalculator.CalculateGrossSalary(employee));
}

[Test]
public void CalculateGrossSalary_WithNullEmployee_ThrowsArgumentNullException()
{
    
    Employee employee = null;

    
    Assert.Throws<ArgumentNullException>(() => _salaryCalculator.CalculateGrossSalary(employee));
}
}

public class Employee
{
public string EmployeeId { get; set; }
        public int EmployeeID { get; internal set; }
        public decimal BaseSalary { get; set; }
public decimal HousingAllowanceRate { get; set; }
public decimal TransportationAllowance { get; set; }
public decimal BonusRate { get; set; }
        public string Position { get; internal set; }
    }

public class SalaryCalculator
{
public decimal CalculateGrossSalary(Employee employee)
{
    if (employee == null)
    {
        throw new ArgumentNullException(nameof(employee), "Employee object cannot be null");
    }

    if (employee.BaseSalary <= 0)
    {
        throw new ArgumentException("Base salary must be greater than zero", nameof(employee.BaseSalary));
    }

    decimal housingAllowance = employee.BaseSalary * employee.HousingAllowanceRate;
    decimal bonus = employee.BaseSalary * employee.BonusRate;

    return employee.BaseSalary
           + housingAllowance
           + employee.TransportationAllowance
           + bonus;
}
}
}