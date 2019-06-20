using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Exceptions;

namespace IndicatorsManager.WebApi.Models
{
    public enum StringType
    {
        Sql, Text
    }
    public class StringItemModel : ComponentModel
    {
        public string Value { get; set; }
        public StringType Type { get; set; }
        
        public StringItemModel() { }

        public StringItemModel(ItemText text) : base(text)
        {
            this.Value = text.TextValue;
            this.Type = StringType.Text;
        }

        public StringItemModel(ItemQuery query) : base(query)
        {
            this.Value = query.QueryTextValue;
            this.Type = StringType.Sql;
        }

        public override Component ToEntity()
        {
            switch(this.Type)
            {
                case StringType.Sql:
                    return new ItemQuery { Position = this.Position, QueryTextValue = this.Value };
                case StringType.Text:
                    return new ItemText { Position = this.Position, TextValue = this.Value };
                default:
                    throw new ComponentException("type must be Sql or Text.");
            }
        }
    }
    
}