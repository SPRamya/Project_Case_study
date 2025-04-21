using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case_study_PayXpert.entity;

namespace Case_study_PayXpert.dao
{
    public interface IFinancialRecordService
    {
        void AddFinancialRecord(int employeeId, string description, decimal amount, string recordType);
        FinancialRecord GetFinancialRecordById(int recordId);
        List<FinancialRecord> GetFinancialRecordsForEmployee(int employeeId);
        List<FinancialRecord> GetFinancialRecordsForDate(DateTime recordDate);
        void RemoveFinancialRecord(int recordId);
    }
}