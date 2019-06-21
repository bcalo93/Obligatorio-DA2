using System;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsManager.Domain
{
    public class Area
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string DataSource { get; set; }

        public virtual List<UserArea> UserAreas { get; set; }
        public virtual List<Indicator> Indicators { get; set; }

        public Area()
        {
            UserAreas = new List<UserArea>();
            Indicators = new List<Indicator>();
        }

        public Area Update(Area area)
        {
            Name = area.Name;
            DataSource = area.DataSource;
            return this;
        }

        public void AddUser(UserArea userArea)
        {
            this.UserAreas.Add(userArea);
        }

        public void RemoveUser(UserArea userArea)
        {
            UserArea ua = UserAreas.Where(u => u.AreaId == userArea.AreaId && u.UserId == userArea.UserId).FirstOrDefault();
            this.UserAreas.Remove(ua);
        }

        public bool IsValid() 
        {
            bool isEmpty = String.IsNullOrEmpty(this.Name) || this.Name.Trim() == "" || String.IsNullOrEmpty(this.DataSource)
                 || this.DataSource.Trim() == "";
            return !isEmpty;
        }
    }    
}