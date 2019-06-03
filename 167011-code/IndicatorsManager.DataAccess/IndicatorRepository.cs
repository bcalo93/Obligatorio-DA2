using System;
using System.Collections.Generic;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using System.Linq;
using System.Linq.Expressions;

namespace IndicatorsManager.DataAccess
{
    public class IndicatorRepository : Repository<Indicator>
    {
        public IndicatorRepository(DbContext Context) : base(Context) { }

        public override Indicator Get(Guid id)
        {
            return Context.Set<Indicator>().FirstOrDefault(x => x.Id == id);
        }

        public override ICollection<Indicator> GetAll()
        {
            return Context.Set<Indicator>().ToList();
        }
        public List<Indicator> GetActiveVisibleUserAlerts(Guid userId)
        {
            List<Indicator> indicators = Context.Set<Indicator>().ToList();
            List<Indicator> filteredResult = new List<Indicator>();
            foreach (var indicator in indicators)
            {
                if(IsManagerActiveVisibleIndicators(userId, indicator.Id, indicator))
                    filteredResult.Add(indicator);
            }
            List<Indicator> orderedResult = filteredResult.OrderByDescending(x => GetPosition(userId, x.Id, x)).ToList();
            return orderedResult;
        }

        private bool IsManagerActiveVisibleIndicators(Guid userId, Guid indicatorId, Indicator indicator)
        {
            bool isManager = IsManager(userId, indicator);
            bool isActive = IsActive(userId, indicator.Id, indicator);
            bool hasConfig = IndicatorHasConfig(userId, indicator.Id, indicator);
            bool isVisible = true;
            if(hasConfig)
             isVisible = IsVisible(userId, indicator.Id, indicator);
            return isManager && isActive && isVisible;
        }

        public bool IsActive(Guid userId, Guid indicatorId, Indicator indicator)
        {
           return indicator.IsAnyActive();
        }
        private bool IsVisible(Guid userId, Guid indicatorId, Indicator indicator)
        {
            return GetUserIndicator(userId, indicatorId, indicator).IsVisible;
        }
        private int GetPosition(Guid userId, Guid indicatorId, Indicator indicator)
        {
            if(!IndicatorHasConfig(userId, indicator.Id, indicator)) return int.MaxValue;
            else return GetUserIndicator(userId, indicatorId, indicator).Position;
        }

        public bool IndicatorHasConfig(Guid userId, Guid indicatorId, Indicator indicator)
        {
            return GetUserIndicator(userId, indicatorId, indicator) != null;
        }
        private UserIndicator GetUserIndicator(Guid userId, Guid indicatorId, Indicator indicator)
        {
            UserIndicator config = indicator.UserConfigurations.Find(item => item.IndicatorId == indicatorId && item.UserId == userId);
            return config;
        }

        public List<Indicator> GetUserIndicators(Guid userId)
        {
            List<Indicator> indicators = Context.Set<Indicator>().ToList();
            List<Indicator> filteredResult = new List<Indicator>();
            foreach (var indicator in indicators)
            {
                if(IsManager(userId, indicator))
                    filteredResult.Add(indicator);
            }
            List<Indicator> orderedResult = filteredResult.OrderByDescending(x => GetPosition(userId, x.Id, x)).ToList();
            return orderedResult;
        }

        private bool IsManager(Guid userId, Indicator indicator)
        {
            return indicator.Area.Managers.Exists(m => m.UserId == userId);
        }

    }
}