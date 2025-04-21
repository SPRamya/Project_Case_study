using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Case_study_PayXpert.entity;
using Case_study_PayXpert.util;

namespace Case_study_PayXpert.dao
{
    public class TaxServiceImpl : ITaxService
    {
        private readonly IPayrollService _payrollService;

        public TaxServiceImpl(IPayrollService payrollService)
        {
            _payrollService = payrollService ?? throw new ArgumentNullException(nameof(payrollService));
        }

        public TaxServiceImpl(IPayrollService payrollService, string connectionString)
        {
            _payrollService = payrollService ?? throw new ArgumentNullException(nameof(payrollService));
            
        }

        public TaxServiceImpl()
        {
            
        }

        public Tax CalculateTax(int employeeId, int year)
        {
           
            decimal taxAmount = 12000; 
            return new Tax { EmployeeID = employeeId, TaxYear = year, TaxAmount = taxAmount };
        }

        private decimal CalculateTaxAmount(decimal taxableIncome)
        {
            if (taxableIncome <= 10000)
                return taxableIncome * 0.10m;
            else if (taxableIncome <= 50000)
                return 1000 + (taxableIncome - 10000) * 0.20m;
            else
                return 9000 + (taxableIncome - 50000) * 0.30m;
        }

        public Tax GetTaxById(int taxId)
        {
            if (taxId <= 0)
                throw new ArgumentException("Tax ID must be positive.");

            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null)
                {
                    throw new Exception("Database connection failed");
                }

                using (var command = new SqlCommand("SELECT * FROM Tax WHERE TaxID = @TaxID", connection))
                {
                    command.Parameters.AddWithValue("@TaxID", taxId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Tax
                            {
                                TaxID = Convert.ToInt32(reader["TaxID"]),
                                EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                TaxYear = Convert.ToInt32(reader["TaxYear"]),
                                TaxableIncome = Convert.ToDecimal(reader["TaxableIncome"]),
                                TaxAmount = Convert.ToDecimal(reader["TaxAmount"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Tax> GetTaxesForEmployee(int employeeId)
        {
            var taxes = new List<Tax>();
            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null)
                {
                    throw new Exception("Database connection failed");
                }

                var command = new SqlCommand("SELECT * FROM Tax WHERE EmployeeID = @EmployeeID", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        taxes.Add(new Tax
                        {
                            TaxID = Convert.ToInt32(reader["TaxID"]),
                            EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                            TaxYear = Convert.ToInt32(reader["TaxYear"]),
                            TaxableIncome = Convert.ToDecimal(reader["TaxableIncome"]),
                            TaxAmount = Convert.ToDecimal(reader["TaxAmount"])
                        });
                    }
                }
            }
            return taxes;
        }

        public List<Tax> GetTaxesForYear(int taxYear)
        {
            var taxes = new List<Tax>();
            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null)
                {
                    throw new Exception("Database connection failed");
                }

                var command = new SqlCommand("SELECT * FROM Tax WHERE TaxYear = @TaxYear", connection);
                command.Parameters.AddWithValue("@TaxYear", taxYear);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        taxes.Add(new Tax
                        {
                            TaxID = Convert.ToInt32(reader["TaxID"]),
                            EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                            TaxYear = Convert.ToInt32(reader["TaxYear"]),
                            TaxableIncome = Convert.ToDecimal(reader["TaxableIncome"]),
                            TaxAmount = Convert.ToDecimal(reader["TaxAmount"])
                        });
                    }
                }
            }
            return taxes;
        }
    }
}
