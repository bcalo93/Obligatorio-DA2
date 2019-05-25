using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Models;
using IndicatorsManager.WebApi.Visitors;
using System.Linq;

namespace IndicatorsManager.WebApi.Test
{
    [TestClass]
    public class ComponentModelVisitorTest
    {

        [TestMethod]
        public void ConvertItemNumericModelOkTest()
        {
            ItemNumeric numeric = new ItemNumeric{ Position = 1, NumberValue = 20 };

            ComponentModel result = numeric.Accept(new ComponentModelVisitor());

            IntItemModel model = result as IntItemModel;
            Assert.AreEqual(1, model.Position);
            Assert.AreEqual(20, model.Value);
        }

        [TestMethod]
        public void ConvertItemTextModelOkTest()
        {
            ItemText text = new ItemText{ Position = 3, TextValue = "Test text item" };

            ComponentModel result = text.Accept(new ComponentModelVisitor());

            StringItemModel model = result as StringItemModel;
            Assert.AreEqual(3, model.Position);
            Assert.AreEqual("Test text item", model.Value);
            Assert.AreEqual("Text", model.Type);
        }

        [TestMethod]
        public void ConvertItemQueryModelOkTest()
        {
            ItemQuery query = new ItemQuery{ Position = 2, QueryTextValue = "SELECT FROM TABLE" };

            ComponentModel result = query.Accept(new ComponentModelVisitor());

            StringItemModel model = result as StringItemModel;
            Assert.AreEqual(2, model.Position);
            Assert.AreEqual("SELECT FROM TABLE", model.Value);
            Assert.AreEqual("Sql", model.Type);
        }
        

        [TestMethod]
        public void ConvertAndConditionModelOkTest()
        {
            ItemQuery query1 = new ItemQuery{ Position = 1, QueryTextValue = "SELECT FROM TABLEA" };
            ItemQuery query2 = new ItemQuery{ Position = 2, QueryTextValue = "SELECT FROM TABLEB" };
            AndCondition condition = new AndCondition { Position = 1, Components = new List<Component> { query1, query2 } };

            ComponentModel result = condition.Accept(new ComponentModelVisitor());

            ConditionModel model = result as ConditionModel;
            Assert.AreEqual("And", model.ConditionType);
            Assert.AreEqual(1, model.Position);
            Assert.AreEqual(2, model.Components.Count());

            StringItemModel part1 = model.Components.Single(s => s.Position == 1) as StringItemModel;
            Assert.AreEqual("SELECT FROM TABLEA", part1.Value);
            Assert.AreEqual("Sql", part1.Type);

            StringItemModel part2 = model.Components.Single(s => s.Position == 2) as StringItemModel;
            Assert.AreEqual("SELECT FROM TABLEB", part2.Value);
            Assert.AreEqual("Sql", part2.Type);
        }

        [TestMethod]
        public void ConvertOrConditionModelOkTest()
        {
            ItemText text = new ItemText{ Position = 2, TextValue = "Text test" };
            ItemQuery query = new ItemQuery{ Position = 1, QueryTextValue = "SELECT FROM TABLEB" };
            OrCondition condition = new OrCondition { Position = 2, Components = new List<Component> { query, text } };

            ComponentModel result = condition.Accept(new ComponentModelVisitor());

            ConditionModel model = result as ConditionModel;
            Assert.AreEqual("Or", model.ConditionType);
            Assert.AreEqual(2, model.Position);
            Assert.AreEqual(2, model.Components.Count());

            StringItemModel part1 = model.Components.Single(s => s.Position == 1) as StringItemModel;
            Assert.AreEqual("SELECT FROM TABLEB", part1.Value);
            Assert.AreEqual("Sql", part1.Type);

            StringItemModel part2 = model.Components.Single(s => s.Position == 2) as StringItemModel;
            Assert.AreEqual("Text test", part2.Value);
            Assert.AreEqual("Text", part2.Type);
        }

