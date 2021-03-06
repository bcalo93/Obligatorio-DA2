using System;
using Newtonsoft.Json.Linq;
using IndicatorsManager.WebApi.Models;

namespace IndicatorsManager.WebApi.Parsers
{
    public class ComponentModelJsonConverter : JsonCreationConverter<ComponentModel>
    {
        protected override ComponentModel Create(Type objectType, JObject jObject)
        {
            if(jObject == null)
            {
                throw new ArgumentNullException("jObject null");
            }
            if(jObject["conditionType"] != null)
            {
                return new ConditionModel();
            }
            if(jObject["type"] != null)
            {
                return new StringItemModel();
            }
            if(jObject["dateValue"] != null)
            {
                return new DateItemModel();
            }
            if(jObject["booleanValue"] != null)
            {
                return new BooleanItemModel();
            }
            if(jObject["value"] != null)
            {
                return new IntItemModel();
            }
            return null;
        }
    }
}