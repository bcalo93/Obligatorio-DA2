using System.Collections.Generic;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public enum ConditionType
    {
        And, Or, Equals, Minor, MinorEquals, Mayor, MayorEquals
    }
    public class ConditionImport : ComponentImport
    {
        public List<ComponentImport> Components { get; set; }
        public ConditionType ConditionType { get; set; }

        public ConditionImport()
        {
            this.Components = new List<ComponentImport>();
        }
    }
}