using System;
using System.Collections.Generic;
using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public class IndicatorResult
    {
        public Indicator Indicator { get; set; }
        public IEnumerable<IndicatorItemResult> ItemsResults { get; set; }

        public IndicatorResult()
        {
            ItemsResults = new List<IndicatorItemResult>();
        }
    }
    
}