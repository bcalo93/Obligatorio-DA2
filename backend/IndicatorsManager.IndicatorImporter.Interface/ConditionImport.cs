using System.Collections.Generic;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public abstract class ConditionImport : ComponentImport
    {
        public List<ComponentImport> ComponentImports { get; set; }

        public ConditionImport()
        {
            this.ComponentImports = new List<ComponentImport>();
        }
    }
}