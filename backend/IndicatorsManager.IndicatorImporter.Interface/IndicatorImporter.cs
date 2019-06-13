using System;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public class IndicatorrImporter
    {
        public string Name { get; set; }
        public List<IndicatorItemImporter> Items { get; set;  }

        public Indicator()
        {
            this.Items = new List<IndicatorItemImporter>();
        }
    }
}