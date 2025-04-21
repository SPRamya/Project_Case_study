using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case_study_PayXpert.dao;
using Case_study_PayXpert.exception;

namespace Case_study_PayXpert.UI
{
    public class TaxServiceUI
    {
        private readonly ITaxService _taxService;

        public TaxServiceUI(ITaxService taxService)
        {
            _taxService = taxService;
        }

        public void CalculateTax()
        {
            try
            {
                Console.WriteLine("=== Calculate Tax ===");
                Console.Write("Employee ID: ");
                string empIdInput = Console.ReadLine() ?? throw new InvalidInputException("Employee ID is required");
                if (!int.TryParse(empIdInput, out int empId) || empId <= 0)
                    throw new InvalidInputException("Invalid Employee ID");

                Console.Write("Tax Year (yyyy): ");
                string yearInput = Console.ReadLine() ?? throw new InvalidInputException("Tax year is required");
                if (!int.TryParse(yearInput, out int year) || year < 2000 || year > DateTime.Now.Year)
                    throw new InvalidInputException("Invalid Tax Year");

                _taxService.CalculateTax(empId, year);

                var taxes = _taxService.GetTaxesForEmployee(empId)
                    .Where(t => t.TaxYear == year)
                    .ToList();

                if (taxes.Count > 0)
                {
                    Console.WriteLine("\nTax Calculation Results:");
                    Console.WriteLine($"Employee ID: {empId}");
                    Console.WriteLine($"Tax Year: {year}");
                    Console.WriteLine($"Taxable Income: {taxes.Last().TaxableIncome:C}");
                    Console.WriteLine($"Tax Amount: {taxes.Last().TaxAmount:C}");
                }

                Console.WriteLine("Tax calculated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void GetTaxById()
        {
            try
            {
                Console.Write("Enter Tax ID: ");
                int taxId = int.Parse(Console.ReadLine());

                var tax = _taxService.GetTaxById(taxId);
                if (tax == null)
                {
                    Console.WriteLine("Tax record not found.");
                    return;
                }

                Console.WriteLine($"Tax ID: {tax.TaxID}");
                Console.WriteLine($"Employee ID: {tax.EmployeeID}");
                Console.WriteLine($"Tax Year: {tax.TaxYear}");
                Console.WriteLine($"Taxable Income: {tax.TaxableIncome:C}");
                Console.WriteLine($"Tax Amount: {tax.TaxAmount:C}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void ViewTaxesForEmployee()
        {
            try
            {
                Console.Write("Enter Employee ID: ");
                int empId = int.Parse(Console.ReadLine());
                var taxes = _taxService.GetTaxesForEmployee(empId);

                if (taxes.Count == 0)
                {
                    Console.WriteLine("No tax records found for this employee.");
                    return;
                }

                foreach (var t in taxes)
                {
                    Console.WriteLine($"Tax ID: {t.TaxID} | Tax Year: {t.TaxYear} | Taxable Income: {t.TaxableIncome:C} | Tax Amount: {t.TaxAmount:C}");
                    Console.WriteLine("-----------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void ViewTaxesForYear()
        {
            try
            {
                Console.Write("Enter Tax Year: ");
                int taxYear = int.Parse(Console.ReadLine());
                var taxes = _taxService.GetTaxesForYear(taxYear);

                if (taxes.Count == 0)
                {
                    Console.WriteLine("No tax records found for this year.");
                    return;
                }

                foreach (var t in taxes)
                {
                    Console.WriteLine($"Tax ID: {t.TaxID} | Employee ID: {t.EmployeeID} | Taxable Income: {t.TaxableIncome:C} | Tax Amount: {t.TaxAmount:C}");
                    Console.WriteLine("-----------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}