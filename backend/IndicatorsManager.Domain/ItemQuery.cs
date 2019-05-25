using System;
using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.Domain
{
    public class ItemQuery : Component
    {
        public string QueryTextValue { get; set; }

        public ItemQuery() { }

        public override T Accept<T>(IVisitorComponent<T> visitor)
        {
            return visitor.VisitItemQuery(this);
        }
    }
}