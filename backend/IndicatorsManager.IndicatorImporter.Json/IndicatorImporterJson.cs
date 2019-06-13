using System.Collections.Generic;
using IndicatorsManager.IndicatorImporter.Interface;

namespace IndicatorsManager.IndicatorImporter.Json
{
    public class IndicatorImporterJson : IIndicatorImporter
    {
        public string GetName()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Interface.IndicatorImport> ImportIndicators(string filePath)
        {
            throw new System.NotImplementedException();
        }
    }
}