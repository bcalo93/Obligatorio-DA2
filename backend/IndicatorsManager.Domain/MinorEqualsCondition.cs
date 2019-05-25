using System;
using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.Domain
{
    public class MinorEqualsCondition : Condition
    {
        public MinorEqualsCondition() { }

        public override T Accept<T>(IVisitorComponent<T> visitor)
        {
            return visitor.VisitMinorEqualsCondition(this);
        }
    }
    
}