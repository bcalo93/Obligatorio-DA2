using IndicatorsManager.IndicatorImporter.Interface.Visitor;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public abstract class ComponentImport
    {
        public int Position { get; set; }

        public abstract T Accept<T>(IConditionImportVisitor<T> visitor);
    }
}