using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class IndicatorConfigurationModel : Model<UserIndicator, IndicatorConfigurationModel>
    {  
        public Guid IndicatorId { get; set; }
        public bool IsVisible { get; set; }
        public int Position { get; set; }


        public IndicatorConfigurationModel()
        {
        }

        public IndicatorConfigurationModel(UserIndicator entity)
        {
            SetModel(entity);
        }


        public override UserIndicator ToEntity() => new UserIndicator()
        {
            IndicatorId = this.IndicatorId,
            IsVisible = this.IsVisible,
            Position = this.Position
        };

        protected override IndicatorConfigurationModel SetModel(UserIndicator entity)
        {
            IndicatorId = entity.IndicatorId;
            IsVisible = entity.IsVisible;
            Position = entity.Position;
            return this;
        }
    }
}