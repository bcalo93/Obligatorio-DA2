using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IndicatorsManager.Logger.Interface;
using IndicatorsManager.Logger.Interface.Exceptions;

namespace IndicatorsManager.Logger.Database
{
    public class LoggerDatabase : ILogger
    {
        private const string ERROR_CONNECTION = "The service is unavailable.";
        public LoggerDatabase() { }

        public IEnumerable<Log> GetLogActions(DateTime start, DateTime end)
        {
            List<Log> result = new List<Log>();
            using(var context = new LogContext(CreateOptionContext()))
            {
                try
                {
                    result = context.Logs
                        .Where(l => l.LogDate >= start && l.LogDate <= end)
                        .OrderBy(l => l.LogDate)
                        .ToList();
                    return result;
                }
                catch(SqlException se)
                {
                    throw new LoggerException(ERROR_CONNECTION, se);
                }
            }
        }

        public IEnumerable<string> GetMostLoggedInUsers()
        {
            List<string> result = new List<string>();
            using(var context = new LogContext(CreateOptionContext()))
            {
                try
                {
                    result = context.Logs
                        .Where(l => l.LogType == "login")
                        .GroupBy(l => l.Username)
                        .OrderByDescending(l => l.Count())
                        .Select(i => i.Key).ToList();
                }
                catch(SqlException se)
                {
                    throw new LoggerException(ERROR_CONNECTION, se);
                }
            }
            return result;
        }

        public void LogAction(string username, string actionType)
        {
            using(var context = new LogContext(CreateOptionContext()))
            {
                Log create = new Log
                {
                    Username = username,
                    LogType = actionType,
                    LogDate = DateTime.Now
                };
                try
                {
                    context.Add(create);
                    context.SaveChanges();
                }
                catch(DbUpdateException de) 
                { 
                    throw new LoggerException(ERROR_CONNECTION, de);
                }
            }
        }

        private DbContextOptions CreateOptionContext()
        {
            return new DbContextOptionsBuilder<LogContext>()
                .UseSqlServer(ConnectionStringHandler.Instance.ConnectionString)
                .Options;
        }
    }
}
