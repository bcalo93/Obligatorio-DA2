using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using IndicatorsManager.DataAccess.Visitors;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;

namespace IndicatorsManager.DataAccess
{
    public class IndicatorRepository : BaseRepository<Indicator>, IIndicatorQuery
    {
        public IndicatorRepository(DbContext context) : base(context) { }
        public override Indicator Get(Guid id)
        {
            return this.context.Set<Indicator>().Where(i => i.Id == id)
                .Include(i => i.Area)
                .Include(i => i.UserIndicators)
                .Include(i => i.IndicatorItems)
                .ThenInclude(ii => ii.Condition)
                .FirstOrDefault();
        }

        public override IEnumerable<Indicator> GetAll()
        {
            return this.context.Set<Indicator>()
                .Include(i => i.Area)
                .Include(i => i.IndicatorItems)
                .Include(i => i.UserIndicators)
                .ThenInclude(ui => ui.User)
                .ToList();
        }

        public override void Remove(Indicator indicator)
        {
            if(indicator == null)
            {
                throw new DataAccessException("El indicador es null.");
            }
            foreach(IndicatorItem items in indicator.IndicatorItems)
            {
                this.context.Set<Component>().RemoveRange(items.Condition.Accept(new VisitorComponentToList()));
            }
            this.context.Set<Indicator>().Remove(indicator);
        }

        public IEnumerable<Indicator> GetManagerIndicators(Guid userId)
        {
            return this.context.Set<Indicator>().Where(i => i.Area.UserAreas.Any(u => u.UserId == userId))
                                                .Include(i => i.UserIndicators)
                                                .ToList();
        }

        public IEnumerable<Indicator> GetMostHiddenIndicators(int limit)
        {
            return this.context.Set<UserIndicator>()
                .Where(ui => !ui.IsVisible)
                .GroupBy(ui => ui.Indicator)
                .OrderByDescending(ui => ui.Count())
                .Take(limit)
                .Select(ui => ui.Key);
        }

    }
}