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
        private IVisitorComponent<EvaluateConditionResult> visitor;

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
            ItemBoolean numeric = new ItemBoolean { Boolean = true };
            EvaluateConditionResult result = numeric.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("True", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateItemDateOkTest()
        {
            DateTime testDate = new DateTime(2015, 3, 15);
            ItemDate numeric = new ItemDate { Date = testDate };
            EvaluateConditionResult result = numeric.Accept(visitor);
            Assert.AreEqual(testDate, (DateTime)result.ConditionResult);
            Assert.AreEqual(testDate.ToString(), result.ConditionToString);
        }
        
        [TestMethod]
        public void EvaluateItemNumericOkTest()
        {
            ItemNumeric numeric = new ItemNumeric { NumberValue = 232};
            EvaluateConditionResult result = numeric.Accept(visitor);
            Assert.AreEqual(232, result.ConditionResult);
            Assert.AreEqual("232", result.ConditionToString);
        }
        
        [TestMethod]
        public void EvaluateItemTextOkTest()
        {
            ItemText text = new ItemText { TextValue = "Test String" };
            EvaluateConditionResult result = text.Accept(visitor);
            Assert.AreEqual("Test String", result.ConditionResult);
            Assert.AreEqual("Test String", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateItemTextNullTest()
        {
            ItemText text = new ItemText { TextValue = "" };
            EvaluateConditionResult result = text.Accept(visitor);
            Assert.AreEqual("", result.ConditionResult);
            Assert.AreEqual("", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateItemQueryTextResultOkTest()
        {
            string queryString = "SELECT Name FROM TABLE LIMIT 1";
            mock.Setup(m => m.RunQuery(queryString)).Returns("Test Name");
            ItemQuery query = new ItemQuery { QueryTextValue = queryString};
            EvaluateConditionResult result = query.Accept(visitor);
            Assert.AreEqual("Test Name", result.ConditionResult);
            Assert.AreEqual("Test Name", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateItemQueryNumberResultOkTest()
        {
            string queryString = "SELECT COUNT() FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { QueryTextValue = queryString};
            EvaluateConditionResult result = query.Accept(visitor);
            Assert.AreEqual(20, result.ConditionResult);
            Assert.AreEqual("20", result.ConditionToString);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void EvaluateItemQueryDataAccessExceptionTest()
        {
            string queryString = "SELECT * FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { QueryTextValue = queryString};
            EvaluateConditionResult result = query.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateMayorConditionCompareIntsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 19 };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{number, query }};

            EvaluateConditionResult result = mayor.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(20 > 19)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMayorConditionCompareIntsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(30);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 30 };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{number, query }};

            EvaluateConditionResult result = mayor.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(30 > 30)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMayorConditionCompareStringsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Jupiter");
            ItemQuery query = new ItemQuery { Position = 2, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 1, TextValue = "Venus" };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = mayor.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(Venus > Jupiter)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMayorConditionCompareStringsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Jupiter");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Jupiter" };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = mayor.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(Jupiter > Jupiter)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMayorStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(100);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = mayor.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(100 > Venus)", result.ConditionToString);
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
            
            EvaluateConditionResult result = mayor.Accept(visitor);
        }
        

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareIntsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(101);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 100 };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{number, query }};

            EvaluateConditionResult result = mayorEquals.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(101 >= 100)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareIntsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 20 };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{number, query }};

            EvaluateConditionResult result = mayorEquals.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(20 >= 20)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareIntsOkTest_3()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 300 };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{number, query }};

            EvaluateConditionResult result = mayorEquals.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(20 >= 300)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareStringsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Beach");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Bleach" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = mayorEquals.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(Beach >= Bleach)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareStringsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("SameWord");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "SameWord" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = mayorEquals.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(SameWord >= SameWord)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMayorEqualsConditionCompareStringsOkTest_3()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Last");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Alphabet" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = mayorEquals.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(Last >= Alphabet)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMayorEqualsStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Others" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = mayorEquals.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(20 >= Others)", result.ConditionToString);
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
            
            EvaluateConditionResult result = mayorEquals.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateMinorConditionCompareIntsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(50);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 60 };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{number, query }};

            EvaluateConditionResult result = minor.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(50 < 60)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMinorConditionCompareIntsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(300);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 300 };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{number, query }};

            EvaluateConditionResult result = minor.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(300 < 300)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMinorConditionCompareStringsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Beach");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Bleach" };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = minor.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(Beach < Bleach)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMinorConditionCompareStringsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("SameWord");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "SameWord" };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = minor.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(SameWord < SameWord)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMinorConditionCompareStringsOkTest_3()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Last");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Alphabet" };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = minor.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(Last < Alphabet)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMinorStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Others" };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = minor.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(20 < Others)", result.ConditionToString);
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
            
            EvaluateConditionResult result = minor.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareIntsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(30);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 45 };
            MinorEqualsCondition mayorEquals = new MinorEqualsCondition { Components = new List<Component>{number, query }};

            EvaluateConditionResult result = mayorEquals.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(30 <= 45)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareIntsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(30);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 30 };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{number, query }};

            EvaluateConditionResult result = minorEquals.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(30 <= 30)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareIntsOkTest_3()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(400);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 300 };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{number, query }};

            EvaluateConditionResult result = minorEquals.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(400 <= 300)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareStringsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Primary");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Secundary" };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = minorEquals.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(Primary <= Secundary)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareStringsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("SameWord");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "SameWord" };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = minorEquals.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(SameWord <= SameWord)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMinorEqualsConditionCompareStringsOkTest_3()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Last");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Alphabet" };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = minorEquals.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(Last <= Alphabet)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateMinorEqualsStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "20" };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = minorEquals.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(20 <= 20)", result.ConditionToString);
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
            
            EvaluateConditionResult result = minorEquals.Accept(visitor);
        }

        [TestMethod]
        public void EvaluateEqualsConditionCompareIntsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(45);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 45 };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{number, query }};

            EvaluateConditionResult result = equals.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(45 = 45)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateEqualsConditionCompareIntsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(30);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 31 };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{number, query }};

            EvaluateConditionResult result = equals.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(30 = 31)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateEqualsConditionCompareStringsOkTest_1()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Door");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Door" };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = equals.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(Door = Door)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateEqualsConditionCompareStringsOkTest_2()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Open");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Close" };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = equals.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(Open = Close)", result.ConditionToString);
        }

        [TestMethod]
        public void EvaluateEqualsStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "20" };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ query, text }};
            
            EvaluateConditionResult result = equals.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(20 = 20)", result.ConditionToString);
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
            
            EvaluateConditionResult result = equals.Accept(visitor);
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

            EvaluateConditionResult result = condition.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(String = String) And (56 > 45)", result.ConditionToString);
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

            EvaluateConditionResult result = condition.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(OtherString = String) And (56 > 45)", result.ConditionToString);
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
            
            EvaluateConditionResult result = condition.Accept(visitor);
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

            EvaluateConditionResult result = condition.Accept(visitor);
            Assert.IsTrue((bool)result.ConditionResult);
            Assert.AreEqual("(Test = Test) Or (56 <= 45)", result.ConditionToString);
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

            EvaluateConditionResult result = condition.Accept(visitor);
            Assert.IsFalse((bool)result.ConditionResult);
            Assert.AreEqual("(OtherString = Test) Or (56 <= 45)", result.ConditionToString);
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
            
            EvaluateConditionResult result = condtion.Accept(visitor);
        }
    }
}