using System.Collections.Generic;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public class ImportResult
    {
        public int IndicatorsImported { get; set; }
        public int TotalIndicators { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}