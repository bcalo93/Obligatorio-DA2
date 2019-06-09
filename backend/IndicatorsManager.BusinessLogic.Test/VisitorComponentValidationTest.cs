using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IndicatorsManager.Domain;
using IndicatorsManager.BusinessLogic.Visitors;

namespace IndicatorsManager.BusinessLogic.Test
{
    [TestClass]
    public class VisitorComponentValidationTest
    {
        
        [TestMethod]
        public void ValidateItemNumericOkTest()
        {
            ItemNumeric numeric = new ItemNumeric { NumberValue = 232};
            bool result = numeric.Accept(new VisitorComponentValidation());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateItemTextOkTest()
        {
            ItemText text = new ItemText { TextValue = "Text Test"};
            bool result = text.Accept(new VisitorComponentValidation());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateItemTextNullTest()
        {
            ItemText text = new ItemText();
            bool result = text.Accept(new VisitorComponentValidation());

            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void ValidateItemQueryOkTest()
        {
            ItemQuery query = new ItemQuery { QueryTextValue = "Query" };
            bool result = query.Accept(new VisitorComponentValidation());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateItemQueryNullTest()
        {
            ItemQuery query = new ItemQuery();
            bool result = query.Accept(new VisitorComponentValidation());

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateItemQueryBlankStringTest()
        {
            ItemQuery query = new ItemQuery{ QueryTextValue = "" };
            bool result = query.Accept(new VisitorComponentValidation());

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateItemQuerySpaceStringTest()
        {
            ItemQuery query = new ItemQuery{ QueryTextValue = "    " };
            bool result = query.Accept(new VisitorComponentValidation());

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateItemBooleanOkTest()
        {
            ItemBoolean boolean = new ItemBoolean();
            bool result = boolean.Accept(new VisitorComponentValidation());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateItemDateOkTest()
        {
            ItemDate date = new ItemDate();
            bool result = date.Accept(new VisitorComponentValidation());

            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void ValidateAndConditionOkTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemText text = new ItemText { Position = 0, TextValue = "2" };
            ItemQuery query = new ItemQuery { Position = 3, QueryTextValue = "Query" };
            AndCondition condition = new AndCondition{ Components = new List<Component> { numeric, text, query } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateAndConditionNotEnoughComponentTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 2 };
            AndCondition condition = new AndCondition{ Components = new List<Component> { numeric } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void ValidateAndConditionRepeatedPositionTest()
        {
            ItemNumeric numeric1 = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemNumeric numeric2 = new ItemNumeric { Position = 1, NumberValue = 3 };
            AndCondition condition = new AndCondition{ Components = new List<Component> { numeric1, numeric2 } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateAndConditionAcceptBooleansTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            AndCondition condition = new AndCondition{ Components = new List<Component> { numeric, boolean } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateOrConditionOkTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 20 };
            ItemText text = new ItemText { Position = 0, TextValue = "223" };
            ItemQuery query = new ItemQuery { Position = 3, QueryTextValue = "Query" };
            OrCondition condition = new OrCondition{ Components = new List<Component> { numeric, text, query } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateOrConditionNotEnoughComponentTest()
        {
            ItemText text = new ItemText { Position = 0, TextValue = "Test" };
            OrCondition condition = new OrCondition{ Components = new List<Component> { text } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void ValidateOrConditionRepeatedPositionTest()
        {
            ItemNumeric numeric1 = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemNumeric numeric2 = new ItemNumeric { Position = 1, NumberValue = 3 };
            OrCondition condition = new OrCondition{ Components = new List<Component> { numeric1, numeric2 } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateOrConditionAcceptBooleansTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            OrCondition condition = new OrCondition{ Components = new List<Component> { numeric, boolean } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateMayorConditionOkTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 20 };
            ItemQuery query = new ItemQuery { Position = 3, QueryTextValue = "Query" };
            MayorCondition condition = new MayorCondition{ Components = new List<Component> { numeric, query } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateMayorConditionNotEnoughComponentTest()
        {
            ItemText text = new ItemText { Position = 0, TextValue = "Test" };
            MayorCondition condition = new MayorCondition{ Components = new List<Component> { text } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMayorConditionToManyComponentsTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 20 };
            ItemText text = new ItemText { Position = 0, TextValue = "223" };
            ItemQuery query = new ItemQuery { Position = 2, QueryTextValue = "Query" };
            MayorCondition condition = new MayorCondition{ Components = new List<Component> { numeric, text, query } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMayorConditionRepeatedPositionTest()
        {
            ItemNumeric numeric1 = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemNumeric numeric2 = new ItemNumeric { Position = 1, NumberValue = 3 };
            MayorCondition condition = new MayorCondition{ Components = new List<Component> { numeric1, numeric2 } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMayorConditionNotAcceptBooleansTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            MayorCondition condition = new MayorCondition{ Components = new List<Component> { numeric, boolean } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMayorEqualsConditionOkTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 20 };
            ItemQuery query = new ItemQuery { Position = 3, QueryTextValue = "Query" };
            MayorEqualsCondition condition = new MayorEqualsCondition{ Components = new List<Component> { numeric, query } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateMayorEqualsConditionNotEnoughComponentTest()
        {
            ItemText text = new ItemText { Position = 0, TextValue = "Test" };
            MayorEqualsCondition condition = new MayorEqualsCondition{ Components = new List<Component> { text } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMayorEqualsConditionToManyComponentsTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 20 };
            ItemText text = new ItemText { Position = 0, TextValue = "223" };
            ItemQuery query = new ItemQuery { Position = 2, QueryTextValue = "Query" };
            MayorEqualsCondition condition = new MayorEqualsCondition{ Components = new List<Component> { numeric, text, query } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMayorEqualsConditionRepeatedPositionTest()
        {
            ItemNumeric numeric1 = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemNumeric numeric2 = new ItemNumeric { Position = 1, NumberValue = 3 };
            MayorEqualsCondition condition = new MayorEqualsCondition{ Components = new List<Component> { numeric1, numeric2 } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMayorEqualsConditionNotAcceptBooleansTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            MayorEqualsCondition condition = new MayorEqualsCondition{ Components = new List<Component> { numeric, boolean } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMinorConditionOkTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 20 };
            ItemQuery query = new ItemQuery { Position = 3, QueryTextValue = "Query" };
            MinorCondition condition = new MinorCondition{ Components = new List<Component> { numeric, query } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateMinorConditionNotEnoughComponentTest()
        {
            ItemText text = new ItemText { Position = 0, TextValue = "Test" };
            MinorCondition condition = new MinorCondition{ Components = new List<Component> { text } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMinorConditionToManyComponentsTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 20 };
            ItemText text = new ItemText { Position = 0, TextValue = "223" };
            ItemQuery query = new ItemQuery { Position = 2, QueryTextValue = "Query" };
            MinorCondition condition = new MinorCondition{ Components = new List<Component> { numeric, text, query } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMinorConditionRepeatedPositionTest()
        {
            ItemNumeric numeric1 = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemNumeric numeric2 = new ItemNumeric { Position = 1, NumberValue = 3 };
            MinorCondition condition = new MinorCondition{ Components = new List<Component> { numeric1, numeric2 } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMinorConditionNotAcceptBooleansTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            MinorCondition condition = new MinorCondition{ Components = new List<Component> { numeric, boolean } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMinorEqualsConditionOkTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 20 };
            ItemQuery query = new ItemQuery { Position = 3, QueryTextValue = "Query" };
            MinorEqualsCondition condition = new MinorEqualsCondition{ Components = new List<Component> { numeric, query } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateMinorEqualsConditionNotEnoughComponentTest()
        {
            ItemText text = new ItemText { Position = 0, TextValue = "Test" };
            MinorEqualsCondition condition = new MinorEqualsCondition{ Components = new List<Component> { text } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMinorEqualsConditionToManyComponentsTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 20 };
            ItemText text = new ItemText { Position = 0, TextValue = "223" };
            ItemQuery query = new ItemQuery { Position = 2, QueryTextValue = "Query" };
            MinorEqualsCondition condition = new MinorEqualsCondition{ Components = new List<Component> { numeric, text, query } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMinorEqualsConditionRepeatedPositionTest()
        {
            ItemNumeric numeric1 = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemNumeric numeric2 = new ItemNumeric { Position = 1, NumberValue = 3 };
            MinorEqualsCondition condition = new MinorEqualsCondition{ Components = new List<Component> { numeric1, numeric2 } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMinorEqualsConditionNotAcceptBooleansTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            MinorEqualsCondition condition = new MinorEqualsCondition{ Components = new List<Component> { numeric, boolean } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateEqualsConditionOkTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 20 };
            ItemQuery query = new ItemQuery { Position = 3, QueryTextValue = "Query" };
            EqualsCondition condition = new EqualsCondition{ Components = new List<Component> { numeric, query } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateEqualsConditionNotEnoughComponentTest()
        {
            ItemText text = new ItemText { Position = 0, TextValue = "Test" };
            EqualsCondition condition = new EqualsCondition{ Components = new List<Component> { text } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateEqualsConditionToManyComponentsTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 20 };
            ItemText text = new ItemText { Position = 0, TextValue = "223" };
            ItemQuery query = new ItemQuery { Position = 2, QueryTextValue = "Query" };
            EqualsCondition condition = new EqualsCondition{ Components = new List<Component> { numeric, text, query } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateEqualsConditionRepeatedPositionTest()
        {
            ItemNumeric numeric1 = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemNumeric numeric2 = new ItemNumeric { Position = 1, NumberValue = 3 };
            EqualsCondition condition = new EqualsCondition{ Components = new List<Component> { numeric1, numeric2 } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateEqualsConditionAcceptBooleansTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemBoolean boolean = new ItemBoolean { Position = 2, Boolean = true };
            EqualsCondition condition = new EqualsCondition{ Components = new List<Component> { numeric, boolean } };

            bool result = condition.Accept(new VisitorComponentValidation());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateMultipleLevelsOkTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemQuery query1 = new ItemQuery { Position = 2, QueryTextValue = "SELECT FROM TABLE" };
            MayorEqualsCondition condition1 = new MayorEqualsCondition{ Position = 1, Components = new List<Component> { numeric, query1 } };

            ItemText text = new ItemText { Position = 1, TextValue = "Hello" };
            ItemQuery query2 = new ItemQuery { Position = 2, QueryTextValue = "SELECT FROM TABLE" };
            MayorEqualsCondition condition2 = new MayorEqualsCondition{ Position = 2, Components = new List<Component> { text, query2 } };

            AndCondition condition3 = new AndCondition { Components = new List<Component> { condition1, condition2 } };

            bool result = condition3.Accept(new VisitorComponentValidation());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateMultipleLevelsWrongQueryTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemQuery query1 = new ItemQuery { Position = 2, QueryTextValue = "" };
            MayorEqualsCondition condition1 = new MayorEqualsCondition{ Position = 1, Components = new List<Component> { numeric, query1 } };

            ItemText text = new ItemText { Position = 1, TextValue = "Hello" };
            ItemQuery query2 = new ItemQuery { Position = 2, QueryTextValue = "SELECT FROM TABLE" };
            MayorEqualsCondition condition2 = new MayorEqualsCondition{ Position = 2, Components = new List<Component> { text, query2 } };

            AndCondition condition3 = new AndCondition { Components = new List<Component> { condition1, condition2 } };

            bool result = condition3.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateMultipleLevelsWrongTextTest()
        {
            ItemNumeric numeric = new ItemNumeric { Position = 1, NumberValue = 2 };
            ItemQuery query1 = new ItemQuery { Position = 2, QueryTextValue = "SELECT FROM TABLE" };
            MayorEqualsCondition condition1 = new MayorEqualsCondition{ Position = 1, Components = new List<Component> { numeric, query1 } };

            ItemText text = new ItemText { Position = 1, TextValue = null };
            ItemQuery query2 = new ItemQuery { Position = 2, QueryTextValue = "SELECT FROM TABLE" };
            MayorEqualsCondition condition2 = new MayorEqualsCondition{ Position = 2, Components = new List<Component> { text, query2 } };

            AndCondition condition3 = new AndCondition { Components = new List<Component> { condition1, condition2 } };

            bool result = condition3.Accept(new VisitorComponentValidation());
            Assert.IsFalse(result);
        }
    }
}