        [TestMethod]
        public void ConvertMayorConditionModelOkTest()
        {
            ItemNumeric numeric = new ItemNumeric{ Position = 3, NumberValue = 20 };
            ItemQuery query = new ItemQuery{ Position = 1, QueryTextValue = "SELECT FROM TABLE" };
            MayorCondition condition = new MayorCondition { Position = 3, Components = new List<Component> { query, numeric } };

            ComponentModel result = condition.Accept(new ComponentModelVisitor());

            ConditionModel model = result as ConditionModel;
            Assert.AreEqual("Mayor", model.ConditionType);
            Assert.AreEqual(3, model.Position);
            Assert.AreEqual(2, model.Components.Count());

            StringItemModel part1 = model.Components.Single(s => s.Position == 1) as StringItemModel;
            Assert.AreEqual("SELECT FROM TABLE", part1.Value);
            Assert.AreEqual("Sql", part1.Type);

            IntItemModel part2 = model.Components.Single(s => s.Position == 3) as IntItemModel;
            Assert.AreEqual(20, part2.Value);
        }

        [TestMethod]
        public void ConvertMayorEqualsConditionModelOkTest()
        {
            ItemNumeric numeric = new ItemNumeric{ Position = 1, NumberValue = 10 };
            ItemQuery query = new ItemQuery{ Position = 2, QueryTextValue = "SELECT FROM TABLE" };
            MayorEqualsCondition condition = new MayorEqualsCondition { Position = 1, Components = new List<Component> { query, numeric } };

            ComponentModel result = condition.Accept(new ComponentModelVisitor());

            ConditionModel model = result as ConditionModel;
            Assert.AreEqual("MayorEquals", model.ConditionType);
            Assert.AreEqual(1, model.Position);
            Assert.AreEqual(2, model.Components.Count());

            IntItemModel part1 = model.Components.Single(s => s.Position == 1) as IntItemModel;
            Assert.AreEqual(10, part1.Value);

            StringItemModel part2 = model.Components.Single(s => s.Position == 2) as StringItemModel;
            Assert.AreEqual("SELECT FROM TABLE", part2.Value);
            Assert.AreEqual("Sql", part2.Type);
        }

        [TestMethod]
        public void ConvertMinorConditionModelOkTest()
        {
            ItemText numeric = new ItemText{ Position = 1, TextValue = "Test" };
            ItemQuery query = new ItemQuery{ Position = 2, QueryTextValue = "SELECT FROM TABLE" };
            MinorCondition condition = new MinorCondition { Position = 3, Components = new List<Component> { query, numeric } };

            ComponentModel result = condition.Accept(new ComponentModelVisitor());

            ConditionModel model = result as ConditionModel;
            Assert.AreEqual("Minor", model.ConditionType);
            Assert.AreEqual(3, model.Position);
            Assert.AreEqual(2, model.Components.Count());

            StringItemModel part1 = model.Components.Single(s => s.Position == 1) as StringItemModel;
            Assert.AreEqual("Test", part1.Value);
            Assert.AreEqual("Text", part1.Type);

            StringItemModel part2 = model.Components.Single(s => s.Position == 2) as StringItemModel;
            Assert.AreEqual("SELECT FROM TABLE", part2.Value);
            Assert.AreEqual("Sql", part2.Type);
        }

        [TestMethod]
        public void ConvertMinorEqualsConditionModelOkTest()
        {
            ItemText numeric = new ItemText{ Position = 1, TextValue = "Test Minor Equals" };
            ItemQuery query = new ItemQuery{ Position = 2, QueryTextValue = "SELECT FROM MinorEquals" };
            MinorEqualsCondition condition = new MinorEqualsCondition { Position = 30, Components = new List<Component> { query, numeric } };

            ComponentModel result = condition.Accept(new ComponentModelVisitor());

            ConditionModel model = result as ConditionModel;
            Assert.AreEqual("MinorEquals", model.ConditionType);
            Assert.AreEqual(30, model.Position);
            Assert.AreEqual(2, model.Components.Count());

            StringItemModel part1 = model.Components.Single(s => s.Position == 1) as StringItemModel;
            Assert.AreEqual("Test Minor Equals", part1.Value);
            Assert.AreEqual("Text", part1.Type);

            StringItemModel part2 = model.Components.Single(s => s.Position == 2) as StringItemModel;
            Assert.AreEqual("SELECT FROM MinorEquals", part2.Value);
            Assert.AreEqual("Sql", part2.Type);
        }

