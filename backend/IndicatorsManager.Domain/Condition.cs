using System.Collections.Generic;

namespace IndicatorsManager.Domain
{
    public abstract class Condition : Component
    {
        public virtual List<Component> Components { get; set; }

        public Condition()
        {
            this.Components = new List<Component>();
         }

    }

}