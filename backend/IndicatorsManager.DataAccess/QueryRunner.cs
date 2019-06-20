using System;
using System.Data.SqlClient;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;

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
                throw new DataAccessException("The connection string is null");
            }

            SqlConnection conn = null;
            object ret = null;
            SqlDataReader rdr = null;
            try
            {
                conn = new SqlConnection(this.connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                rdr = cmd.ExecuteReader();
                rdr.Read();
                ret = rdr[0];
            }
            catch(ArgumentException ae)
            {
                throw new DataAccessException("Connection String format is invalid", ae);
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