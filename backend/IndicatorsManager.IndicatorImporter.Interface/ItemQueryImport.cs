using IndicatorsManager.IndicatorImporter.Interface.Visitor;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public class ItemQueryImport : ComponentImport
    {
        public string Query { get; set; }

        public override T Accept<T>(IConditionImportVisitor<T> visitor)
        {
            return visitor.VisitItemQueryImport(this);
        }
    }
}