using System;
using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.Domain
{
    public class AndCondition : Condition
    {
        public AndCondition() { }

        public override T Accept<T>(IVisitorComponent<T> visitor)
        {
            return visitor.VisitAndCondition(this);
        }
    }
}