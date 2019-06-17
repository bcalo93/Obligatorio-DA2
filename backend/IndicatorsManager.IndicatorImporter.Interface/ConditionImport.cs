using System.Collections.Generic;
using IndicatorsManager.IndicatorImporter.Interface.Visitor;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public enum ConditionType
    {
        And, Or, Equals, Minor, MinorEquals, Greater, GreaterEquals
    }
    public class ConditionImport : ComponentImport
    {
        public List<ComponentImport> Components { get; set; }
        public ConditionType ConditionType { get; set; }

        public ConditionImport()
        {
            this.Components = new List<ComponentImport>();
        }

        public override T Accept<T>(IConditionImportVisitor<T> visitor)
        {
            return visitor.VisitConditionImport(this);
        }
    }
}