using System;
using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.Domain
{
    public class ItemDate : Component
    {
        public DateTime Date { get; set; }

        public ItemDate() { }
        public override T Accept<T>(IVisitorComponent<T> visitor)
        {
            return visitor.VisitItemDate(this);
        }
    }

}