using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using IndicatorsManager.IndicatorImporter.Json;
using IndicatorsManager.IndicatorImporter.Interface;

namespace IndicatorsManager.IndicatorImporter.Test
{
    [TestClass]
    public class IndicatorImporterJsonTest
    {
        private const string jsonOkTest = @"backend\IndicatorsManager.IndicatorImporter.Test\testjsons\oktest.json";

        [TestMethod]
        public void ImportIndicatorsOkTest()
        {
            IndicatorImporterJson importer = new IndicatorImporterJson();
            IEnumerable<IndicatorImport> result = importer.ImportIndicators(jsonOkTest);
        }
    }
}
