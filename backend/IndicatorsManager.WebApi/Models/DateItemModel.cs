using System;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class DateItemModel : ComponentModel
    {
        public DateTime DateValue { get; set; }
        
        public DateItemModel() { }
        public DateItemModel(ItemDate date) : base(date)
        {
            this.DateValue = date.Date;
        }

        public override Component ToEntity() => new ItemDate
        {
            Position = this.Position,
            Date = this.DateValue
        };
    }

}