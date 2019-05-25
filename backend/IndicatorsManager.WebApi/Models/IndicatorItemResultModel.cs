using System;
using IndicatorsManager.Domain;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.WebApi.Visitors;

namespace IndicatorsManager.WebApi.Models
{
    public class IndicatorItemResultModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ComponentModel Condition { get; set; }
        public EvaluateConditionResult Result { get; set; }
        public IndicatorItemResultModel(IndicatorItemResult result)
        {
            this.Id = result.IndicatorItem.Id;
            this.Name = result.IndicatorItem.Name;
            this.Condition = result.IndicatorItem.Condition.Accept(new ComponentModelVisitor());
            this.Result = result.Result;
        }
        
    }
    
}