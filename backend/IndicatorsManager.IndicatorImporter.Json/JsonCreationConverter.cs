using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IndicatorsManager.IndicatorImporter.Json
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        public override bool CanWrite { get => false; }

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader == null)
            {
                throw new ArgumentNullException("Reader null");
            }
            if(serializer == null)
            {
                throw new ArgumentNullException("Serializer null");
            }
            if(reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            JObject jObject = JObject.Load(reader);
            T target = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("CanWrite is always false. This method won't be called.");
        }

        protected abstract T Create(Type objectType, JObject jObject);
    }
}