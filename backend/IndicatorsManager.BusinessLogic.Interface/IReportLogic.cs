using System;
using System.Collections.Generic;
using IndicatorsManager.Domain;
using IndicatorsManager.Logger.Interface;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public interface IReportLogic
    { 
       IEnumerable<User> GetMostLoggedInManagers(int limit);

       IEnumerable<Indicator> GetMostHiddenIndicators(int limit);
       IEnumerable<Log> GetSystemActivity(DateTime start, DateTime end);

    }
}