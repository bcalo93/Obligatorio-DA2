using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Visitors;

namespace IndicatorsManager.DataAccess.Test
{
    [TestClass]
    public class VisitorComponentToListTest
    {
        [TestMethod]
        public void ItemNumericToListTest()
        {
            ItemNumeric numeric = new ItemNumeric { NumberValue = 20, Position = 2 };
            List<Component> result = numeric.Accept(new VisitorComponentToList());
            Assert.AreEqual(1, result.Count);

            ItemNumeric numericResult = result.ElementAt(0) as ItemNumeric;
            Assert.AreEqual(20, numericResult.NumberValue);
            Assert.AreEqual(2, numericResult.Position);
        }

        [TestMethod]
        public void ItemTextToListTest()
        {
            ItemText text = new ItemText { Position = 1, TextValue = "Test Value" };
            List<Component> result = text.Accept(new VisitorComponentToList());
            Assert.AreEqual(1, result.Count);

            ItemText textResult = result.ElementAt(0) as ItemText;
            Assert.AreEqual("Test Value", textResult.TextValue);
            Assert.AreEqual(1, textResult.Position);
        }

        [TestMethod]
        public void ItemQueryToListTest()
        {
            ItemQuery text = new ItemQuery { Position = 2, QueryTextValue = "Select Count(*) From Table" };
            List<Component> result = text.Accept(new VisitorComponentToList());
            Assert.AreEqual(1, result.Count);

            ItemQuery queryResult = result.ElementAt(0) as ItemQuery;
            Assert.AreEqual("Select Count(*) From Table", queryResult.QueryTextValue);
            Assert.AreEqual(2, queryResult.Position);
        }

        [TestMethod]
        public void ItemBooleanToListTest()
        {
            ItemBoolean boolean = new ItemBoolean { Position = 3, Boolean = false };
            List<Component> result = boolean.Accept(new VisitorComponentToList());
            Assert.AreEqual(1, result.Count);

            ItemBoolean booleanResult = result.ElementAt(0) as ItemBoolean;
            Assert.IsFalse(booleanResult.Boolean);
            Assert.AreEqual(3, booleanResult.Position);
        }

        [TestMethod]
        public void ItemDateToListTest()
        {
            DateTime testDate = new DateTime(2019, 2, 4);
            
            ItemDate date = new ItemDate { Position = 2, Date = testDate };
            List<Component> result = date.Accept(new VisitorComponentToList());
            Assert.AreEqual(1, result.Count);

            ItemDate dateResult = result.ElementAt(0) as ItemDate;
            Assert.AreEqual(testDate, date.Date);
            Assert.AreEqual(2, dateResult.Position);
        }

        [TestMethod]
        public void EqualsCoditionToListTest()
        {
            DateTime testDate = new DateTime(2019, 2, 4);
            
            ItemDate date = new ItemDate { Id = Guid.NewGuid(), Position = 2, Date = testDate };
            ItemQuery query = new ItemQuery { Id = Guid.NewGuid(), Position = 1, QueryTextValue = "SELECT MAX(Date) FROM TABLE" };
            EqualsCondition condition = new EqualsCondition { Id = Guid.NewGuid(), Position = 1, Components = new List<Component> { date, query } };

            List<Component> result = condition.Accept(new VisitorComponentToList());
            Assert.AreEqual(3, result.Count);

            EqualsCondition equalsResult = result.Single(c => c.Id == condition.Id) as EqualsCondition;
            Assert.AreEqual(2, equalsResult.Components.Count);
            Assert.AreEqual(1, equalsResult.Position);
            
            ItemDate dateResult = result.Single(c => c.Id == date.Id) as ItemDate;
            Assert.AreEqual(testDate, date.Date);
            Assert.AreEqual(2, dateResult.Position);

            ItemQuery queryResult = result.Single(c => c.Id == query.Id) as ItemQuery;
            Assert.AreEqual("SELECT MAX(Date) FROM TABLE", queryResult.QueryTextValue);
            Assert.AreEqual(1, queryResult.Position);
        }

        [TestMethod]
        public void MinorCoditionToListTest()
        {   
            ItemBoolean boolean = new ItemBoolean { Id = Guid.NewGuid(), Position = 2, Boolean = true };
            ItemQuery query = new ItemQuery { Id = Guid.NewGuid(), Position = 1, QueryTextValue = "SELECT COUNT(*) FROM TABLE" };
            MinorCondition condition = new MinorCondition { Id = Guid.NewGuid(), Position = 1, Components = new List<Component> { boolean, query } };

            List<Component> result = condition.Accept(new VisitorComponentToList());
            Assert.AreEqual(3, result.Count);

            MinorCondition minorResult = result.Single(c => c.Id == condition.Id) as MinorCondition;
            Assert.AreEqual(2, minorResult.Components.Count);
            Assert.AreEqual(1, minorResult.Position);
            
            ItemBoolean booleanResult = result.Single(c => c.Id == boolean.Id) as ItemBoolean;
            Assert.IsTrue(booleanResult.Boolean);
            Assert.AreEqual(2, booleanResult.Position);

            ItemQuery queryResult = result.Single(c => c.Id == query.Id) as ItemQuery;
            Assert.AreEqual("SELECT COUNT(*) FROM TABLE", queryResult.QueryTextValue);
            Assert.AreEqual(1, queryResult.Position);
        }


