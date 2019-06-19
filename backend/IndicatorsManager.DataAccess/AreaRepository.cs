using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using System.Data.SqlClient;
using IndicatorsManager.DataAccess.Visitors;

namespace IndicatorsManager.DataAccess
{
    public class AreaRepository : BaseRepository<Area>, IAreaQuery
    {
        public AreaRepository(DbContext context) : base(context) { }

        public override Area Get(Guid id)
        {
            try
            {
                return context.Set<Area>().Where(x => x.Id == id).FirstOrDefault();
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

        public override IEnumerable<Area> GetAll()
        {
            try
            {
                return context.Set<Area>().ToList();
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

        public Area GetByName(string name)
        {
            try
            {
                return this.context.Set<Area>().Where(a => a.Name == name).FirstOrDefault();
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

        public override void Remove(Area area)
        {
            try
            {
                if (area == null)
                {
                    throw new DataAccessException("El indicador es null.");
                }
                area.Indicators.ForEach(i => RemoveIndicator(i));
                this.context.Set<Area>().Remove(area);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

        private void RemoveIndicator(Indicator indicator)
        {
            foreach (IndicatorItem items in indicator.IndicatorItems)
            {
                this.context.Set<Component>().RemoveRange(items.Condition.Accept(
                    new VisitorComponentToList()));
            }
            this.context.Set<Indicator>().Remove(indicator);
        }

    }
}