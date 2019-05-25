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
        IEnumerable<Indicator> GetManagerIndicators(Guid userId);
        void AddUserIndicator(Guid indicatorId, Guid userId);
        void RemoveUserIndicator(Guid indicatorId, Guid userId);
        IndicatorResult Get(Guid indicatorId);
        
    }
    
}