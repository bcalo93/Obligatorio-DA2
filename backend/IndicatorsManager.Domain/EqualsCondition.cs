using System;
using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.Domain
{
    public class EqualsCondition : Condition
    {
        public EqualsCondition() { }

        public override T Accept<T>(IVisitorComponent<T> visitor)
        {
            return visitor.VisitEqualsCondition(this);
        }
    }
}