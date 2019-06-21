using System;

namespace IndicatorsManager.Domain
{
    public class IndicatorItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual Component Condition { get; set; }

        public virtual Indicator Indicator { get; set; }

        public IndicatorItem() { }
        
    }
    
}