using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.Domain;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.BusinessLogic
{
    public class ReportLogic : IReportLogic
    {
        private ILogQuery logQuery;

        private IIndicatorQuery indicatorQuery;

        public ReportLogic(ILogQuery logQuery, IIndicatorQuery indicatorQuery)
        {
            this.logQuery = logQuery;
            this.indicatorQuery = indicatorQuery;
        }

        public IEnumerable<User> GetUsersMostLogs(int limit)
        {
            return logQuery.GetUsersMostLogs(limit);
        }

        public IEnumerable<Indicator> GetMostHiddenIndicators(int limit)
        {
            return indicatorQuery.GetMostHiddenIndicators(limit);
        }

    }

}