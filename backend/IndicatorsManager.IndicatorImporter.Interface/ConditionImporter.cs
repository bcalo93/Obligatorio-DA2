using System;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public abstract class ConditionImporter : ComponentImporter
    {
        public List<ComponentImporter> ComponentImporters { get; set; }

        public ConditionImporter()
        {
            this.ComponentImporters = new List<ComponentImporter>();
        }
    }
}