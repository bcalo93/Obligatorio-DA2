using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.BusinessLogic.Interface;

namespace IndicatorsManager.WebApi.Models
{
    public class ActiveIndicatorModel : IndicatorConfigModel
    {
        public IEnumerable<string> ActiveItems { get; set; }

        public ActiveIndicatorModel()
        {
            this.ActiveItems = new List<string>();
        }

        public ActiveIndicatorModel(ActiveIndicator activeIndicator) : base(activeIndicator)
        {
            this.ActiveItems = activeIndicator.ActiveItems.Select(i => i.Name);
        }
    }
}