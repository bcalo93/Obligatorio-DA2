using System;

namespace IndicatorsManager.Domain
{
    public class UserIndicator
    {
        public Guid IndicatorId { get; set; }
        public virtual Indicator Indicator { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string Alias { get; set; }
        public int Position { get; set; }
        public bool IsVisible { get; set; }

        public UserIndicator() { }

        public UserIndicator(User user, Indicator indicator) 
        {
            this.UserId = user.Id;
            this.User = user;
            this.IndicatorId = indicator.Id;
            this.Indicator = indicator;
        }
    }
    
}