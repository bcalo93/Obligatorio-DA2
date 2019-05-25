using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class IndicatorCreateModel
    {
        public string Name { get; set; }

        public IEnumerable<IndicatorItemPersistModel> Items { get; set; }

        public IndicatorCreateModel()
        {
            this.Items = new List<IndicatorItemPersistModel>();
        }

        public IndicatorCreateModel(Indicator indicator)
        {
            this.Items = indicator.IndicatorItems.Select(i => new IndicatorItemPersistModel(i));
        }

        public Indicator ToEntity() => new Indicator
        {
            Name = this.Name,
            IndicatorItems = this.Items.Select(i => i.ToEntity()).ToList()
        };
    }
}