using System;
using System.Collections.Generic;
using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public interface IReportLogic
    { 
       IEnumerable<User> GetMostLoggedInManagers(int limit);

       IEnumerable<Indicator> GetMostHiddenIndicators(int limit);

    }
}