using System.Collections.Generic;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public class ImporterInfo
    {
        public string Name { get; set; }
        public Dictionary<string,string> Parameters { get; set; }
    }
}