using System.Collections.Generic;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public interface IIndicatorImporter
    {
        string GetName();
        IEnumerable<IndicatorImport> ImportIndicators(string filePath);
    }
}