using System;
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Visitors;

namespace IndicatorsManager.WebApi.Models
{
    public class IndicatorItemGetModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ComponentModel Condition { get; set; }

        public IndicatorItemGetModel() { }

        public IndicatorItemGetModel(IndicatorItem item) 
        {
            this.Id = item.Id;
            this.Name = item.Name;
            this.Condition = item.Condition.Accept(new ComponentModelVisitor());
        }
    }
}