        [TestMethod]
        public void MinorEqualsCoditionToListTest()
        {   
            ItemText text = new ItemText { Id = Guid.NewGuid(), Position = 2, TextValue = "Test Text" };
            ItemQuery query = new ItemQuery { Id = Guid.NewGuid(), Position = 1, QueryTextValue = "SELECT COUNT(*) FROM TABLE" };
            MinorEqualsCondition condition = new MinorEqualsCondition { Id = Guid.NewGuid(), Position = 1, Components = new List<Component> { text, query } };

            List<Component> result = condition.Accept(new VisitorComponentToList());
            Assert.AreEqual(3, result.Count);

            MinorEqualsCondition minorEqualsResult = result.Single(c => c.Id == condition.Id) as MinorEqualsCondition;
            Assert.AreEqual(2, minorEqualsResult.Components.Count);
            Assert.AreEqual(1, minorEqualsResult.Position);
            
            ItemText textResult = result.Single(c => c.Id == text.Id) as ItemText;
            Assert.AreEqual("Test Text", textResult.TextValue);
            Assert.AreEqual(2, textResult.Position);

            ItemQuery queryResult = result.Single(c => c.Id == query.Id) as ItemQuery;
            Assert.AreEqual("SELECT COUNT(*) FROM TABLE", queryResult.QueryTextValue);
            Assert.AreEqual(1, queryResult.Position);
        }

        [TestMethod]
        public void MayorCoditionToListTest()
        {   
            ItemNumeric numeric = new ItemNumeric { Id = Guid.NewGuid(), Position = 2, NumberValue = 20 };
            ItemQuery query = new ItemQuery { Id = Guid.NewGuid(), Position = 1, QueryTextValue = "SELECT COUNT(*) FROM TABLE" };
            MayorCondition condition = new MayorCondition { Id = Guid.NewGuid(), Position = 1, Components = new List<Component> { numeric, query } };

            List<Component> result = condition.Accept(new VisitorComponentToList());
            Assert.AreEqual(3, result.Count);

            MayorCondition mayorResult = result.Single(c => c.Id == condition.Id) as MayorCondition;
            Assert.AreEqual(2, mayorResult.Components.Count);
            Assert.AreEqual(1, mayorResult.Position);
            
            ItemNumeric numericResult = result.Single(c => c.Id == numeric.Id) as ItemNumeric;
            Assert.AreEqual(20, numericResult.NumberValue);
            Assert.AreEqual(2, numericResult.Position);

            ItemQuery queryResult = result.Single(c => c.Id == query.Id) as ItemQuery;
            Assert.AreEqual("SELECT COUNT(*) FROM TABLE", queryResult.QueryTextValue);
            Assert.AreEqual(1, queryResult.Position);
        }


        [TestMethod]
        public void MayorEqualsCoditionToListTest()
        {   
            ItemText text = new ItemText { Id = Guid.NewGuid(), Position = 1, TextValue = "Test Text Mayor Equals" };
            ItemQuery query = new ItemQuery { Id = Guid.NewGuid(), Position = 2, QueryTextValue = "SELECT COUNT(*) FROM TABLE" };
            MayorEqualsCondition condition = new MayorEqualsCondition { Id = Guid.NewGuid(), Position = 1, Components = new List<Component> { text, query } };

            List<Component> result = condition.Accept(new VisitorComponentToList());
            Assert.AreEqual(3, result.Count);

            MayorEqualsCondition mayorEqualsResult = result.Single(c => c.Id == condition.Id) as MayorEqualsCondition;
            Assert.AreEqual(2, mayorEqualsResult.Components.Count);
            Assert.AreEqual(1, mayorEqualsResult.Position);
            
            ItemText textResult = result.Single(c => c.Id == text.Id) as ItemText;
            Assert.AreEqual("Test Text Mayor Equals", textResult.TextValue);
            Assert.AreEqual(1, textResult.Position);

            ItemQuery queryResult = result.Single(c => c.Id == query.Id) as ItemQuery;
            Assert.AreEqual("SELECT COUNT(*) FROM TABLE", queryResult.QueryTextValue);
            Assert.AreEqual(2, queryResult.Position);
        }

