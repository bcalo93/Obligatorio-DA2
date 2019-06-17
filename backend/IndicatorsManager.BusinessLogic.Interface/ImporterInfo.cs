using System.Collections.Generic;
using IndicatorsManager.IndicatorImporter.Interface;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public class ImporterInfo
    {
        public string Name { get; set; }
        public IEnumerable<Parameter> Parameters { get; set; }
    }
}