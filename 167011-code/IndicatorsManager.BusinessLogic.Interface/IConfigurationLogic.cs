using System;
using System.Collections.Generic;
using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic
{
    public interface IConfigurationLogic
    {
        User AddConfiguration(Guid userId, List<UserIndicator> configurations);
        User UpdateConfiguration(Guid userId, UserIndicator configuration);
        List<Indicator> GetTopHiddenIndicators(int limit = 10);
    }
}