using System;
using System.Collections.Generic;
using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public interface IIndicatorLogic
    {
        Indicator Create(Guid areaId, Indicator indicator);
        void Remove(Guid id);
        Indicator Update(Guid id, Indicator entity);
        IEnumerable<Indicator> GetAll(Guid parentId);
        IEnumerable<IndicatorConfiguration> GetManagerIndicators(Guid token);
        IEnumerable<ActiveIndicator> GetManagerActiveIndicators(Guid token);
        void AddIndicatorConfiguration(IEnumerable<UserIndicator> userIndicators, Guid token);
        IndicatorResult Get(Guid indicatorId);
        
    }
    
}