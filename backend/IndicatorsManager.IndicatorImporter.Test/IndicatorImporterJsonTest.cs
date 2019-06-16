using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.IndicatorImporter.Json;
using IndicatorsManager.IndicatorImporter.Interface;
using IndicatorsManager.IndicatorImporter.Interface.Exceptions;

namespace IndicatorsManager.IndicatorImporter.Test
{
    [TestClass]
    public class IndicatorImporterJsonTest
    {
        private const string jsonOkTest = @"C:\Users\Brahian\Documents\Facultad\Diseno 2\Obligatorio 2\backend\IndicatorsManager.IndicatorImporter.Test\testjsons\oktest.json";
        private const string jsonWrongTest = @"C:\Users\Brahian\Documents\Facultad\Diseno 2\Obligatorio 2\backend\IndicatorsManager.IndicatorImporter.Test\testjsons\wrongtest.json";

        [TestMethod]
        public void GetNameTest()
        {
            IndicatorImporterJson importer = new IndicatorImporterJson();
            string name = importer.GetName();
            Assert.AreEqual("Json Importer", name);
        }

        [TestMethod]
        public void GetParametersTest()
        {
            IndicatorImporterJson importer = new IndicatorImporterJson();
            Dictionary<string, string> parameters = importer.GetParameters();
            Assert.AreEqual(1, parameters.Count);
            Assert.IsTrue(parameters.ContainsKey("filePath"));
        }
        
        [TestMethod]
        public void ImportIndicatorsOkTest()
        {
            IndicatorImporterJson importer = new IndicatorImporterJson();
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"filePath", jsonOkTest }
            };
            IEnumerable<IndicatorImport> result = importer.ImportIndicators(parameters);
            
            Assert.AreEqual(2, result.Count());
            IndicatorImport indicator1 = result.Single(i => i.Name == "Indicator Import Test 1");
            Assert.AreEqual(3, indicator1.Items.Count);
            
            // Red Item
            IndicatorItemImport item11 = indicator1.Items.Single(ii => ii.Name == "Red");
            ConditionImport condition1 = item11.Condition as ConditionImport;
            Assert.AreEqual(1, condition1.Position);
            Assert.AreEqual(2, condition1.Components.Count);
            Assert.AreEqual(ConditionType.Greater, condition1.ConditionType);
            ItemQueryImport condition11 = condition1.Components.Single(c => c.Position == 1) as ItemQueryImport;
            Assert.AreEqual("SELECT COUNT(*) FROM category;", condition11.Query);
            ItemNumberImport condition12 = condition1.Components.Single(c => c.Position == 2) as ItemNumberImport;
            Assert.AreEqual(6, condition12.Number);

            // Yellow Item
            IndicatorItemImport item12 = indicator1.Items.Single(ii => ii.Name == "Yellow");
            ConditionImport condition2 = item12.Condition as ConditionImport;
            Assert.AreEqual(1, condition2.Position);
            Assert.AreEqual(2, condition2.Components.Count);
            Assert.AreEqual(ConditionType.And, condition2.ConditionType);

            ConditionImport condition21 = condition2.Components.Single(c => c.Position == 1) as ConditionImport;
            Assert.AreEqual(2, condition21.Components.Count);
            Assert.AreEqual(ConditionType.MinorEquals, condition21.ConditionType);
            ItemQueryImport condition211 = condition21.Components.Single(c => c.Position == 1) as ItemQueryImport;
            Assert.AreEqual("SELECT COUNT(*) FROM category;", condition211.Query);
            ItemNumberImport condition212 = condition21.Components.Single(c => c.Position == 2) as ItemNumberImport;
            Assert.AreEqual(5, condition212.Number);

            ConditionImport condition22 = condition2.Components.Single(c => c.Position == 2) as ConditionImport;
            Assert.AreEqual(2, condition22.Components.Count);
            Assert.AreEqual(ConditionType.GreaterEquals, condition22.ConditionType);
            ItemQueryImport condition221 = condition22.Components.Single(c => c.Position == 1) as ItemQueryImport;
            Assert.AreEqual("SELECT COUNT(*) FROM category;", condition221.Query);
            ItemNumberImport condition222 = condition22.Components.Single(c => c.Position == 2) as ItemNumberImport;
            Assert.AreEqual(3, condition222.Number);
            
            // Green Item
            IndicatorItemImport item13 = indicator1.Items.Single(ii => ii.Name == "Green");
            ConditionImport condition3 = item13.Condition as ConditionImport;
            Assert.AreEqual(1, condition3.Position);
            Assert.AreEqual(2, condition3.Components.Count);
            Assert.AreEqual(ConditionType.Minor, condition3.ConditionType);
            ItemQueryImport condition31 = condition3.Components.Single(c => c.Position == 1) as ItemQueryImport;
            Assert.AreEqual("SELECT COUNT(*) FROM category;", condition31.Query);
            ItemNumberImport condition32 = condition3.Components.Single(c => c.Position == 2) as ItemNumberImport;
            Assert.AreEqual(3, condition32.Number);

            
            IndicatorImport indicator2 = result.Single(i => i.Name == "Indicator Import Test 2");
            Assert.AreEqual(3, indicator2.Items.Count);
            