        [TestMethod]
        public void ConvertEqualsConditionModelOkTest()
        {
            ItemText numeric = new ItemText{ Position = 1, TextValue = "Test Equals" };
            ItemQuery query = new ItemQuery{ Position = 2, QueryTextValue = "SELECT FROM Equals" };
            EqualsCondition condition = new EqualsCondition { Position = 20, Components = new List<Component> { query, numeric } };

            ComponentModel result = condition.Accept(new ComponentModelVisitor());

            ConditionModel model = result as ConditionModel;
            Assert.AreEqual("Equals", model.ConditionType);
            Assert.AreEqual(20, model.Position);
            Assert.AreEqual(2, model.Components.Count());

            StringItemModel part1 = model.Components.Single(s => s.Position == 1) as StringItemModel;
            Assert.AreEqual("Test Equals", part1.Value);
            Assert.AreEqual("Text", part1.Type);

            StringItemModel part2 = model.Components.Single(s => s.Position == 2) as StringItemModel;
            Assert.AreEqual("SELECT FROM Equals", part2.Value);
            Assert.AreEqual("Sql", part2.Type);
        }

        [TestMethod]
        public void ConvertComponentModelMultipleLevelsOkTest()
        {
            ItemNumeric numeric = new ItemNumeric{ Position = 1, NumberValue = 100 };
            ItemQuery query1 = new ItemQuery{ Position = 2, QueryTextValue = "SELECT FROM Equals" };
            EqualsCondition mayor = new EqualsCondition { Position = 1, Components = new List<Component> { query1, numeric } };

            ItemText text = new ItemText{ Position = 1, TextValue = "Test Equals" };
            ItemQuery query2 = new ItemQuery{ Position = 2, QueryTextValue = "SELECT FROM MayorEquals" };
            MayorEqualsCondition mayorEquals = new MayorEqualsCondition { Position = 2, Components = new List<Component> { query2, text } };

            OrCondition condition = new OrCondition { Position = 2, Components = new List<Component> { mayor, mayorEquals} };

            ComponentModel result = condition.Accept(new ComponentModelVisitor());

            ConditionModel model = result as ConditionModel;
            Assert.AreEqual("Or", model.ConditionType);
            Assert.AreEqual(2, model.Position);
            Assert.AreEqual(2, model.Components.Count());

            // Part 1
            ConditionModel part1 = model.Components.Single(c => c.Position == 1) as ConditionModel;
            Assert.AreEqual("Equals", part1.ConditionType);
            Assert.AreEqual(2, part1.Components.Count());

            IntItemModel part11 = part1.Components.Single(s => s.Position == 1) as IntItemModel;
            Assert.AreEqual(100, part11.Value);

            StringItemModel part12 = part1.Components.Single(s => s.Position == 2) as StringItemModel;
            Assert.AreEqual("SELECT FROM Equals", part12.Value);
            Assert.AreEqual("Sql", part12.Type);

            // Part 2
            ConditionModel part2 = model.Components.Single(c => c.Position == 2) as ConditionModel;
            Assert.AreEqual("MayorEquals", part2.ConditionType);
            Assert.AreEqual(2, part2.Components.Count());

            StringItemModel part21 = part2.Components.Single(s => s.Position == 1) as StringItemModel;
            Assert.AreEqual("Test Equals", part21.Value);

            StringItemModel part22 = part2.Components.Single(s => s.Position == 2) as StringItemModel;
            Assert.AreEqual("SELECT FROM MayorEquals", part22.Value);
            Assert.AreEqual("Sql", part22.Type);
        }
    }
    
}