using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.Domain;
using IndicatorsManager.Logger.Interface;

namespace IndicatorsManager.BusinessLogic
{
    public class ReportLogic : IReportLogic
    {
        private ILogger logger;
        private IIndicatorQuery indicatorQuery;
        private IRepository<User> userRepository;

        public ReportLogic(ILogger logger, IIndicatorQuery indicatorQuery, 
            IRepository<User> userRepository)
        {
            this.logger = logger;
            this.indicatorQuery = indicatorQuery;
            this.userRepository = userRepository;
        }

        public IEnumerable<User> GetMostLoggedInManagers(int limit)
        {
            List<User> result = new List<User>();
            IEnumerable<string> usernames = this.logger.GetMostLoggedInUsers();
            IEnumerable<User> allUsers = this.userRepository.GetAll();
            foreach(string username in usernames)
            {
                User user = allUsers.FirstOrDefault(u => u.Username == username);
                if(user != null && user.Role == Role.Manager && result.Count < limit)
                {
                    result.Add(user);
                }
                if(result.Count == limit)
                {
                    break;
                }
            }
            return result;
        }

        public IEnumerable<Indicator> GetMostHiddenIndicators(int limit)
        {
            return indicatorQuery.GetMostHiddenIndicators(limit);
        }

    }

}