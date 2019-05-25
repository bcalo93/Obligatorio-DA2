using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.DataAccess
{
    public class LogRepository : BaseRepository<Log>, ILogQuery
    {
        public LogRepository(DbContext context) : base(context) { }
        
        public override Log Get(Guid id)
        {
            return this.context.Set<Log>().Where(u => u.Id == id).FirstOrDefault();
        }

        public override IEnumerable<Log> GetAll()
        {
            return this.context.Set<Log>().ToList();
        }

        public IEnumerable<User> GetUsersMostLogs(int limit)
        {
            return this.context.Set<Log>()
                .GroupBy(l => l.User)
                .OrderByDescending(l => l.Count())
                .Take(limit)
                .Select(l => l.Key);
        }
        
    }
}