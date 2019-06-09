using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;
using IndicatorsManager.BusinessLogic.Interface;

namespace IndicatorsManager.WebApi.Models
{
    public class IndicatorConfigModel : IndicatorGetModel
    {
        public int? Position { get; set; }
        public bool IsVisible { get; set; }
        public string Alias { get; set; }
        
        public IndicatorConfigModel() : base() { }

        public IndicatorConfigModel(IndicatorConfiguration configuration) : base(configuration.Indicator)
        {
            this.Position = configuration.Position;
            this.IsVisible = configuration.IsVisible;
            this.Alias = configuration.Alias;
        }
    }
}