using System;
using System.Collections.Generic;

namespace IndicatorsManager.Domain
{
    public class Indicator
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual List<Condition> Conditions { get; set; }
        public virtual Area Area { get; set; }
        public virtual List<UserIndicator> UserConfigurations { get; set; }

        public Indicator()
        {
            Conditions = new List<Condition>();
            UserConfigurations = new  List<UserIndicator>();
        }

        public Indicator Update(Indicator entity)
        {
            if (entity.Name != null) 
                Name = entity.Name;
            if (entity.Conditions != null)
                Conditions = entity.Conditions;
            return this;
        }

        public List<string> GetActiveColours()
        {   
            var activeColours = new List<string>();
            if(Conditions.Count != 0)
            {
                foreach (var condition in Conditions)
                {
                    if(condition.IsActive())
                    {
                        activeColours.Add(condition.Colour);
                    }
                }
            }
            return activeColours;
        }

        public bool IsAnyActive()
        {
            return Conditions.Exists(x => x.IsActive());
        }
    }
}