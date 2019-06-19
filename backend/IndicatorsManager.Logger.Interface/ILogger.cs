using System;
using System.Collections.Generic;

namespace IndicatorsManager.Logger.Interface
{
    public interface ILogger
    {
        void LogAction(string username, string actionType);
        IEnumerable<string> GetMostLoggedInUsers();
        IEnumerable<Log> GetLogActions(DateTime start, DateTime end);
    }
}
