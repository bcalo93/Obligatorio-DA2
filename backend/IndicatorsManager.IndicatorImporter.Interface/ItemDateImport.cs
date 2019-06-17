using System;
using IndicatorsManager.IndicatorImporter.Interface.Visitor;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public class ItemDateImport : ComponentImport
    {
        public DateTime Date { get; set; }

        public override T Accept<T>(IConditionImportVisitor<T> visitor)
        {
            return visitor.VisitItemDateImport(this);
        }
    }
}