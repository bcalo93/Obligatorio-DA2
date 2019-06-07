using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.BusinessLogic.Interface;

namespace IndicatorsManager.WebApi.Models
{
    public class ActiveIndicatorModel
    {
        public IndicatorGetModel Indicator { get; set; }
        public IEnumerable<IndicatorItemGetModel> Items { get; set; }

        public ActiveIndicatorModel()
        {
            this.Items = new List<IndicatorItemGetModel>();
        }

        public ActiveIndicatorModel(ActiveIndicator activeIndicator)
        {
            this.Indicator = new IndicatorGetModel(activeIndicator.Indicator);
            this.Items = activeIndicator.ActiveItems.Select(i => new IndicatorItemGetModel(i));
        }
    }
}