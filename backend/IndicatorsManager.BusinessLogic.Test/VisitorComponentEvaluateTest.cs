using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using IndicatorsManager.Domain;
using IndicatorsManager.Domain.Visitors;
using IndicatorsManager.BusinessLogic.Visitors;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.BusinessLogic.Test
{
    [TestClass]
    public class VisitorComponentEvaluateTest
    {
        private Mock<IQueryRunner> mock;
        private IVisitorComponent<DataType> visitor;

        [TestInitialize]
        public void InitMock()
        {
            mock = new Mock<IQueryRunner>(MockBehavior.Strict);
            visitor = new VisitorComponentEvaluate(mock.Object);
        }
        
        [TestCleanup]
        public void VerifyAll()
        {
            mock.VerifyAll();
        }

        [TestMethod]
        public void EvaluateItemBooleanOkTest()
        {
            ItemBoolean boolean = new ItemBoolean { Boolean = true };
            DataType result = boolean.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateItemDateOkTest()
        {
            DateTime testDate = new DateTime(2015, 3, 15);
            ItemDate date = new ItemDate { Date = testDate };
            DataType result = date.Accept(visitor);
            DateDataType asDate = result as DateDataType;
            Assert.AreEqual(testDate, asDate.DateValue);
        }
        
        [TestMethod]
        public void EvaluateItemNumericOkTest()
        {
            ItemNumeric numeric = new ItemNumeric { NumberValue = 232};
            DataType result = numeric.Accept(visitor);
            DecimalDataType asDecimal = result as DecimalDataType;
            Assert.AreEqual(232m, asDecimal.DecimalValue);
        }
        
        [TestMethod]
        public void EvaluateItemTextOkTest()
        {
            ItemText text = new ItemText { TextValue = "Test String" };
            DataType result = text.Accept(visitor);
            StringDataType asString = result as StringDataType;
            Assert.AreEqual("Test String", asString.StringValue);
        }

        [TestMethod]
        public void EvaluateItemTextNullTest()
        {
            ItemText text = new ItemText { TextValue = "" };
            DataType result = text.Accept(visitor);
            StringDataType asString = result as StringDataType;
            Assert.AreEqual("", asString.StringValue);
        }

        [TestMethod]
        public void EvaluateItemQueryTextResultOkTest()
        {
            string queryString = "SELECT Name FROM TABLE LIMIT 1";
            mock.Setup(m => m.RunQuery(queryString)).Returns("Test Name");
            ItemQuery query = new ItemQuery { QueryTextValue = queryString};
            DataType result = query.Accept(visitor);
            StringDataType asString = result as StringDataType;
            Assert.AreEqual("Test Name", asString.StringValue);
        }

        [TestMethod]
        public void EvaluateItemQueryNumberResultOkTest()
        {
            string queryString = "SELECT COUNT() FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { QueryTextValue = queryString};
            DataType result = query.Accept(visitor);
            DecimalDataType asDecimal = result as DecimalDataType;
            Assert.AreEqual(20m, asDecimal.DecimalValue);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateItemQueryDataAccessExceptionTest()
        {
            string queryString = "SELECT * FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { QueryTextValue = queryString};
            DataType result = query.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateMayorConditionCompareIntsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 19 };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{number, query }};

            DataType result = mayor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorConditionCompareIntsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(30);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 30 };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{number, query }};

            DataType result = mayor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorConditionCompareStringsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Jupiter");
            ItemQuery query = new ItemQuery { Position = 2, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 1, TextValue = "Venus" };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{ query, text }};
            
            DataType result = mayor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorConditionCompareStringsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Jupiter");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Jupiter" };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{ query, text }};
            
            DataType result = mayor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorConditionCompareDatesOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(new DateTime(2018, 10, 5));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2018, 10, 4) };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{date, query }};

            DataType result = mayor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorConditionCompareDatesOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(new DateTime(2018, 10, 1));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2018, 10, 4) };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{date, query }};

            DataType result = mayor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateMayorConditionCompareBooleanOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(true);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = false };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{boolean, query }};

            DataType result = mayor.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateMayorStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(100);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{ query, text }};
            
            DataType result = mayor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorStringDatesOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("Test String");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2018, 10, 4) };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{date, query }};

            DataType result = mayor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorBooleanStringOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("Holidays");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean date = new ItemBoolean { Position = 2, Boolean = false };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{date, query }};

            DataType result = mayor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateMayorDataAccessExceptionTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{ query, text }};
            
            DataType result = mayor.Accept(visitor);
        }
        

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareIntsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(101);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 100 };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{number, query }};

            DataType result = mayorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareIntsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 20 };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{number, query }};

            DataType result = mayorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareIntsOkTest_3()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 300 };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{number, query }};

            DataType result = mayorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareStringsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Beach");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Bleach" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = mayorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareStringsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("SameWord");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "SameWord" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = mayorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareStringsOkTest_3()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Last");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Alphabet" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = mayorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareDatesOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(new DateTime(2018, 10, 4));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2018, 10, 4) };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{date, query }};

            DataType result = mayorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareDatesOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(new DateTime(2018, 9, 1));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2018, 9, 4) };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{date, query }};

            DataType result = mayorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateMayorEqualsConditionCompareBooleanOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(true);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{boolean, query }};

            DataType result = mayorEquals.Accept(visitor);
        }
        
        [TestMethod]
        public void EvaluateMayorEqualsStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Others" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = mayorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorEqualsStringDatesOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("Test String");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2018, 10, 4) };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{date, query }};

            DataType result = mayorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMayorEqualsBooleanStringOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("False");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean date = new ItemBoolean { Position = 2, Boolean = false };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{date, query }};

            DataType result = mayorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateMayorEqualsDataAccessExceptionTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = mayorEquals.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateMinorConditionCompareIntsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(50);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 60 };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{number, query }};

            DataType result = minor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorConditionCompareIntsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(300);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 300 };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{number, query }};

            DataType result = minor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorConditionCompareStringsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Beach");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Bleach" };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{ query, text }};
            
            DataType result = minor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorConditionCompareStringsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("SameWord");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "SameWord" };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{ query, text }};
            
            DataType result = minor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorConditionCompareStringsOkTest_3()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Last");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Alphabet" };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{ query, text }};
            
            DataType result = minor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorConditionCompareDatesOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(new DateTime(2019, 3, 4));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2019, 3, 7) };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{date, query }};

            DataType result = minor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorConditionCompareDatesOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(new DateTime(2019, 2, 3));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2019, 2, 3) };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{date, query }};

            DataType result = minor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateMinorConditionCompareBooleanOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(true);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{boolean, query }};

            DataType result = minor.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateMinorStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Others" };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{ query, text }};
            
            DataType result = minor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorStringDatesOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("Test String");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2018, 10, 4) };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{date, query }};

            DataType result = minor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorBooleanStringOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("False");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean date = new ItemBoolean { Position = 2, Boolean = false };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{date, query }};

            DataType result = minor.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateMinorDataAccessExceptionTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{ query, text }};
            
            DataType result = minor.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareIntsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(30);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 45 };
            MinorEqualsCondition mayorEquals = new MinorEqualsCondition { Components = new List<Component>{number, query }};

            DataType result = mayorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareIntsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(30);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 30 };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{number, query }};

            DataType result = minorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareIntsOkTest_3()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(400);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 300 };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{number, query }};

            DataType result = minorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
            //Assert.AreEqual("(400 <= 300)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareStringsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Primary");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Secundary" };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = minorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareStringsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("SameWord");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "SameWord" };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = minorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareStringsOkTest_3()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Last");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Alphabet" };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = minorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareDatesOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(new DateTime(2019, 3, 7));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2019, 3, 7) };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{date, query }};

            DataType result = minorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareDatesOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(new DateTime(2019, 2, 4));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2019, 2, 3) };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{date, query }};

            DataType result = minorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateMinorEqualsConditionCompareBooleanOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(false);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{boolean, query }};

            DataType result = minorEquals.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateMinorEqualsStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "20" };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = minorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorEqualsStringDatesOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("Test String");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2018, 10, 4) };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{date, query }};

            DataType result = minorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateMinorEqualsBooleanStringOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("True");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean date = new ItemBoolean { Position = 2, Boolean = true };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{date, query }};

            DataType result = minorEquals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateMinorEqualsDataAccessExceptionTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = minorEquals.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateEqualsConditionCompareIntsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(45);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 45 };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{number, query }};

            DataType result = equals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateEqualsConditionCompareIntsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(30);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 31 };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{number, query }};

            DataType result = equals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateEqualsConditionCompareStringsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Door");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Door" };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = equals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateEqualsConditionCompareStringsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Open");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Close" };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = equals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateEqualsConditionCompareDatesOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(new DateTime(2019, 5, 7));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2019, 5, 7) };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ date, query }};

            DataType result = equals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateEqualsConditionCompareDatesOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(new DateTime(2019, 2, 4));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2019, 2, 3) };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ date, query }};

            DataType result = equals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateEqualsConditionCompareBooleanOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(false);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = false };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ boolean, query }};

            DataType result = equals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateEqualsConditionCompareBooleanOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(true);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = false };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ boolean, query }};

            DataType result = equals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateEqualsStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "20" };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = equals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateEqualsStringDatesOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("Test String");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemDate date = new ItemDate { Position = 2, Date = new DateTime(2018, 10, 4) };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{date, query }};

            DataType result = equals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateEqualsBooleanStringOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("True");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean date = new ItemBoolean { Position = 2, Boolean = true };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{date, query }};

            DataType result = equals.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateEqualsDataAccessExceptionTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ query, text }};
            
            DataType result = equals.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateAndConditionOkTest_1()
        {
            string queryString1 = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString1)).Returns("String");
            ItemQuery query1 = new ItemQuery { Position = 1, QueryTextValue = queryString1};
            ItemText text = new ItemText { Position = 2, TextValue = "String" };
            // Evaluates to true
            EqualsCondition equals = new EqualsCondition { Position = 1, Components = new List<Component>{ query1, text }};

            ItemNumeric number1 = new ItemNumeric { Position = 1, NumberValue = 56};
            ItemNumeric number2 = new ItemNumeric { Position = 2, NumberValue = 45 };
            // Evaluates to true
            MayorCondition mayor = new MayorCondition { Position = 2, Components = new List<Component>{number1, number2 }};

            AndCondition condition = new AndCondition { Components = new List<Component> {equals, mayor }};

            DataType result = condition.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateAndConditionOkTest_2()
        {
            string queryString1 = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString1)).Returns("OtherString");
            ItemQuery query1 = new ItemQuery { Position = 1, QueryTextValue = queryString1};
            ItemText text = new ItemText { Position = 2, TextValue = "String" };
            // Evaluates to true
            EqualsCondition equals = new EqualsCondition { Position = 1, Components = new List<Component>{ query1, text }};

            ItemNumeric number1 = new ItemNumeric { Position = 1, NumberValue = 56};
            ItemNumeric number2 = new ItemNumeric { Position = 2, NumberValue = 45 };
            // Evaluates to true
            MayorCondition mayor = new MayorCondition { Position = 2, Components = new List<Component>{number1, number2 }};

            AndCondition condition = new AndCondition { Components = new List<Component> {equals, mayor }};

            DataType result = condition.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateAndWrongTypeTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "20" };
            AndCondition condition = new AndCondition { Components = new List<Component>{ query, text }};
            
            DataType result = condition.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateOrConditionOkTest_1()
        {
            string queryString1 = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString1)).Returns("Test");
            ItemQuery query1 = new ItemQuery { Position = 1, QueryTextValue = queryString1};
            ItemText text = new ItemText { Position = 2, TextValue = "Test" };
            // Evaluates to true
            EqualsCondition equals = new EqualsCondition { Position = 1, Components = new List<Component>{ query1, text }};

            ItemNumeric number1 = new ItemNumeric { Position = 1, NumberValue = 56};
            ItemNumeric number2 = new ItemNumeric { Position = 2, NumberValue = 45 };
            // Evaluates to false
            MinorEqualsCondition minor = new MinorEqualsCondition { Position = 2, Components = new List<Component>{number1, number2 }};

            OrCondition condition = new OrCondition { Components = new List<Component> {equals, minor }};

            DataType result = condition.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsTrue(asBoolean.BooleanValue);
        }

        [TestMethod]
        public void EvaluateOrConditionOkTest_2()
        {
            string queryString1 = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString1)).Returns("OtherString");
            ItemQuery query1 = new ItemQuery { Position = 1, QueryTextValue = queryString1};
            ItemText text = new ItemText { Position = 2, TextValue = "Test" };
            // Evaluates to true
            EqualsCondition equals = new EqualsCondition { Position = 1, Components = new List<Component>{ query1, text }};

            ItemNumeric number1 = new ItemNumeric { Position = 1, NumberValue = 56};
            ItemNumeric number2 = new ItemNumeric { Position = 2, NumberValue = 45 };
            // Evaluates to false
            MinorEqualsCondition minor = new MinorEqualsCondition { Position = 2, Components = new List<Component>{number1, number2 }};

            OrCondition condition = new OrCondition { Components = new List<Component> {equals, minor }};

            DataType result = condition.Accept(visitor);
            BooleanDataType asBoolean = result as BooleanDataType;
            Assert.IsFalse(asBoolean.BooleanValue);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateOrWrongTypeTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "20" };
            OrCondition condtion = new OrCondition { Components = new List<Component>{ query, text }};
            
            DataType result = condtion.Accept(visitor);
        }
    }
}