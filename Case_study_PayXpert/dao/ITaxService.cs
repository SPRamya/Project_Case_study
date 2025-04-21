using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case_study_PayXpert.entity;

namespace Case_study_PayXpert.dao
{

    public interface ITaxService
    {
       
        Tax CalculateTax(int employeeId, int year);
        Tax GetTaxById(int taxId);
        List<Tax> GetTaxesForEmployee(int employeeId);
        List<Tax> GetTaxesForYear(int taxYear);





    }
}
    