using Microsoft.EntityFrameworkCore;
using IndicatorsManager.Logger.Interface;

namespace IndicatorsManager.Logger.Database
{
    public class LogContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        public LogContext(DbContextOptions options) : base(options) { }
    }
}