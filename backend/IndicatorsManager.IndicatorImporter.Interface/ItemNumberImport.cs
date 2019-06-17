using IndicatorsManager.IndicatorImporter.Interface.Visitor;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public class ItemNumberImport : ComponentImport
    {
        public int Number { get; set; }

        public override T Accept<T>(IConditionImportVisitor<T> visitor)
        {
            return visitor.VisitItemNumberImport(this);
        }
    }
}