using System;
using System.Collections.Generic;

namespace IndicatorsManager.Domain
{
    public class UserIndicator
    {
        public bool IsVisible { get; set; }
        public int Position { get; set; }
        public Guid IndicatorId { get; set; }
        public virtual Indicator Indicator { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; } 

        public UserIndicator Update(UserIndicator entity)
        {
            IsVisible = entity.IsVisible;
            Position = entity.Position;
            return this;
        }
    }
}