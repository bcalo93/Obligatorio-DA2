using System;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class IndicatorConfigPersistModel
    {
        public Guid IndicatorId { get; set; }
        public int Position { get; set; }
        public bool IsVisible { get; set; }
        public string Alias { get; set; }

        public IndicatorConfigPersistModel() { }

        public UserIndicator ToEntity() => new UserIndicator
        {
            IndicatorId = this.IndicatorId,
            Position = this.Position,
            IsVisible = this.IsVisible,
            Alias = this.Alias
        };
    }
}