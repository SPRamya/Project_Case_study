using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case_study_PayXpert.util
{

    public static class DBPropertyUtil
    {
       
        private static readonly string connectionString = @"Server=LAPTOP-T9L5C12T; Database = case_study; Integrated Security=True; MultipleActiveResultSets=true;";

        
        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Opening the Connection: {ex.Message}");
                return null;
            }
        }
    }
}