        [TestMethod]
        public void AndCoditionToListTest()
        {   
            ItemText text = new ItemText { Id = Guid.NewGuid(), Position = 1, TextValue = "Test Text And Condition" };
            ItemQuery query1 = new ItemQuery { Id = Guid.NewGuid(), Position = 2, QueryTextValue = "SELECT COUNT(*) FROM TABLE" };
            EqualsCondition condition1 = new EqualsCondition { Id = Guid.NewGuid(), Position = 1, Components = new List<Component> { text, query1 } };

            ItemBoolean boolean = new ItemBoolean { Id = Guid.NewGuid(), Position = 1, Boolean = false };
            ItemQuery query2 = new ItemQuery { Id = Guid.NewGuid(), Position = 2, QueryTextValue = "SELECT Column FROM TABLE" };
            MayorCondition condition2 = new MayorCondition { Id = Guid.NewGuid(), Position = 2, Components = new List<Component> { boolean, query2 } };

            AndCondition andCondition = new AndCondition { Id = Guid.NewGuid(), Position = 1, Components = new List<Component> { condition1, condition2 } };

            List<Component> result = andCondition.Accept(new VisitorComponentToList());
            Assert.AreEqual(7, result.Count);

            AndCondition andResult = result.Single(c => c.Id == andCondition.Id) as AndCondition;
            Assert.AreEqual(2, andResult.Components.Count);
            Assert.AreEqual(1, andResult.Position);

            EqualsCondition equalsResult = result.Single(c => c.Id == condition1.Id) as EqualsCondition;
            Assert.AreEqual(2, equalsResult.Components.Count);
            Assert.AreEqual(1, equalsResult.Position);

            ItemText textResult = result.Single(c => c.Id == text.Id) as ItemText;
            Assert.AreEqual("Test Text And Condition", textResult.TextValue);
            Assert.AreEqual(1, textResult.Position);

            ItemQuery query1Result = result.Single(c => c.Id == query1.Id) as ItemQuery;
            Assert.AreEqual("SELECT COUNT(*) FROM TABLE", query1Result.QueryTextValue);
            Assert.AreEqual(2, query1Result.Position);

            MayorCondition mayorResult = result.Single(c => c.Id == condition2.Id) as MayorCondition;
            Assert.AreEqual(2, mayorResult.Components.Count);
            Assert.AreEqual(2, mayorResult.Position);

            ItemBoolean booleanResult = result.Single(c => c.Id == text.Id) as ItemBoolean;
            Assert.IsFalse(boolean.Boolean);
            Assert.AreEqual(1, textResult.Position);

            ItemQuery query2Result = result.Single(c => c.Id == query2.Id) as ItemQuery;
            Assert.AreEqual("SELECT Column FROM TABLE", query2Result.QueryTextValue);
            Assert.AreEqual(2, query2Result.Position);
        }

        [TestMethod]
        public void OrCoditionToListTest()
        {   
            ItemText text = new ItemText { Id = Guid.NewGuid(), Position = 1, TextValue = "Test Text Or Condition" };
            ItemQuery query1 = new ItemQuery { Id = Guid.NewGuid(), Position = 2, QueryTextValue = "SELECT COUNT(*) FROM TABLE" };
            EqualsCondition condition1 = new EqualsCondition { Id = Guid.NewGuid(), Position = 1, Components = new List<Component> { text, query1 } };

            ItemBoolean boolean = new ItemBoolean { Id = Guid.NewGuid(), Position = 1, Boolean = false };
            ItemQuery query2 = new ItemQuery { Id = Guid.NewGuid(), Position = 2, QueryTextValue = "SELECT Column FROM TABLE" };
            MayorCondition condition2 = new MayorCondition { Id = Guid.NewGuid(), Position = 2, Components = new List<Component> { boolean, query2 } };

            OrCondition orCondition = new OrCondition { Id = Guid.NewGuid(), Position = 1, Components = new List<Component> { condition1, condition2 } };

            List<Component> result = orCondition.Accept(new VisitorComponentToList());
            Assert.AreEqual(7, result.Count);

            OrCondition andResult = result.Single(c => c.Id == orCondition.Id) as OrCondition;
            Assert.AreEqual(2, andResult.Components.Count);
            Assert.AreEqual(1, andResult.Position);

            EqualsCondition equalsResult = result.Single(c => c.Id == condition1.Id) as EqualsCondition;
            Assert.AreEqual(2, equalsResult.Components.Count);
            Assert.AreEqual(1, equalsResult.Position);

            ItemText textResult = result.Single(c => c.Id == text.Id) as ItemText;
            Assert.AreEqual("Test Text Or Condition", textResult.TextValue);
            Assert.AreEqual(1, textResult.Position);

            ItemQuery query1Result = result.Single(c => c.Id == query1.Id) as ItemQuery;
            Assert.AreEqual("SELECT COUNT(*) FROM TABLE", query1Result.QueryTextValue);
            Assert.AreEqual(2, query1Result.Position);

            MayorCondition mayorResult = result.Single(c => c.Id == condition2.Id) as MayorCondition;
            Assert.AreEqual(2, mayorResult.Components.Count);
            Assert.AreEqual(2, mayorResult.Position);

            ItemBoolean booleanResult = result.Single(c => c.Id == text.Id) as ItemBoolean;
            Assert.IsFalse(boolean.Boolean);
            Assert.AreEqual(1, textResult.Position);

            ItemQuery query2Result = result.Single(c => c.Id == query2.Id) as ItemQuery;
            Assert.AreEqual("SELECT Column FROM TABLE", query2Result.QueryTextValue);
            Assert.AreEqual(2, query2Result.Position);
        }
    } 
}