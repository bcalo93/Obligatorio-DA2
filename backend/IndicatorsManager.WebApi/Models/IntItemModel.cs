using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class IntItemModel : ComponentModel
    {
        public int Value { get; set; }

        public IntItemModel() { }

        public IntItemModel(ItemNumeric numeric) : base(numeric)
        {
            this.Value = numeric.NumberValue;
        }

        public override Component ToEntity() => new ItemNumeric
        {
            Position = this.Position,
            NumberValue = this.Value
        };
    }
    
}