using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using IndicatorsManager.DataAccess;

namespace IndicatorsManager.DataAccess.Test
{
    [TestClass]
    public class BaseTest
    {
        protected DbContext Context;
        protected string dbName;

        [TestInitialize]
        public void InitContext()
        {
            dbName = Guid.NewGuid().ToString();
            this.Context = CreateContext(dbName);
        }

        protected DbContext CreateContext(string name)
        {
            var builder = new DbContextOptionsBuilder<DomainContext>();
            builder.UseInMemoryDatabase(name);
            return new DomainContext(builder.Options);
        }
    }
}