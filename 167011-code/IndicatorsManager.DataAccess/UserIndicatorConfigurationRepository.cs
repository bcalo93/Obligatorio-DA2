using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using System.Linq.Expressions;

namespace IndicatorsManager.DataAccess
{
    public class UserIndicatorConfigurationRepository : IConfigurationRepository
    {
        private DbContext context;
        public UserIndicatorConfigurationRepository(DbContext context) {
            this.context = context;
        }

        public bool ExistCondition(Func<UserIndicator, bool> expression)
        {
            return context.Set<UserIndicator>().Any(expression);
        }

        public List<Indicator> GetTopHiddenIndicators(int limit = 10)
        {
            List<UserIndicator> configurations = context.Set<UserIndicator>().ToList();
            List<UserIndicator> filteredResult = new List<UserIndicator>();
            foreach (var configuration in configurations)
            {
                if(!configuration.IsVisible)
                    filteredResult.Add(configuration);
            }

            List<Indicator> orderedResult = 
                filteredResult
                    .GroupBy(config => config.Indicator)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Key)
                    .ToList();
                    
            return orderedResult;
        }
    }
}