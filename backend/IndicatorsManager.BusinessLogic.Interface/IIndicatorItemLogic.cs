using System;
using System.Collections.Generic;
using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public interface IIndicatorItemLogic
    {
        IndicatorItem Create(Guid inidcatorId, IndicatorItem item);
        IndicatorItem Update(Guid id, IndicatorItem item);
        void Remove(Guid id);
        IndicatorItemResult Get(Guid id);   
    }
}