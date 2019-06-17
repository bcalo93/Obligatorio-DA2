using System;
using System.Collections.Generic; 

namespace IndicatorsManager.BusinessLogic.Interface
{
    public interface IIndicatorImportLogic
    {
        IEnumerable<ImporterInfo> GetIndicatorImporters();
        ImportResult ImportIndicators(Guid areaId, string importerName, Dictionary<string, string> parameters);
        
    }
}