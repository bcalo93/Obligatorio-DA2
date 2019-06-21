using System;
using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.Domain
{
    public class ItemBoolean : Component
    {
        public bool Boolean { get; set; }

        public ItemBoolean() { }
        
        public override T Accept<T>(IVisitorComponent<T> visitor)
        {
            return visitor.VisitItemBoolean(this);
        }
    }
}