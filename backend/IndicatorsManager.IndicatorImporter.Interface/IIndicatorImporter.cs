using System.Collections.Generic;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public interface IIndicatorImporter
    {
        string GetName();
        IEnumerable<Parameter> GetParameters();
        IEnumerable<IndicatorImport> ImportIndicators(Dictionary<string, string> parameters);
    }
}