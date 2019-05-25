using System;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class IndicatorOnlyModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public IndicatorOnlyModel() { }

        public IndicatorOnlyModel(Indicator indicator)
        {
            this.Id = indicator.Id;
            this.Name = indicator.Name;
        }

        public Indicator ToEntity() => new Indicator
        {
            Name = this.Name
        };
    }
    
}