using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.BusinessLogic.Visitors;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic
{
    public class IndicatorLogic : IIndicatorLogic
    {
        private IRepository<Indicator> indicatorRepository;
        private IRepository<Area> areaRepository;
        private IRepository<User> userRepository;
        private IIndicatorQuery indicatorQuery;
        private IQueryRunner queryRunner;
        private ITokenRepository tokenRepository;

        public IndicatorLogic(IRepository<Indicator> indicatorRepository, IRepository<Area> areaRepository, IRepository<User> userRepository,
            IIndicatorQuery indicatorQuery, IQueryRunner queryRunner, ITokenRepository tokenRepository)
        {
            this.indicatorRepository = indicatorRepository;
            this.areaRepository = areaRepository;
            this.userRepository = userRepository;
            this.indicatorQuery = indicatorQuery;
            this.queryRunner = queryRunner;
            this.tokenRepository = tokenRepository;
        }
        
        public Indicator Create(Guid areaId, Indicator indicator)
        {
            if(!this.IsValidIndicator(indicator))
            {
                throw new InvalidEntityException("The Indicator or any part is invalid.");
            }

            Area area = this.areaRepository.Get(areaId);
            if(area == null)
            {
                throw new EntityNotExistException("The Area is invalid.");
            }
            area.Indicators.Add(indicator);
            this.areaRepository.Save();
            return indicator;
        }

        public IndicatorResult Get(Guid indicatorId)
        {
            List<IndicatorItemResult> itemResults = new List<IndicatorItemResult>();
            Indicator toEvalualte = this.indicatorRepository.Get(indicatorId);
            if(toEvalualte == null)
            {
                throw new EntityNotExistException(string.Format("The Indicator of Id {0} doesn't exist.", indicatorId));
            }
            this.queryRunner.SetConnectionString(toEvalualte.Area.DataSource);
            foreach (IndicatorItem item in toEvalualte.IndicatorItems)
            {
                string condAsString = item.Condition.Accept(new VisitorComponentToString(this.queryRunner));
                EvaluateConditionResult result = new EvaluateConditionResult { ConditionToString = condAsString };
                try
                {
                    DataType dataType = item.Condition.Accept(new VisitorComponentEvaluate(this.queryRunner));
                    result.ConditionResult = dataType.GetDataValue();
                }
                catch(EvaluationException ee)
                {
                    result.ConditionResult = ee.Message;
                }
                itemResults.Add(new IndicatorItemResult { IndicatorItem = item, Result = result });
            }
            return new IndicatorResult { Indicator = toEvalualte, ItemsResults = itemResults };
        }

        public IEnumerable<Indicator> GetAll(Guid areaId)
        {
            Area area = this.areaRepository.Get(areaId);
            if(area  == null)
            {
                throw new EntityNotExistException(String.Format("The Area with Id {0} doesn't exist", areaId.ToString()));
            }
            return area.Indicators;
        }

        public IEnumerable<IndicatorConfiguration> GetManagerIndicators(Guid token)
        {
            User authUser = GetUserByToken(token, Role.Manager);
            return this.indicatorQuery.GetManagerIndicators(authUser.Id)
                .Select(i => ConvertToIndicatorConfiguration(i, authUser.Id))
                .OrderByDescending(i => i.Position.HasValue).ThenBy(i => i.Position);
        }

        public IEnumerable<ActiveIndicator> GetManagerActiveIndicators(Guid token)
        {
            User authUser = GetUserByToken(token, Role.Manager);
            List<ActiveIndicator> result = new List<ActiveIndicator>();
            IEnumerable<Indicator> indicators = this.indicatorQuery.GetManagerIndicators(authUser.Id)
                .Where(i => i.UserIndicators.Count(u => u.UserId == authUser.Id) == 0 || 
                i.UserIndicators.Any(u => u.UserId == authUser.Id && u.IsVisible));
            foreach (Indicator indicator in indicators)
            {
                this.queryRunner.SetConnectionString(indicator.Area.DataSource);
                List<IndicatorItem> turnOn = new List<IndicatorItem>();
                foreach (IndicatorItem item in indicator.IndicatorItems)
                {
                    try
                    {
                        DataType dataResult = item.Condition.Accept(new VisitorComponentEvaluate(this.queryRunner));
                        if(dataResult.GetType() == typeof(BooleanDataType) && ((BooleanDataType)dataResult).BooleanValue)
                        {
                            turnOn.Add(item);
                        }
                    }
                    catch(EvaluationException) { }
                }
                if(turnOn.Count > 0)
                {
                    result.Add(ConvertToActiveIndicator(indicator, authUser.Id, turnOn));
                }
            }
            return result.OrderByDescending(i => i.Position.HasValue).ThenBy(i => i.Position);
        }

        public void Remove(Guid id)
        {
            Indicator toDelete = this.indicatorRepository.Get(id);
            if(toDelete != null)
            {
                this.indicatorRepository.Remove(toDelete);
                this.indicatorRepository.Save();
            }
        }

        public Indicator Update(Guid id, Indicator indicator)
        {
             if(indicator == null || !this.ValidName(indicator.Name))
             {
                 throw new InvalidEntityException("The indicator is invalid");
             }
             Indicator original = this.indicatorRepository.Get(id);
             if(original == null)
             {
                 throw new EntityNotExistException(String.Format("The indicator with Id {0} doesn't exist.", id.ToString()));
             }

             original.Update(indicator);
             this.indicatorRepository.Update(original);
             this.indicatorRepository.Save();
             return original;
        }

        public void AddIndicatorConfiguration(IEnumerable<UserIndicator> userIndicators, Guid token)
        {
            User user = GetUserByToken(token, Role.Manager);
            CheckIfAnyIndicatorDoesNotExist(userIndicators);
            foreach (UserIndicator config in userIndicators)
            {
                Indicator indicator = this.indicatorRepository.Get(config.IndicatorId);
                UserIndicator foundConfig = indicator.UserIndicators.FirstOrDefault(ui => ui.UserId == user.Id);
                if(foundConfig == null)
                {
                    config.User = user;
                    indicator.UserIndicators.Add(config);
                }
                else
                {
                    foundConfig.Update(config);
                }
            }
            indicatorRepository.Save();
        }

        private bool IsValidIndicator(Indicator indicator)
        {
            return this.ValidName(indicator.Name) && indicator.IndicatorItems.Count <= 3 && indicator.IndicatorItems.All(ii => ValidName(ii.Name) && 
               indicator.IndicatorItems.GroupBy(c => c.Name).All(c => c.Count() == 1) && ii.Condition != null && 
               ii.Condition.Accept(new VisitorComponentValidation()));
        }

        private bool ValidName(string name)
        {
            return !String.IsNullOrEmpty(name) && name.Trim() != "";
        }

        private User GetUserByToken(Guid authToken, Role aRole)
        {
            AuthenticationToken token = this.tokenRepository.GetByToken(authToken);
            if(token == null || token.User == null || token.User.IsDeleted)
            {
                throw new UnauthorizedException("The token is invalid.");
            }
            if(token.User.Role != aRole)
            {
                throw new UnauthorizedException(string.Format("The user isn't {0}", aRole.ToString()));
            }
            return token.User;
        }

        private IndicatorConfiguration ConvertToIndicatorConfiguration(Indicator indicator, Guid userId)
        {
            UserIndicator userIndicator = indicator.UserIndicators.FirstOrDefault(ui => ui.UserId == userId);
            int? position = null;
            string alias = null;
            if(userIndicator != null)
            {
                position = userIndicator.Position;
                alias = userIndicator.Alias;
            }
            return new IndicatorConfiguration
            {
                Indicator = indicator,
                IsVisible = userIndicator == null || userIndicator.IsVisible,
                Position = position,
                Alias = alias
            };
        }

        private ActiveIndicator ConvertToActiveIndicator(Indicator indicator, Guid userId, IEnumerable<IndicatorItem> activeItems)
        {
            IndicatorConfiguration config = ConvertToIndicatorConfiguration(indicator, userId);
            return new ActiveIndicator
            {
                Indicator = indicator,
                Position = config.Position,
                IsVisible = config.IsVisible,
                Alias = config.Alias,
                ActiveItems = activeItems
            };
        }

        private void CheckIfAnyIndicatorDoesNotExist(IEnumerable<UserIndicator> configurations)
        {
            IEnumerable<Indicator> allIndicators = this.indicatorRepository.GetAll();
            foreach (UserIndicator config in configurations)
            {
                if(!allIndicators.Any(i => i.Id == config.IndicatorId))
                {
                    throw new EntityNotExistException("One or many Indicator of the list don't exist.");
                }
            }
        }
    }
}