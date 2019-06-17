using System;
using Newtonsoft.Json.Linq;
using IndicatorsManager.IndicatorImporter.Interface;

namespace IndicatorsManager.IndicatorImporter.Json
{
    public class ComponentJsonParser : JsonCreationConverter<ComponentImport>
    {
        protected override ComponentImport Create(Type objectType, JObject jObject)
        {
            if(jObject == null)
            {
                throw new ArgumentNullException("jObject null");
            }
            if(jObject["conditionType"] != null)
            {
                return new ConditionImport();
            }
            if(jObject["text"] != null)
            {
                return new ItemTextImport();
            }
            if(jObject["query"] != null)
            {
                return new ItemQueryImport();
            }
            if(jObject["date"] != null)
            {
                return new ItemDateImport();
            }
            if(jObject["boolean"] != null)
            {
                return new ItemBooleanImport();
            }
            if(jObject["number"] != null)
            {
                return new ItemNumberImport();
            }
            return null;
        }
    }
}