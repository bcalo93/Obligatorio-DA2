using System;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public class IndicatorItemImporter
    {
        public string Name { get; set; }
        public ComponentImporter ComponentImporter { get; set; }
    }
    
}