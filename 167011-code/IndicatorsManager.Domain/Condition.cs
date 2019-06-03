using System;
using System.Collections.Generic;

namespace IndicatorsManager.Domain
{
    public class Condition
    {
        public Guid Id { get; set; }
        public string Colour { get; set; }
        public virtual Expression Expression { get; set; }
        public virtual Indicator Indicator { get; set; }

        public Condition Update(Condition entity)
        {
            if (entity.Colour != null) 
                Colour = entity.Colour;
            if (entity.Expression != null)
                Expression = entity.Expression;
            return this;
        }

        public bool IsActive()
        {
            return (bool)Expression.Evaluate();
        }
    }
}