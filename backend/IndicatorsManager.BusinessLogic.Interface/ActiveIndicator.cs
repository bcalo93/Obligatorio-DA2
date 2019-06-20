using System.Collections.Generic;
using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public class ActiveIndicator : IndicatorConfiguration
    {
        public IEnumerable<IndicatorItem> ActiveItems { get; set; }

        public ActiveIndicator() { }
    } 
    
}