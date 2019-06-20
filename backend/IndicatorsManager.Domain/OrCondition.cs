using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.Domain
{
    public class OrCondition : Condition
    {
        public OrCondition() { }

        public override T Accept<T>(IVisitorComponent<T> visitor)
        {
            return visitor.VisitOrCondition(this);
        }
    }
    
}