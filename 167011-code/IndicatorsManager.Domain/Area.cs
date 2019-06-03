using System;
using System.Collections.Generic;

namespace IndicatorsManager.Domain
{
    public class Area
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public virtual List<UserArea> Managers { get; set; }
        public virtual List<Indicator> Indicators { get; set; }

        public Area()
        {
            Indicators = new List<Indicator>();
            Managers = new List<UserArea>();
        }

        public Area Update(Area entity)
        {
            if (!String.IsNullOrEmpty(entity.Name))
                Name = entity.Name;
            if (!String.IsNullOrEmpty(entity.ConnectionString))
                ConnectionString = entity.ConnectionString;
            if (entity.Managers != null)
                Managers = entity.Managers;
            if (entity.Indicators != null)
                Indicators = entity.Indicators;
            return this;
        }

    }
}