using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class IndicatorGetModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<IndicatorItemGetModel> Items { get; set; }

        public IndicatorGetModel() 
        { 
            this.Items = new List<IndicatorItemGetModel>();
        }

        public IndicatorGetModel(Indicator indicator)
        {
            this.Id = indicator.Id;
            this.Name = indicator.Name;
            this.Items = indicator.IndicatorItems.Select(i => new IndicatorItemGetModel(i));
        }
    }
    
}