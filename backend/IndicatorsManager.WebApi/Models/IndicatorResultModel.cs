using System;
using IndicatorsManager.BusinessLogic.Interface;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsManager.WebApi.Models
{
    public class IndicatorResultModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<IndicatorItemResultModel> ItemsResult { get; set; }

        public IndicatorResultModel()
        {
            this.ItemsResult = new List<IndicatorItemResultModel>();
        }

        public IndicatorResultModel(IndicatorResult result)
        {
            this.Id = result.Indicator.Id;
            this.Name = result.Indicator.Name;
            this.ItemsResult = result.ItemsResults.Select(i => new IndicatorItemResultModel(i));
        }

    }
    
}