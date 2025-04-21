using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case_study_PayXpert.entity
{
    public class Tax
    {
        public int TaxID { get; set; }
        public int EmployeeID { get; set; }
        public int TaxYear { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal TaxAmount { get; set; }

        public override string ToString()
        {
            return $"Tax ID: {TaxID}, Employee ID: {EmployeeID}, Year: {TaxYear}, Taxable Income: {TaxableIncome:C}, Tax Amount: {TaxAmount:C}";
        }
    }
}