using System;
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Parsers;
using Newtonsoft.Json;

namespace IndicatorsManager.WebApi.Models
{
    [JsonConverter(typeof(ComponentModelJsonConverter))]
    public abstract class ComponentModel
    {
        public int Position { get; set; }

        public ComponentModel() { }

        public ComponentModel(Component component)
        {
            this.Position = component.Position;
        }

        public ComponentModel(int position)
        {
            this.Position = position;
        }

        public abstract Component ToEntity();
    }
}