            // Red Item
            IndicatorItemImport item21 = indicator2.Items.Single(ii => ii.Name == "Red");
            ConditionImport condition4 = item21.Condition as ConditionImport;
            Assert.AreEqual(1, condition4.Position);
            Assert.AreEqual(2, condition4.Components.Count);
            Assert.AreEqual(ConditionType.Equals, condition4.ConditionType);
            ItemQueryImport condition41 = condition4.Components.Single(c => c.Position == 1) as ItemQueryImport;
            Assert.AreEqual("SELECT COUNT(*) As TotalInglesEspanol FROM Account WHERE LangPref ='Spanish' OR LangPref = 'English'", condition41.Query);
            ItemBooleanImport condition42 = condition4.Components.Single(c => c.Position == 2) as ItemBooleanImport;
            Assert.AreEqual(true, condition42.Boolean);

            // Yellow Item
            IndicatorItemImport item22 = indicator2.Items.Single(ii => ii.Name == "Yellow");
            ConditionImport condition5 = item22.Condition as ConditionImport;
            Assert.AreEqual(1, condition5.Position);
            Assert.AreEqual(2, condition5.Components.Count);
            Assert.AreEqual(ConditionType.MinorEquals, condition5.ConditionType);
            ItemTextImport condition51 = condition5.Components.Single(c => c.Position == 1) as ItemTextImport;
            Assert.AreEqual("Test Text", condition51.Text);
            ItemQueryImport condition52 = condition5.Components.Single(c => c.Position == 2) as ItemQueryImport;
            Assert.AreEqual("SELECT COUNT(*) As TotalInglesEspanol FROM Account WHERE LangPref ='Spanish' OR LangPref = 'English'", condition52.Query);
        
            // Green Item
            IndicatorItemImport item23 = indicator2.Items.Single(ii => ii.Name == "Green");
            ConditionImport condition6 = item23.Condition as ConditionImport;
            Assert.AreEqual(1, condition6.Position);
            Assert.AreEqual(2, condition6.Components.Count);
            Assert.AreEqual(ConditionType.Or, condition6.ConditionType);
            ConditionImport condition61 = condition6.Components.Single(c => c.Position == 1) as ConditionImport;
            Assert.AreEqual(2, condition61.Components.Count);
            Assert.AreEqual(ConditionType.GreaterEquals, condition61.ConditionType);
            ItemQueryImport condition611 = condition61.Components.Single(c => c.Position == 1) as ItemQueryImport;
            Assert.AreEqual("SELECT COUNT(*) As TotalInglesEspanol FROM Account WHERE LangPref = 'Spanish' OR LangPref = 'English'", condition611.Query);
            ItemDateImport condition612 = condition61.Components.Single(c => c.Position == 2) as ItemDateImport;
            Assert.AreEqual(new DateTime(2013, 10, 21, 13, 28, 6), condition612.Date);

            ConditionImport condition62 = condition6.Components.Single(c => c.Position == 2) as ConditionImport;
            Assert.AreEqual(2, condition62.Components.Count);
            Assert.AreEqual(ConditionType.Greater, condition62.ConditionType);
            ItemQueryImport condition621 = condition62.Components.Single(c => c.Position == 1) as ItemQueryImport;
            Assert.AreEqual("SELECT COUNT(*) As TotalEspanol FROM Account WHERE LangPref = 'Spanish'", condition621.Query);
            ItemQueryImport condition622 = condition62.Components.Single(c => c.Position == 2) as ItemQueryImport;
            Assert.AreEqual("SELECT COUNT(*) As TotalIngles FROM Account WHERE LangPref = 'English'", condition622.Query);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void ImportIndicatorIncorrectJsonTest()
        {
            IndicatorImporterJson importer = new IndicatorImporterJson();
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"filePath", jsonWrongTest }
            };
            IEnumerable<IndicatorImport> result = importer.ImportIndicators(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectParameterException))]
        public void ImportIndicatorsWrongPathTest()
        {
            IndicatorImporterJson importer = new IndicatorImporterJson();
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"filePath", "Wrong path" }
            };
            IEnumerable<IndicatorImport> result = importer.ImportIndicators(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectParameterException))]
        public void ImportIndicatorsWrongParameterTest()
        {
            IndicatorImporterJson importer = new IndicatorImporterJson();
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"filePat", jsonOkTest }
            };
            IEnumerable<IndicatorImport> result = importer.ImportIndicators(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectParameterException))]
        public void ImportIndicatorsParameterValueMissingTest()
        {
            IndicatorImporterJson importer = new IndicatorImporterJson();
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"filePath", " " }
            };
            IEnumerable<IndicatorImport> result = importer.ImportIndicators(parameters);
        }
    }
}
