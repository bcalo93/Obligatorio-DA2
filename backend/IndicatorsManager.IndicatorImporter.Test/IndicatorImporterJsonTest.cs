using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.IndicatorImporter.Json;
using IndicatorsManager.IndicatorImporter.Interface;

namespace IndicatorsManager.IndicatorImporter.Test
{
    [TestClass]
    public class IndicatorImporterJsonTest
    {
        private const string jsonOkTest = @"C:\Users\Brahian\Documents\Facultad\Diseno 2\Obligatorio 2\backend\IndicatorsManager.IndicatorImporter.Test\testjsons\oktest.json";

        [TestMethod]
        public void GetNameOkTest()
        {
            IndicatorImporterJson importer = new IndicatorImporterJson();
            string name = importer.GetName();
            Assert.AreEqual("Json Importer", name);
        }
        
        [TestMethod]
        public void ImportIndicatorsOkTest()
        {
            IndicatorImporterJson importer = new IndicatorImporterJson();
            IEnumerable<IndicatorImport> result = importer.ImportIndicators(jsonOkTest);
            
            Assert.AreEqual(2, result.Count());
            IndicatorImport indicator1 = result.Single(i => i.Name == "Indicator Import Test 1");
            Assert.AreEqual(3, indicator1.Items.Count);
            
            // Red Item
            IndicatorItemImport item11 = indicator1.Items.Single(ii => ii.Name == "Red");
            ConditionImport condition1 = item11.Condition as ConditionImport;
            Assert.AreEqual(1, condition1.Position);
            Assert.AreEqual(2, condition1.Components.Count);
            Assert.AreEqual(ConditionType.Mayor, condition1.ConditionType);
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
            Assert.AreEqual(ConditionType.MayorEquals, condition22.ConditionType);
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
        }
    }
}
