using System;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class BooleanItemModel : ComponentModel
    {
        public bool BooleanValue { get; set; }
        
        public BooleanItemModel() { }
        public BooleanItemModel(ItemBoolean boolean) : base(boolean)
        {
            this.BooleanValue = boolean.Boolean;
        }
        public override Component ToEntity() => new ItemBoolean
        {
           Position = this.Position,
           Boolean = this.BooleanValue
        };
    }

}