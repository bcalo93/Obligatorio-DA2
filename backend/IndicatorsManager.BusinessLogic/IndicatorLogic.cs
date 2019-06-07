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
                throw new InvalidEntityException("El Indicador o alguna de las partes es invalido.");
            }

            Area area = this.areaRepository.Get(areaId);
            if(area == null)
            {
                throw new EntityNotExistException("El Area es invalida.");
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
                throw new EntityNotExistException(string.Format("El Indicator de id {0} no existe.", indicatorId));
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
                throw new EntityNotExistException(String.Format("El area de id {0} no existe", areaId.ToString()));
            }
            return area.Indicators;
        }

        public IEnumerable<Indicator> GetManagerIndicators(Guid token)
        {
            User authUser = GetUserByToken(token);
            return this.indicatorQuery.GetManagerIndicators(authUser.Id);
        }

        public IEnumerable<ActiveIndicator> GetManagerActiveIndicators(Guid token)
        {
            User authUser = GetUserByToken(token);
            List<ActiveIndicator> result = new List<ActiveIndicator>();
            IEnumerable<Indicator> indicators = this.indicatorQuery.GetManagerIndicators(authUser.Id);
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
                    result.Add(new ActiveIndicator { Indicator = indicator, ActiveItems = turnOn });
                }
            }
            return result;
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
                 throw new InvalidEntityException("El indicador es invalido");
             }
             Indicator original = this.indicatorRepository.Get(id);
             if(original == null)
             {
                 throw new EntityNotExistException(String.Format("El indicador de id {0} no existe", id.ToString()));
             }

             original.Update(indicator);
             this.indicatorRepository.Update(original);
             this.indicatorRepository.Save();
             return original;
        }

        public void AddUserIndicator(Guid indicatorId, Guid userId)
        {
            User user = this.userRepository.Get(userId);
            if (user == null || user.IsDeleted || user.Role != Role.Manager)
            {
                throw new InvalidEntityException("El usuario no existe/ no es válido");
            }
            Indicator indicator = this.indicatorRepository.Get(indicatorId);
            if (indicator == null)
            {
                throw new InvalidEntityException("El indicator no existe");
            }
            UserIndicator userIndicator = new UserIndicator(user, indicator);
            indicator.AddUser(userIndicator);
            indicatorRepository.Save();
        }

        public void RemoveUserIndicator(Guid indicatorId, Guid userId)
        {
            User user = this.userRepository.Get(userId);
            Indicator indicator = this.indicatorRepository.Get(indicatorId);

            UserIndicator userIndicator = new UserIndicator(user, indicator);
            indicator.RemoveUser(userIndicator);
            user.RemoveIndicator(userIndicator);
            indicatorRepository.Update(indicator);
            userRepository.Update(user);
            indicatorRepository.Save();
            userRepository.Save();
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

        private User GetUserByToken(Guid authToken)
        {
            AuthenticationToken token = this.tokenRepository.GetByToken(authToken);
            if(token == null || token.User == null)
            {
                throw new UnauthorizedException("El token es invalido.");
            }
            return token.User;
        }
    }
}