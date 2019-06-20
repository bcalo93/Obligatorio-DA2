using System;
using System.Collections.Generic;

namespace IndicatorsManager.Domain
{
    public class Indicator
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual Area Area { get; set; }
        public virtual List<UserIndicator> UserIndicators { get; set; }
        public virtual List<IndicatorItem> IndicatorItems { get; set;  }

        public Indicator() 
        { 
            this.UserIndicators = new List<UserIndicator>();
            this.IndicatorItems = new List<IndicatorItem>();
        }

        public Indicator Update(Indicator indicator)
        {
            this.Name = indicator.Name;
            return this;
        }
    }
}