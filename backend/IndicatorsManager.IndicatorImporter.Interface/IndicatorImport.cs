using System.Collections.Generic;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public class IndicatorImport
    {
        public string Name { get; set; }
        public List<IndicatorItemImport> Items { get; set;  }

        public IndicatorImport()
        {
            this.Items = new List<IndicatorItemImport>();
        }
    }
}