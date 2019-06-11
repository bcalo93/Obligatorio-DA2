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
    public class VisitorComponentToStringTest
    {
        private Mock<IQueryRunner> mock;
        private IVisitorComponent<string> visitor;

        [TestInitialize]
        public void InitMock()
        {
            mock = new Mock<IQueryRunner>(MockBehavior.Strict);
            visitor = new VisitorComponentToString(mock.Object);
        }
        
        [TestCleanup]
        public void VerifyAll()
        {
            mock.VerifyAll();
        }

        [TestMethod]
        public void ToStringItemBooleanOkTest()
        {
            ItemBoolean boolean = new ItemBoolean { Boolean = true };
            
            string result = boolean.Accept(visitor);
            Assert.AreEqual("True", result);
        }

        [TestMethod]
        public void ToStringItemDateOkTest()
        {
            DateTime testDate = new DateTime(2015, 3, 15);
            ItemDate date = new ItemDate { Date = testDate };
            
            string result = date.Accept(visitor);
            Assert.AreEqual(testDate.ToString(), result);
        }
        

        [TestMethod]
        public void ToStringItemNumericOkTest()
        {
            ItemNumeric numeric = new ItemNumeric { NumberValue = 232};
            
            string result = numeric.Accept(visitor);
            Assert.AreEqual("232", result);
        }
        
        [TestMethod]
        public void ToStringItemTextOkTest()
        {
            ItemText text = new ItemText { TextValue = "Test String" };
            
            string result = text.Accept(visitor);
            Assert.AreEqual("Test String", result);
        }

        [TestMethod]
        public void ToStringItemTextNullTest()
        {
            ItemText text = new ItemText { TextValue = "" };
            
            string result = text.Accept(visitor);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void ToStringItemQueryTextResultOkTest()
        {
            string queryString = "SELECT Name FROM TABLE LIMIT 1";
            mock.Setup(m => m.RunQuery(queryString)).Returns("Test Name");
            ItemQuery query = new ItemQuery { QueryTextValue = queryString};
            
            string result = query.Accept(visitor);
            Assert.AreEqual("Test Name", result);
        }

        [TestMethod]
        public void ToStringItemQueryNumberResultOkTest()
        {
            string queryString = "SELECT COUNT() FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { QueryTextValue = queryString};
            
            string result = query.Accept(visitor);
            Assert.AreEqual("20", result);
        }

        [TestMethod]
        public void ToStringItemQueryDataAccessExceptionTest()
        {
            string queryString = "SELECT * FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { QueryTextValue = queryString};

            string result = query.Accept(visitor);
            Assert.AreEqual("Incorrect Query - SELECT * FROM TABLE", result);
        }

        [TestMethod]
        public void ToStringMayorConditionCompareIntsOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 19 };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{number, query }};

            string result = mayor.Accept(visitor);
            Assert.AreEqual("(20 > 19)", result);
        }

        [TestMethod]
        public void ToStringMayorConditionCompareStringsOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Jupiter");
            ItemQuery query = new ItemQuery { Position = 2, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 1, TextValue = "Venus" };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{ query, text }};
            
            string result = mayor.Accept(visitor);
            Assert.AreEqual("(Venus > Jupiter)", result);
        }

        [TestMethod]
        public void ToStringMayorStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(100);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{ query, text }};
            
            string result = mayor.Accept(visitor);
            Assert.AreEqual("(100 > Venus)", result);
        }

        [TestMethod]
        public void ToStringMayorBooleanStringOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("Holidays");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean date = new ItemBoolean { Position = 2, Boolean = false };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{date, query }};

            string result = mayor.Accept(visitor);
            Assert.AreEqual("(Holidays > False)", result);
        }

        [TestMethod]
        public void ToStringMayorDataAccessExceptionTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            MayorCondition mayor = new MayorCondition { Components = new List<Component>{ query, text }};
            
            string result = mayor.Accept(visitor);
            Assert.AreEqual("(Incorrect Query - SELECT COUNT(*) FROM TABLE > Venus)", result);
        }
        

        [TestMethod]
        public void ToStringMayorEqualsConditionCompareIntsOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(101);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 100 };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{number, query }};

            string result = mayorEquals.Accept(visitor);
            Assert.AreEqual("(101 >= 100)", result);
        }

        [TestMethod]
        public void ToStringMayorEqualsConditionCompareStringsOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Beach");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Bleach" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{ query, text }};
            
            string result = mayorEquals.Accept(visitor);
            Assert.AreEqual("(Beach >= Bleach)", result);
        }

        [TestMethod]
        public void ToStringMayorEqualsConditionCompareBooleanOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(true);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{boolean, query }};

            string result = mayorEquals.Accept(visitor);
            Assert.AreEqual("(True >= True)", result);
        }
        
        [TestMethod]
        public void ToStringMayorEqualsStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Others" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{ query, text }};
            
            string result = mayorEquals.Accept(visitor);
            Assert.AreEqual("(20 >= Others)", result);
        }

        [TestMethod]
        public void ToStringMayorEqualsBooleanStringOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("False");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean date = new ItemBoolean { Position = 2, Boolean = false };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{date, query }};

            string result = mayorEquals.Accept(visitor);
            Assert.AreEqual("(False >= False)", result);
        }

        [TestMethod]
        public void ToStringMayorEqualsDataAccessExceptionTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Components = new List<Component>{ query, text }};
            
            string result = mayorEquals.Accept(visitor);
            Assert.AreEqual("(Incorrect Query - SELECT COUNT(*) FROM TABLE >= Venus)", result);
        }

        [TestMethod]
        public void ToStringMinorConditionCompareIntsOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(50);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 60 };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{number, query }};

            string result = minor.Accept(visitor);
            Assert.AreEqual("(50 < 60)", result);
        }

        [TestMethod]
        public void ToStringMinorConditionCompareStringsOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Beach");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Bleach" };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{ query, text }};
            
            string result = minor.Accept(visitor);
            Assert.AreEqual("(Beach < Bleach)", result);
        }

        [TestMethod]
        public void ToStringMinorConditionCompareBooleanOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(true);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{boolean, query }};

            string result = minor.Accept(visitor);
            Assert.AreEqual("(True < True)", result);
        }

        [TestMethod]
        public void ToStringMinorStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Others" };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{ query, text }};
            
            string result = minor.Accept(visitor);
            Assert.AreEqual("(20 < Others)", result);
        }

        [TestMethod]
        public void ToStringMinorBooleanStringOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("False");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean date = new ItemBoolean { Position = 2, Boolean = false };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{date, query }};

            string result = minor.Accept(visitor);
            Assert.AreEqual("(False < False)", result);
        }

        [TestMethod]
        public void ToStringMinorDataAccessExceptionTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            MinorCondition minor = new MinorCondition { Components = new List<Component>{ query, text }};
            
            string result = minor.Accept(visitor);
            Assert.AreEqual("(Incorrect Query - SELECT COUNT(*) FROM TABLE < Venus)", result);
        }

        [TestMethod]
        public void ToStringMinorEqualsConditionCompareIntsOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(30);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 45 };
            MinorEqualsCondition mayorEquals = new MinorEqualsCondition { Components = new List<Component>{number, query }};

            string result = mayorEquals.Accept(visitor);
            Assert.AreEqual("(30 <= 45)", result);
        }

        [TestMethod]
        public void ToStringMinorEqualsConditionCompareStringsOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Primary");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Secundary" };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{ query, text }};
            
            string result = minorEquals.Accept(visitor);
            Assert.AreEqual("(Primary <= Secundary)", result);
        }

        [TestMethod]
        public void ToStringMinorEqualsConditionCompareBooleanOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(false);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{boolean, query }};

            string result = minorEquals.Accept(visitor);
            Assert.AreEqual("(False <= True)", result);
        }

        [TestMethod]
        public void ToStringMinorEqualsStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "20" };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{ query, text }};
            
            string result = minorEquals.Accept(visitor);
            Assert.AreEqual("(20 <= 20)", result);
        }

        [TestMethod]
        public void ToStringMinorEqualsBooleanStringOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("True");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean date = new ItemBoolean { Position = 2, Boolean = true };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{date, query }};

            string result = minorEquals.Accept(visitor);
            Assert.AreEqual("(True <= True)", result);
        }

        [TestMethod]
        public void ToStringMinorEqualsDataAccessExceptionTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            MinorEqualsCondition minorEquals = new MinorEqualsCondition { Components = new List<Component>{ query, text }};
            
            string result = minorEquals.Accept(visitor);
            Assert.AreEqual("(Incorrect Query - SELECT COUNT(*) FROM TABLE <= Venus)", result);
        }

        [TestMethod]
        public void ToStringEqualsConditionCompareIntsOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(45);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 45 };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{number, query }};

            string result = equals.Accept(visitor);
            Assert.AreEqual("(45 = 45)", result);
        }

        [TestMethod]
        public void ToStringEqualsConditionCompareStringsOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns("Door");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Door" };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ query, text }};
            
            string result = equals.Accept(visitor);
            Assert.AreEqual("(Door = Door)", result);
        }

        [TestMethod]
        public void ToStringEqualsConditionCompareBooleanOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns(false);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = false };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ boolean, query }};

            string result = equals.Accept(visitor);
            Assert.AreEqual("(False = False)", result);
        }

        [TestMethod]
        public void ToStringEqualsStringIntTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "20" };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ query, text }};
            
            string result = equals.Accept(visitor);
            Assert.AreEqual("(20 = 20)", result);
        }

        [TestMethod]
        public void ToStringEqualsBooleanStringOkTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Returns("True");
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemBoolean date = new ItemBoolean { Position = 2, Boolean = true };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{date, query }};

            string result = equals.Accept(visitor);
            Assert.AreEqual("(True = True)", result);
        }

        [TestMethod]
        public void ToStringEqualsDataAccessExceptionTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";
            mock.Setup(m => m.RunQuery(queryString)).Throws(new DataAccessException(""));
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "Venus" };
            EqualsCondition equals = new EqualsCondition { Components = new List<Component>{ query, text }};
            
            string result = equals.Accept(visitor);
            Assert.AreEqual("(Incorrect Query - SELECT COUNT(*) FROM TABLE = Venus)", result);
        }

        [TestMethod]
        public void ToStringAndConditionOkTest()
        {
            string queryString1 = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString1)).Returns("String");
            ItemQuery query1 = new ItemQuery { Position = 1, QueryTextValue = queryString1};
            ItemText text = new ItemText { Position = 2, TextValue = "String" };
            EqualsCondition equals = new EqualsCondition { Position = 1, Components = new List<Component>{ query1, text }};

            ItemNumeric number1 = new ItemNumeric { Position = 1, NumberValue = 56};
            ItemNumeric number2 = new ItemNumeric { Position = 2, NumberValue = 45 };
            MayorCondition mayor = new MayorCondition { Position = 2, Components = new List<Component>{number1, number2 }};

            AndCondition condition = new AndCondition { Components = new List<Component> {equals, mayor }};

            string result = condition.Accept(visitor);
            Assert.AreEqual("(String = String) And (56 > 45)", result);
        }

        [TestMethod]
        public void ToStringAndWrongTypeTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "20" };
            AndCondition condition = new AndCondition { Components = new List<Component>{ query, text }};
            
            string result = condition.Accept(visitor);
            Assert.AreEqual("20 And 20", result);
        }

        [TestMethod]
        public void ToStringOrConditionOkTest()
        {
            string queryString1 = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString1)).Returns("Test");
            ItemQuery query1 = new ItemQuery { Position = 1, QueryTextValue = queryString1};
            ItemText text = new ItemText { Position = 2, TextValue = "Test" };
            EqualsCondition equals = new EqualsCondition { Position = 1, Components = new List<Component>{ query1, text }};

            ItemNumeric number1 = new ItemNumeric { Position = 1, NumberValue = 56};
            ItemNumeric number2 = new ItemNumeric { Position = 2, NumberValue = 45 };
            MinorEqualsCondition minor = new MinorEqualsCondition { Position = 2, Components = new List<Component>{number1, number2 }};

            OrCondition condition = new OrCondition { Components = new List<Component> {equals, minor }};

            string result = condition.Accept(visitor);
            Assert.AreEqual("(Test = Test) Or (56 <= 45)", result);
        }

        [TestMethod]
        public void ToStringOrWrongTypeTest()
        {
            string queryString = "SELECT COUNT(*) FROM TABLE";

            mock.Setup(m => m.RunQuery(queryString)).Returns(20);
            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemText text = new ItemText { Position = 2, TextValue = "20" };
            OrCondition condtion = new OrCondition { Components = new List<Component>{ query, text }};
            
            string result = condtion.Accept(visitor);
            Assert.AreEqual("20 Or 20", result);
        }
    }
    
}