using Microsoft.VisualStudio.TestTools.UnitTesting;
using IndicatorsManager.Domain;
using IndicatorsManager.IndicatorImporter.Interface;
using IndicatorsManager.BusinessLogic.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsManager.BusinessLogic.Test
{
    [TestClass]
    public class ConditionImportVisitorToDomainTest
    {
        [TestMethod]
        public void ItemNumberImportToItemNumericOkTest()
        {
            ItemNumberImport numeric = new ItemNumberImport{ Position = 1, Number = 20 };

            Component result = numeric.Accept(new ConditionImportVisitorToDomain());

            ItemNumeric model = result as ItemNumeric;
            Assert.AreEqual(1, model.Position);
            Assert.AreEqual(20, model.NumberValue);
        }

        [TestMethod]
        public void ItemTextImportToItemTextOkTest()
        {
            ItemTextImport text = new ItemTextImport{ Position = 3, Text = "Test text item" };

            Component result = text.Accept(new ConditionImportVisitorToDomain());

            ItemText model = result as ItemText;
            Assert.AreEqual(3, model.Position);
            Assert.AreEqual("Test text item", model.TextValue);
        }

        [TestMethod]
        public void ItemQueryImportToItemQueryOkTest()
        {
            ItemQueryImport query = new ItemQueryImport{ Position = 2, Query = "SELECT FROM TABLE" };

            Component result = query.Accept(new ConditionImportVisitorToDomain());

            ItemQuery model = result as ItemQuery;
            Assert.AreEqual(2, model.Position);
            Assert.AreEqual("SELECT FROM TABLE", model.QueryTextValue);
        }
        
        [TestMethod]
        public void ItemBooleanImportToItemBooleanOkTest()
        {
            ItemBooleanImport boolean = new ItemBooleanImport{ Position = 3, Boolean = false };

            Component result = boolean.Accept(new ConditionImportVisitorToDomain());

            ItemBoolean model = result as ItemBoolean;
            Assert.AreEqual(3, model.Position);
            Assert.IsFalse(model.Boolean);
        }

        [TestMethod]
        public void ItemDateImportToItemDateOkTest()
        {
            DateTime expectedDate = new DateTime(2017, 5, 30);
            ItemDateImport date = new ItemDateImport{ Position = 2, Date = expectedDate };

            Component result = date.Accept(new ConditionImportVisitorToDomain());

            ItemDate model = result as ItemDate;
            Assert.AreEqual(2, model.Position);
            Assert.AreEqual(expectedDate, model.Date);
        }

        [TestMethod]
        public void ConditionImportToAndConditionOkTest()
        {
            ItemQueryImport query1 = new ItemQueryImport{ Position = 1, Query = "SELECT FROM TABLEA" };
            ItemQueryImport query2 = new ItemQueryImport{ Position = 2, Query = "SELECT FROM TABLEB" };
            ConditionImport condition = new ConditionImport { Position = 1, 
                Components = new List<ComponentImport> { query1, query2 }, 
                ConditionType = ConditionType.And };

            Component result = condition.Accept(new ConditionImportVisitorToDomain());

            AndCondition andCondition = result as AndCondition;
            Assert.AreEqual(1, andCondition.Position);
            Assert.AreEqual(2, andCondition.Components.Count());

            ItemQuery part1 = andCondition.Components.Single(s => s.Position == 1) as ItemQuery;
            Assert.AreEqual("SELECT FROM TABLEA", part1.QueryTextValue);

            ItemQuery part2 = andCondition.Components.Single(s => s.Position == 2) as ItemQuery;
            Assert.AreEqual("SELECT FROM TABLEB", part2.QueryTextValue);
        }

        [TestMethod]
        public void ConditionImportToOrConditionOkTest()
        {
            ItemTextImport text = new ItemTextImport{ Position = 2, Text = "Text test" };
            ItemQueryImport query = new ItemQueryImport{ Position = 1, Query = "SELECT FROM TABLEB" };
            ConditionImport condition = new ConditionImport { Position = 2, 
                 Components = new List<ComponentImport> { query, text }, ConditionType = ConditionType.Or };

            Component result = condition.Accept(new ConditionImportVisitorToDomain());

            OrCondition orCondition = result as OrCondition;
            Assert.AreEqual(2, orCondition.Position);
            Assert.AreEqual(2, orCondition.Components.Count());

            ItemQuery part1 = orCondition.Components.Single(s => s.Position == 1) as ItemQuery;
            Assert.AreEqual("SELECT FROM TABLEB", part1.QueryTextValue);

            ItemText part2 = orCondition.Components.Single(s => s.Position == 2) as ItemText;
            Assert.AreEqual("Text test", part2.TextValue);
        }

        [TestMethod]
        public void ConditionImportToMayorConditionOkTest()
        {
            ItemNumberImport numeric = new ItemNumberImport{ Position = 3, Number = 20 };
            ItemQueryImport query = new ItemQueryImport{ Position = 1, Query = "SELECT FROM TABLE" };
            ConditionImport condition = new ConditionImport { Position = 3, 
                Components = new List<ComponentImport> { query, numeric }, ConditionType = ConditionType.Greater };

            Component result = condition.Accept(new ConditionImportVisitorToDomain());

            MayorCondition mayorCondition = result as MayorCondition;
            Assert.AreEqual(3, mayorCondition.Position);
            Assert.AreEqual(2, mayorCondition.Components.Count());

            ItemQuery part1 = mayorCondition.Components.Single(s => s.Position == 1) as ItemQuery;
            Assert.AreEqual("SELECT FROM TABLE", part1.QueryTextValue);

            ItemNumeric part2 = mayorCondition.Components.Single(s => s.Position == 3) as ItemNumeric;
            Assert.AreEqual(20, part2.NumberValue);
        }

        [TestMethod]
        public void ConditionImportToMayorEqualsConditionOkTest()
        {
            ItemNumberImport numeric = new ItemNumberImport{ Position = 1, Number = 10 };
            ItemQueryImport query = new ItemQueryImport{ Position = 2, Query = "SELECT FROM TABLE" };
            ConditionImport condition = new ConditionImport { Position = 1, 
                Components = new List<ComponentImport> { query, numeric }, ConditionType = ConditionType.GreaterEquals };

            Component result = condition.Accept(new ConditionImportVisitorToDomain());

            MayorEqualsCondition mayorEqualsCondition = result as MayorEqualsCondition;
            Assert.AreEqual(1, mayorEqualsCondition.Position);
            Assert.AreEqual(2, mayorEqualsCondition.Components.Count());

            ItemNumeric part1 = mayorEqualsCondition.Components.Single(s => s.Position == 1) as ItemNumeric;
            Assert.AreEqual(10, part1.NumberValue);

            ItemQuery part2 = mayorEqualsCondition.Components.Single(s => s.Position == 2) as ItemQuery;
            Assert.AreEqual("SELECT FROM TABLE", part2.QueryTextValue);
        }

        [TestMethod]
        public void ConditionImportToMinorConditionOkTest()
        {
            ItemTextImport numeric = new ItemTextImport{ Position = 1, Text = "Test" };
            ItemQueryImport query = new ItemQueryImport{ Position = 2, Query = "SELECT FROM TABLE" };
            ConditionImport condition = new ConditionImport { Position = 3, 
                Components = new List<ComponentImport> { query, numeric }, ConditionType = ConditionType.Minor };

            Component result = condition.Accept(new ConditionImportVisitorToDomain());

            MinorCondition minorCondition = result as MinorCondition;
            Assert.AreEqual(3, minorCondition.Position);
            Assert.AreEqual(2, minorCondition.Components.Count());

            ItemText part1 = minorCondition.Components.Single(s => s.Position == 1) as ItemText;
            Assert.AreEqual("Test", part1.TextValue);

            ItemQuery part2 = minorCondition.Components.Single(s => s.Position == 2) as ItemQuery;
            Assert.AreEqual("SELECT FROM TABLE", part2.QueryTextValue);
        }

        [TestMethod]
        public void ConditionImportToMinorEqualsConditionOkTest()
        {
            ItemTextImport text = new ItemTextImport{ Position = 1, Text = "Test Minor Equals" };
            ItemQueryImport query = new ItemQueryImport{ Position = 2, Query = "SELECT FROM MinorEquals" };
            ConditionImport condition = new ConditionImport { Position = 30, 
                Components = new List<ComponentImport> { query, text }, ConditionType = ConditionType.MinorEquals };

            Component result = condition.Accept(new ConditionImportVisitorToDomain());

            MinorEqualsCondition minorEquals = result as MinorEqualsCondition;
            Assert.AreEqual(30, minorEquals.Position);
            Assert.AreEqual(2, minorEquals.Components.Count());

            ItemText part1 = minorEquals.Components.Single(s => s.Position == 1) as ItemText;
            Assert.AreEqual("Test Minor Equals", part1.TextValue);

            ItemQuery part2 = minorEquals.Components.Single(s => s.Position == 2) as ItemQuery;
            Assert.AreEqual("SELECT FROM MinorEquals", part2.QueryTextValue);
        }

        [TestMethod]
        public void ConditionImportToEqualsConditionOkTest()
        {
            ItemTextImport text = new ItemTextImport{ Position = 1, Text = "Test Equals" };
            ItemQueryImport query = new ItemQueryImport{ Position = 2, Query = "SELECT FROM Equals" };
            ConditionImport condition = new ConditionImport { Position = 20, 
                Components = new List<ComponentImport> { query, text }, ConditionType = ConditionType.Equals };

            Component result = condition.Accept(new ConditionImportVisitorToDomain());

            EqualsCondition model = result as EqualsCondition;
            Assert.AreEqual(20, model.Position);
            Assert.AreEqual(2, model.Components.Count());

            ItemText part1 = model.Components.Single(s => s.Position == 1) as ItemText;
            Assert.AreEqual("Test Equals", part1.TextValue);

            ItemQuery part2 = model.Components.Single(s => s.Position == 2) as ItemQuery;
            Assert.AreEqual("SELECT FROM Equals", part2.QueryTextValue);
        }

        [TestMethod]
        public void ConditionImportToComponentMultipleLevelsOkTest()
        {
            ItemNumberImport numeric = new ItemNumberImport { Position = 1, Number = 100 };
            ItemQueryImport query1 = new ItemQueryImport { Position = 2, Query = "SELECT FROM Equals" };
            ConditionImport mayor = new ConditionImport { Position = 1, 
                Components = new List<ComponentImport> { query1, numeric }, ConditionType = ConditionType.Equals };

            ItemTextImport text = new ItemTextImport{ Position = 1, Text = "Test Equals" };
            ItemQueryImport query2 = new ItemQueryImport{ Position = 2, Query = "SELECT FROM GreaterEquals" };
            ConditionImport mayorEquals = new ConditionImport { Position = 2, 
                Components = new List<ComponentImport> { query2, text }, ConditionType = ConditionType.GreaterEquals };

            ConditionImport condition = new ConditionImport { Position = 2, ConditionType = ConditionType.Or,
                Components = new List<ComponentImport> { mayor, mayorEquals} };

            Component result = condition.Accept(new ConditionImportVisitorToDomain());

            OrCondition orCondition = result as OrCondition;
            Assert.AreEqual(2, orCondition.Position);
            Assert.AreEqual(2, orCondition.Components.Count());

            // Part 1
            EqualsCondition part1 = orCondition.Components.Single(c => c.Position == 1) as EqualsCondition;
            Assert.AreEqual(2, part1.Components.Count());

            ItemNumeric part11 = part1.Components.Single(s => s.Position == 1) as ItemNumeric;
            Assert.AreEqual(100, part11.NumberValue);

            ItemQuery part12 = part1.Components.Single(s => s.Position == 2) as ItemQuery;
            Assert.AreEqual("SELECT FROM Equals", part12.QueryTextValue);

            // Part 2
            MayorEqualsCondition part2 = orCondition.Components.Single(c => c.Position == 2) as MayorEqualsCondition;
            Assert.AreEqual(2, part2.Components.Count());

            ItemText part21 = part2.Components.Single(s => s.Position == 1) as ItemText;
            Assert.AreEqual("Test Equals", part21.TextValue);

            ItemQuery part22 = part2.Components.Single(s => s.Position == 2) as ItemQuery;
            Assert.AreEqual("SELECT FROM GreaterEquals", part22.QueryTextValue);
        }
    }
}