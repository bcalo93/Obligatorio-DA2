using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using System.Data.SqlClient;

namespace IndicatorsManager.DataAccess
{
    public class LogRepository : BaseRepository<Log>, ILogQuery
    {
        public LogRepository(DbContext context) : base(context) { }
        
        public override Log Get(Guid id)
        {
            try
            {
                return this.context.Set<Log>()
                    .Where(u => u.Id == id)
                    .FirstOrDefault();
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

        public override IEnumerable<Log> GetAll()
        {
            try
            {
                return this.context.Set<Log>().ToList();
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

        public IEnumerable<User> GetUsersMostLogs(int limit)
        {
            try
            {
                return this.context.Set<Log>()
                    .GroupBy(l => l.User)
                    .OrderByDescending(l => l.Count())
                    .Take(limit)
                    .Select(l => l.Key);
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }
    }
}