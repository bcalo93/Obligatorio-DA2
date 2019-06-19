using System;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;

namespace IndicatorsManager.DataAccess
{
    public class DomainContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<UserArea> UserAreas { get; set; }
        public DbSet<UserIndicator> UserIndicators { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<IndicatorItem> IndicatorItems { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<AuthenticationToken> AuthTokens { get; set; }

        public DomainContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User Area model
            modelBuilder.Entity<UserArea>().HasKey(ua => new {ua.AreaId, ua.UserId });
            modelBuilder.Entity<UserArea>().HasOne(ua => ua.Area).WithMany(a => a.UserAreas).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserArea>().HasOne(ua => ua.User).WithMany(u => u.UserAreas).OnDelete(DeleteBehavior.Cascade);

            // User Indicator model
            modelBuilder.Entity<UserIndicator>().HasKey(ua => new {ua.IndicatorId, ua.UserId });
            modelBuilder.Entity<UserIndicator>().HasOne(ua => ua.Indicator).WithMany(a => a.UserIndicators).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserIndicator>().HasOne(ua => ua.User).WithMany(u => u.UserIndicators).OnDelete(DeleteBehavior.Cascade);

            // Area model
            modelBuilder.Entity<Indicator>().HasOne(i => i.Area).WithMany(a => a.Indicators).OnDelete(DeleteBehavior.Cascade);
            
            // Indicator Item model
           modelBuilder.Entity<IndicatorItem>().HasOne(ii => ii.Indicator).WithMany(i => i.IndicatorItems).OnDelete(DeleteBehavior.Cascade);

            // Condition Models
            modelBuilder.Entity<ItemNumeric>();
            modelBuilder.Entity<ItemQuery>();
            modelBuilder.Entity<ItemText>();
            modelBuilder.Entity<ItemBoolean>();
            modelBuilder.Entity<ItemDate>();
            modelBuilder.Entity<OrCondition>();
            modelBuilder.Entity<AndCondition>();
            modelBuilder.Entity<EqualsCondition>();
            modelBuilder.Entity<MayorCondition>();
            modelBuilder.Entity<MayorEqualsCondition>();
            modelBuilder.Entity<MinorCondition>();
            modelBuilder.Entity<MinorEqualsCondition>();
        }
    }
}