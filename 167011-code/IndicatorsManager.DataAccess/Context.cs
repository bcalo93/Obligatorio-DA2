using System;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;

namespace IndicatorsManager.DataAccess
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<Condition> Conditions { get; set; }
     
        public DbSet<Expression> Expressions { get; set; }

        public Context(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserArea>().HasKey(ua => new { ua.UserId, ua.AreaId });
            modelBuilder.Entity<UserIndicator>().HasKey(ui => new { ui.UserId, ui.IndicatorId });

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}