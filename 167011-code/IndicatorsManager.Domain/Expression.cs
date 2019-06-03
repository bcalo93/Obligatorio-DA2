using System;

namespace IndicatorsManager.Domain
{
    public abstract class Expression
    {
        public Guid Id { get; set; }
        public Guid ConditionId { get; set; }
        public virtual Condition Condition { get; set; }
        public abstract Object Evaluate();

        public override abstract String ToString();
    }
}