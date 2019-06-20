using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic.Visitors
{
    public class BinaryCondition
    {
        public Component LeftCondition { get; set; }
        public Component RightCondition { get; set; }

        public BinaryCondition(Condition condition)
        {
            IEnumerable<Component> ordered = condition.Components.OrderBy(c => c.Position);
            this.LeftCondition = LeftCondition = ordered.ElementAt(0); 
            this.RightCondition = RightCondition = ordered.ElementAt(1);
        }
    }
    
}