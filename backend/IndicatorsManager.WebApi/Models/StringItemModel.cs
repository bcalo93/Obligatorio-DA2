using System;
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Exceptions;

namespace IndicatorsManager.WebApi.Models
{
    public class StringItemModel : ComponentModel
    {
        public string Value { get; set; }
        public string Type { get; set; }
        
        public StringItemModel() { }

        public StringItemModel(ItemText text) : base(text)
        {
            this.Value = text.TextValue;
            this.Type = "Text";
        }

        public StringItemModel(ItemQuery query) : base(query)
        {
            this.Value = query.QueryTextValue;
            this.Type = "Sql";
        }

        public override Component ToEntity()
        {
            switch(this.Type)
            {
                case "Sql":
                    return new ItemQuery { Position = this.Position, QueryTextValue = this.Value };
                case "Text":
                    return new ItemText { Position = this.Position, TextValue = this.Value };
                default:
                    throw new ComponentException("type debe ser Sql o Text.");
            }
        }
    }
    
}