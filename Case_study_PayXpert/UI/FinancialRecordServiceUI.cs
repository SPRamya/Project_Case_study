using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case_study_PayXpert.dao;

namespace Case_study_PayXpert.UI
{
    public class FinancialRecordServiceUI
    {
        private readonly IFinancialRecordService _financialRecordService;

        public FinancialRecordServiceUI(IFinancialRecordService financialRecordService)
        {
            _financialRecordService = financialRecordService;
        }

        public void AddFinancialRecord()
        {
            try
            {
                Console.Write("Employee ID: ");
                int empId = int.Parse(Console.ReadLine());
                Console.Write("Description: ");
                string desc = Console.ReadLine();
                Console.Write("Amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                Console.Write("Record Type (Income/Expense): ");
                string type = Console.ReadLine();

                _financialRecordService.AddFinancialRecord(empId, desc, amount, type);
                Console.WriteLine("Financial record added.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error: {ex.Message}");
            }
        }

        public void ViewEmployeeFinancialRecords()
        {
            Console.Write("Enter Employee ID: ");
            int empId = int.Parse(Console.ReadLine());
            var records = _financialRecordService.GetFinancialRecordsForEmployee(empId);

            if (records.Count == 0)
            {
                Console.WriteLine("No records found.");
                return;
            }

            foreach (var r in records)
            {
                Console.WriteLine($"{r.RecordDate:yyyy-MM-dd} | {r.RecordType} | {r.Description} | {r.Amount:C}");
            }
        }

        public void GetFinancialRecordById()
        {
            try
            {
                Console.Write("Enter Financial Record ID: ");
                int recordId = int.Parse(Console.ReadLine());

                var record = _financialRecordService.GetFinancialRecordById(recordId);
                if (record == null)
                {
                    Console.WriteLine("Financial record not found.");
                    return;
                }

                Console.WriteLine($"Record ID: {record.RecordID}");
                Console.WriteLine($"Employee ID: {record.EmployeeID}");
                Console.WriteLine($"Record Date: {record.RecordDate:yyyy-MM-dd}");
                Console.WriteLine($"Description: {record.Description}");
                Console.WriteLine($"Amount: {record.Amount:C}");
                Console.WriteLine($"Record Type: {record.RecordType}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error: {ex.Message}");
            }
        }

        public void GetFinancialRecordsForEmployee()
        {
            try
            {
                Console.Write("Enter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());

                var records = _financialRecordService.GetFinancialRecordsForEmployee(employeeId);
                if (records.Count == 0)
                {
                    Console.WriteLine(" No financial records found for this employee.");
                    return;
                }

                foreach (var record in records)
                {
                    Console.WriteLine($"Record ID: {record.RecordID} | Date: {record.RecordDate:yyyy-MM-dd} | Amount: {record.Amount:C} | Type: {record.RecordType}");
                    Console.WriteLine("-----------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void GetFinancialRecordsForDate()
        {
            try
            {
                Console.Write("Enter Record Date (yyyy-MM-dd): ");
                DateTime recordDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var records = _financialRecordService.GetFinancialRecordsForDate(recordDate);
                if (records.Count == 0)
                {
                    Console.WriteLine(" No financial records found for this date.");
                    return;
                }

                foreach (var record in records)
                {
                    Console.WriteLine($"Record ID: {record.RecordID} | Employee ID: {record.EmployeeID} | Amount: {record.Amount:C} | Type: {record.RecordType}");
                    Console.WriteLine("-----------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void RemoveFinancialRecord()
        {
            try
            {
                Console.Write("Enter Financial Record ID to remove: ");
                int recordId = int.Parse(Console.ReadLine());

                _financialRecordService.RemoveFinancialRecord(recordId);
                Console.WriteLine("Financial Record removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}