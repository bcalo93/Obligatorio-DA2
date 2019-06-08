using System;
using System.Linq;
using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public class IndicatorConfiguration
    {
        public Indicator Indicator { get; set; }
        public int? Position { get; set; }
        public bool IsVisible { get; set; }

        public IndicatorConfiguration() { }
    }
}