using System;
using System.Collections.Generic;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public interface IIndicatorImporter
    {
        string GetName();
        IEnumerable<IndicatorrImporter> ImportIndicators(string filePath);
    }
}