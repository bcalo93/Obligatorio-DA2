using System;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public class IndicatorItemImport
    {
        public string Name { get; set; }
        public ComponentImport Condition { get; set; }
    }
    
}