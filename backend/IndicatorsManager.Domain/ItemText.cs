using System;
using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.Domain
{
    public class ItemText : Component
    {
        public string TextValue { get; set; }

        public ItemText() { }

        public override T Accept<T>(IVisitorComponent<T> visitor)
        {
            return visitor.VisitItemText(this);
        }
    }
}