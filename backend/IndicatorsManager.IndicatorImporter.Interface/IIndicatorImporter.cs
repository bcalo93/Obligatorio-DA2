using System.Collections.Generic;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public interface IIndicatorImporter
    {
        string GetName();
        Dictionary<string, string> GetParameters();
        IEnumerable<IndicatorImport> ImportIndicators(Dictionary<string, string> parameters);
    }
}