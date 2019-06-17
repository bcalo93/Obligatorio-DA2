using IndicatorsManager.IndicatorImporter.Interface.Visitor;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public class ItemBooleanImport : ComponentImport
    {
        public bool Boolean { get; set; }

        public override T Accept<T>(IConditionImportVisitor<T> visitor)
        {
            return visitor.VisitItemBooleanImport(this);
        }
    }
}