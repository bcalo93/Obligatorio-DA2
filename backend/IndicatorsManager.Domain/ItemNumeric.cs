using System;
using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.Domain
{
    public class ItemNumeric : Component
    {
        public int NumberValue { get; set; }

        public ItemNumeric() { }

        public override T Accept<T>(IVisitorComponent<T> visitor)
        {
            return visitor.VisitItemNumeric(this);
        }
    }
}