using System;
using System.Collections.Generic;
using IndicatorsManager.Domain;

namespace IndicatorsManager.DataAccess.Interface
{
    public interface IIndicatorQuery
    {
        IEnumerable<Indicator> GetManagerIndicators(Guid userId);

        IEnumerable<Indicator> GetMostHiddenIndicators(int limit);
    }
    
}