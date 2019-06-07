using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class IndicatorConfigModel : IndicatorGetModel
    {
        public int? Position { get; set; }
        public bool IsVisible { get; set; }
        public IndicatorConfigModel() : base() { }

        public IndicatorConfigModel(Indicator indicator) : base(indicator)
        {
            UserIndicator config = indicator.UserIndicators.FirstOrDefault();
            if(config != null)
            {
                Position = config.Position;
                IsVisible = config.IsVisible;
            }
            else
            {
                IsVisible = true;
            }
        }
    }
}