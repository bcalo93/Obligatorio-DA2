using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Visitors;

namespace IndicatorsManager.WebApi.Models
{
    public class IndicatorItemPersistModel
    {
        public string Name { get; set; }
        public ComponentModel Condition { get; set; }

        public IndicatorItemPersistModel() { }

        public IndicatorItemPersistModel(IndicatorItem item)
        {
            this.Name = item.Name;
            this.Condition = item.Condition.Accept(new ComponentModelVisitor());
        }

        public IndicatorItem ToEntity() => new IndicatorItem
        {
            Name = this.Name,
            Condition = this.Condition.ToEntity()
        };
        
    }
    
}