using System;
using System.Data;
using System.Data.SqlClient;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IndicatorsManager.DataAccess;

namespace IndicatorsManager.DataAccess.Test
{
    [TestClass]
    public class QueryRunnerTest : BaseTest
    {

        private const string CONNECTION_STRING = "Server=.\\SQLEXPRESS;Database=DatosPrueba;Trusted_Connection=True;MultipleActiveResultSets=True;";

        [TestMethod]
        public void RunQueryOKTest()
        {
            IQueryRunner runner = new QueryRunner();

            runner.SetConnectionString(CONNECTION_STRING);
            object result = runner.RunQuery("SELECT COUNT(*) FROM category;");
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void RunQueryMaxResultTest()
        {
            IQueryRunner runner = new QueryRunner();
            runner.SetConnectionString(CONNECTION_STRING);
            object result = runner.RunQuery("SELECT MAX(ItemDescription) FROM Item");
            string asString = (string)result;
            Assert.AreEqual("With tail", asString.Trim());
        }

        [TestMethod]
        public void RunQueryMinResultTest()
        {
            IQueryRunner runner = new QueryRunner();
            runner.SetConnectionString(CONNECTION_STRING);
            string result = (string)runner.RunQuery("SELECT Min(UserCountry) FROM ACCOUNT WHERE FavCategory = 'Cats'");
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("Austria", result);
        }

        [TestMethod]
        public void RunQueryAvgResultTest()
        {
            IQueryRunner runner = new QueryRunner();
            runner.SetConnectionString(CONNECTION_STRING);
            object result = runner.RunQuery("SELECT Avg(ItemListPrice) FROM ITEM");
            Assert.IsInstanceOfType(result, typeof(decimal));
            decimal asDecimal = (decimal)result;
            Assert.AreEqual(30.1192m, asDecimal);
        }

        [TestMethod]
        public void RunQueryDateResultTest()
        {
            DateTime expected = new DateTime(1998, 5, 6);
            IQueryRunner runner = new QueryRunner();
            runner.SetConnectionString(CONNECTION_STRING);
            object result = runner.RunQuery("SELECT MAX(OrderDate) FROM ORDERS");
            Assert.IsInstanceOfType(result, typeof(DateTime));
            DateTime asDate = (DateTime)result;
            Assert.AreEqual(expected, asDate);
        }

        [TestMethod]
        public void RunQueryBooleanResultTest()
        {
            IQueryRunner runner = new QueryRunner();
            runner.SetConnectionString(CONNECTION_STRING);
            object result = runner.RunQuery("SELECT CASE WHEN EXISTS (SELECT 1 FROM ORDERS WHERE OrderId = 18) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END");
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void RunQueryTableNotExistTest()
        {
            IQueryRunner runner = new QueryRunner();
            runner.SetConnectionString(CONNECTION_STRING);
            var result = runner.RunQuery("SELECT COUNT(*) FROM NotExist;");
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void RunQueryColumnNotExistTest()
        {
            IQueryRunner runner = new QueryRunner();
            runner.SetConnectionString(CONNECTION_STRING);
            var result = runner.RunQuery("SELECT NotExist FROM Item;");
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void RunQueryInvalidConnStrTest_1()
        {
            IQueryRunner runner = new QueryRunner();
            runner.SetConnectionString("Server=.\\SQLSERV;Database=prueba;Trusted_Connection=True;MultipleActiveResultSets=True;");
            int ret = (int)runner.RunQuery("SELECT COUNT(*) FROM category;");
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void RunQueryInvalidConnStrTest_2()
        {
            IQueryRunner runner = new QueryRunner();
            runner.SetConnectionString("Wrong");
            int ret = (int)runner.RunQuery("SELECT COUNT(*) FROM category;");
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void RunQueryConnStrNullTest()
        {
            IQueryRunner runner = new QueryRunner();
            runner.SetConnectionString(null);
            int ret = (int)runner.RunQuery("SELECT COUNT(*) FROM category;");
        }
    }
    
}