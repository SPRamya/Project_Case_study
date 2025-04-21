using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case_study_PayXpert.entity
{
    public class FinancialRecord
    {
        public int RecordID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime RecordDate { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string RecordType { get; set; }
   
     public override string ToString()
        {
            return $"Record ID: {RecordID}, Employee ID: {EmployeeID}, Date: {RecordDate.ToShortDateString()}, Type: {RecordType}, Amount: {Amount:C}, Description: {Description}";
        }
    }
}
