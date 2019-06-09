using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Exceptions;

namespace IndicatorsManager.WebApi.Models
{
    public enum ConditionType
    {
        And, Or, Equals, Minor, MinorEquals, Mayor, MayorEquals
    }
    
    public class ConditionModel : ComponentModel
    {
        public IEnumerable<ComponentModel> Components { get; set; }
        public ConditionType ConditionType { get; set; }
        
        public ConditionModel() 
        {
            this.Components = new List<ComponentModel>();
        }

        public ConditionModel(ConditionType conditionType, int position) : base(position)
        {
            this.Components = new List<ComponentModel>();
            this.ConditionType = conditionType;
        }

        public override Component ToEntity()
        {
            Condition result;
            switch (this.ConditionType)
            {
                case ConditionType.And:
                    result = new AndCondition();
                    break;
                case ConditionType.Or:
                    result = new OrCondition();
                    break;
                case ConditionType.Equals:
                    result = new EqualsCondition();
                    break;
                case ConditionType.Mayor:
                    result = new MayorCondition();
                    break;
                case ConditionType.MayorEquals:
                    result = new MayorEqualsCondition();
                    break;
                case ConditionType.Minor:
                    result = new MinorCondition();
                    break;
                case ConditionType.MinorEquals:
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