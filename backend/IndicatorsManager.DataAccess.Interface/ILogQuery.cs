using System;
using System.Collections.Generic;
using IndicatorsManager.Domain;

namespace IndicatorsManager.DataAccess.Interface
{
    public interface ILogQuery
    {
        IEnumerable<User> GetUsersMostLogs(int limit);
    }
    
}