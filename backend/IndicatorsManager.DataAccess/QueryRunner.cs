using System;
using System.Data;
using System.Data.SqlClient;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IndicatorsManager.DataAccess
{
    public class QueryRunner : IQueryRunner
    {
        private string connectionString; 
        
        public QueryRunner() { }
        public void SetConnectionString(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public object RunQuery(string query)
        {
            if(this.connectionString == null)
            {
                throw new DataAccessException("El connection string es null");
            }
            SqlConnection conn = new SqlConnection(this.connectionString);

            SqlDataReader rdr = null;

            object ret = null;
            
            try
            {
                conn.Open();
                
                SqlCommand cmd = new SqlCommand(query, conn);
                
                rdr = cmd.ExecuteReader();
                rdr.Read();
                ret = rdr[0];

            }
            catch(SqlException ex) {
                throw new DataAccessException(ex.Message, ex);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
    
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return ret;
        }
    }
}