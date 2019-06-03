using System;
using System.Linq;
using System.Collections.Generic;
using IndicatorsManager.Domain;

namespace IndicatorsManager.DataAccess.Interface
{
    public interface IConfigurationRepository
    {     
        List<Indicator> GetTopHiddenIndicators(int limit);

        bool ExistCondition(Func<UserIndicator,bool> expression);
    }
}