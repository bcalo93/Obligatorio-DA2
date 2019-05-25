using System;

namespace IndicatorsManager.Domain
{
    public class UserArea
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid AreaId { get; set; }
        public virtual Area Area { get; set; }

        public UserArea() {}

        public UserArea(User user, Area area) 
        {
            this.UserId = user.Id;
            this.User = user;
            this.AreaId = area.Id;
            this.Area = area;
        }
        
    }
}