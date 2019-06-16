using IndicatorsManager.IndicatorImporter.Interface.Visitor;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public class ItemTextImport : ComponentImport
    {
        public string Text { get; set; }

        public override T Accept<T>(IConditionImportVisitor<T> visitor)
        {
            return visitor.VisitItemTextImport(this);
        }
    }
}