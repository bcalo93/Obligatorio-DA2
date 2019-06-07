using System;
using System.Collections.Generic;
using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public class ActiveIndicator 
    {
        public Indicator Indicator { get; set; }
        public IEnumerable<IndicatorItem> ActiveItems { get; set; }

        public ActiveIndicator() { }
    } 
    
}