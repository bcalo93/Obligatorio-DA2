using System;
using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.Domain
{
    public class MayorEqualsCondition : Condition
    {
        public MayorEqualsCondition() { }

        public override T Accept<T>(IVisitorComponent<T> visitor)
        {
            return visitor.VisitMayorEqualsCondition(this);
        }
    }
    
}