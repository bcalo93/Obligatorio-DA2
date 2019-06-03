using System;
using System.Collections.Generic;

namespace IndicatorsManager.Domain
{
    public class UserArea
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid AreaId { get; set; }
        public virtual Area Area { get; set; }

        public UserArea(){}

        public UserArea(User user, Area area)
        {
            UserId = user.Id;
            AreaId = area.Id;
            User = user;
            Area = area;
        }
    }
}