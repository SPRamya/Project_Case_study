using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case_study_PayXpert.entity;
using Case_study_PayXpert.util;

namespace Case_study_PayXpert.dao
{
    public class FinancialRecordServiceImpl : IFinancialRecordService
    {
        public void AddFinancialRecord(int employeeId, string description, decimal amount, string recordType)
        {
            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null) return;

               
                using (var checkCommand = new SqlCommand("SELECT COUNT(1) FROM Employee WHERE EmployeeID = @EmployeeID", connection))
                {
                    checkCommand.Parameters.AddWithValue("@EmployeeID", employeeId);
                    var result = (int)checkCommand.ExecuteScalar();
                    if (result == 0)
                    {
                        throw new Exception("Employee ID does not exist in the Employee table.");
                    }
                }

              
                var command = new SqlCommand(
                    "INSERT INTO FinancialRecord (EmployeeID, RecordDate, Description, Amount, RecordType) " +
                    "VALUES (@EmployeeID, @RecordDate, @Description, @Amount, @RecordType)", connection);

                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@RecordDate", DateTime.Now);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@RecordType", recordType);

                command.ExecuteNonQuery();
            }
        }
        public FinancialRecord GetFinancialRecordById(int recordId)
        {
            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null) return null;

                var command = new SqlCommand("SELECT * FROM FinancialRecord WHERE RecordID = @RecordID", connection);
                command.Parameters.AddWithValue("@RecordID", recordId);

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new FinancialRecord
                    {
                        RecordID = Convert.ToInt32(reader["RecordID"]),
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        RecordDate = Convert.ToDateTime(reader["RecordDate"]),
                        Description = reader["Description"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        RecordType = reader["RecordType"].ToString()
                    };
                }

                return null;
            }
        }

        public List<FinancialRecord> GetFinancialRecordsForEmployee(int employeeId)
        {
            var records = new List<FinancialRecord>();
            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null) return records;

                var command = new SqlCommand("SELECT * FROM FinancialRecord WHERE EmployeeID = @EmployeeID", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    records.Add(new FinancialRecord
                    {
                        RecordID = Convert.ToInt32(reader["RecordID"]),
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        RecordDate = Convert.ToDateTime(reader["RecordDate"]),
                        Description = reader["Description"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        RecordType = reader["RecordType"].ToString()
                    });
                }
            }

            return records;
        }

        public List<FinancialRecord> GetFinancialRecordsForDate(DateTime recordDate)
        {
            var records = new List<FinancialRecord>();
            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null) return records;

                var command = new SqlCommand("SELECT * FROM FinancialRecord WHERE RecordDate = @RecordDate", connection);
                command.Parameters.AddWithValue("@RecordDate", recordDate);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    records.Add(new FinancialRecord
                    {
                        RecordID = Convert.ToInt32(reader["RecordID"]),
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        RecordDate = Convert.ToDateTime(reader["RecordDate"]),
                        Description = reader["Description"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        RecordType = reader["RecordType"].ToString()
                    });
                }
            }

            return records;
        }

        public void RemoveFinancialRecord(int recordId)
        {
            using (var connection = DBPropertyUtil.GetConnection())
            {
                if (connection == null) return;

                var command = new SqlCommand("DELETE FROM FinancialRecord WHERE RecordID = @RecordID", connection);
                command.Parameters.AddWithValue("@RecordID", recordId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new Exception("No financial record found with the specified ID.");
                }
            }
        }
    }
}
