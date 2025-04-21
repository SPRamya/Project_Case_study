using Case_study_PayXpert.dao;
using Case_study_PayXpert.entity;
using Case_study_PayXpert.exception;
using Case_study_PayXpert.UI;
using System.Globalization;

namespace Case_study_PayXpert
{
    class Program
    {
        private static readonly IEmployeeService _employeeService = new EmployeeServiceImpl();
        private static readonly IPayrollService _payrollService = new PayrollServiceImpl(_employeeService);
        private static readonly ITaxService _taxService = new TaxServiceImpl(_payrollService);
        private static readonly IFinancialRecordService _financialRecordService = new FinancialRecordServiceImpl();

        private static readonly EmployeeServiceUI _employeeUI = new EmployeeServiceUI(_employeeService);
        private static readonly PayrollServiceUI _payrollUI = new PayrollServiceUI(_payrollService);
        private static readonly TaxServiceUI _taxUI = new TaxServiceUI(_taxService);
        private static readonly FinancialRecordServiceUI _financialUI = new FinancialRecordServiceUI(_financialRecordService);


        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n========== PAYXPERT PAYROLL MANAGEMENT ==========");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. View All Employees");
                Console.WriteLine("3. Get Employee by ID");
                Console.WriteLine("4. Update Employee");
                Console.WriteLine("5. Remove Employee");
                Console.WriteLine("6. Generate Payroll");
                Console.WriteLine("7. View Employee Payroll");
                Console.WriteLine("8. Get Payroll by ID");
                Console.WriteLine("9. View Payrolls for Employee");
                Console.WriteLine("10. View Payrolls for Period");
                Console.WriteLine("11. Calculate Tax");
                Console.WriteLine("12. Get Tax By ID");
                Console.WriteLine("13. View Taxes For Employee");
                Console.WriteLine("14. View Taxes For Year");
                Console.WriteLine("15. Add Financial Record");
                Console.WriteLine("16. View Employee Financial Records");
                Console.WriteLine("17. Get Financial Record by ID");
                Console.WriteLine("18. Get Financial Records for Employee");
                Console.WriteLine("19. Get Financial Records for a Specific Date");
                Console.WriteLine("20. Remove Financial Record");
                Console.WriteLine("0. Exit");
                Console.WriteLine("==================================================");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1": _employeeUI.AddEmployee(); break;
                    case "2": _employeeUI.ViewAllEmployees(); break;
                    case "3": _employeeUI.GetEmployeeById(); break;
                    case "4": _employeeUI.UpdateEmployee(); break;
                    case "5": _employeeUI.RemoveEmployee(); break;
                    case "6": _payrollUI.GeneratePayroll(); break;
                    case "7": _payrollUI.ViewEmployeePayroll(); break;
                    case "8": _payrollUI.GetPayrollById(); break;
                    case "9": _payrollUI.ViewPayrollsForEmployee(); break;
                    case "10": _payrollUI.ViewPayrollsForPeriod(); break;
                    case "11": _taxUI.CalculateTax(); break;
                    case "12": _taxUI.GetTaxById(); break;
                    case "13": _taxUI.ViewTaxesForEmployee(); break;
                    case "14": _taxUI.ViewTaxesForYear(); break;
                    case "15": _financialUI.AddFinancialRecord(); break;
                    case "16": _financialUI.ViewEmployeeFinancialRecords(); break;
                    case "17": _financialUI.GetFinancialRecordById(); break;
                    case "18": _financialUI.GetFinancialRecordsForEmployee(); break;
                    case "19": _financialUI.GetFinancialRecordsForDate(); break;
                    case "20": _financialUI.RemoveFinancialRecord(); break;
                    case "0":
                        exit = true;
                        Console.WriteLine("Exiting... Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}

