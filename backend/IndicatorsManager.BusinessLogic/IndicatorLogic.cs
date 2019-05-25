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
        private  IQueryRunner queryRunner;

        public IndicatorLogic(IRepository<Indicator> indicatorRepository, IRepository<Area> areaRepository, IRepository<User> userRepository,
            IIndicatorQuery indicatorQuery, IQueryRunner queryRunner)
        {
            this.indicatorRepository = indicatorRepository;
            this.areaRepository = areaRepository;
            this.userRepository = userRepository;
            this.indicatorQuery = indicatorQuery;
            this.queryRunner = queryRunner;
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
                EvaluateConditionResult result;
                try
                {
                    result = item.Condition.Accept(new VisitorComponentEvaluate(this.queryRunner));
                }
                catch(EvaluationException ee)
                {
                    result = new EvaluateConditionResult { ConditionToString = ee.Message };
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

        public IEnumerable<Indicator> GetManagerIndicators(Guid userId)
        {
            return this.indicatorQuery.GetManagerIndicators(userId);
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
                ii.Condition != null && ii.Condition.Accept(new VisitorComponentValidation()));
        }

        private bool ValidName(string name)
        {
            return !String.IsNullOrEmpty(name) && name.Trim() != "";
        }
    }
}