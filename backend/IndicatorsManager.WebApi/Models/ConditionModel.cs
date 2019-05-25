using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Exceptions;

namespace IndicatorsManager.WebApi.Models
{
    public class ConditionModel : ComponentModel
    {
        public IEnumerable<ComponentModel> Components { get; set; }
        public string ConditionType { get; set; }
        
        public ConditionModel() 
        {
            this.Components = new List<ComponentModel>();
        }

        public ConditionModel(string conditionType, int position) : base(position)
        {
            this.Components = new List<ComponentModel>();
            this.ConditionType = conditionType;
        }

        public override Component ToEntity()
        {
            Condition result;
            switch (this.ConditionType)
            {
                case "And":
                    result = new AndCondition();
                    break;
                case "Or":
                    result = new OrCondition();
                    break;
                case "Equals":
                    result = new EqualsCondition();
                    break;
                case "Mayor":
                    result = new MayorCondition();
                    break;
                case "MayorEquals":
                    result = new MayorEqualsCondition();
                    break;
                case "Minor":
                    result = new MinorCondition();
                    break;
                case "MinorEquals":
                    result = new MinorEqualsCondition();
                    break;
                default:
                    throw new ComponentException("conditionType debe ser And, Or, Equals, Mayor, MayorEquals, Minor o MinorEquals.");
            }
            result.Position = this.Position;
            result.Components = this.Components.Select(c => c.ToEntity()).ToList();
            return result;
        }
    }
}