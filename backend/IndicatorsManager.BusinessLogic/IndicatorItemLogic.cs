using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.BusinessLogic.Visitors;
using IndicatorsManager.Domain;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;

namespace IndicatorsManager.BusinessLogic
{
    public class IndicatorItemLogic : IIndicatorItemLogic
    {
        private IRepository<IndicatorItem> itemRepository;
        private IRepository<Indicator> indicatorRepository;
        private  IQueryRunner queryRunner;
        
        public IndicatorItemLogic(IRepository<IndicatorItem> itemRepository, 
            IRepository<Indicator> indicatorRepository, IQueryRunner queryRunner)
        {
            this.itemRepository = itemRepository;
            this.indicatorRepository = indicatorRepository;
            this.queryRunner = queryRunner;
        }
        public IndicatorItem Create(Guid inidcatorId, IndicatorItem item)
        {
            if(!this.IsValidItem(item))
            {
                throw new InvalidEntityException("El Item es invalido.");
            }
            Indicator indicator = this.indicatorRepository.Get(inidcatorId);
            if(indicator == null)
            {
                throw new EntityNotExistException("El Indicador es invalida.");
            }
            if(indicator.IndicatorItems.Any(i => i.Name == item.Name))
            {
                throw new EntityExistException("Un item con ese nombre ya existe");
            }
            if(indicator.IndicatorItems.Count >= 3)
            {
                throw new IndicatorException("El indicador tiene muchos Items");
            }
            indicator.IndicatorItems.Add(item);
            this.indicatorRepository.Save();
            return item;
        }

        public IndicatorItemResult Get(Guid id)
        {
            IndicatorItem toEvalualte  = this.itemRepository.Get(id);
            if(toEvalualte == null)
            {
                throw new EntityNotExistException(string.Format("El Item de id {0} no existe.", id));
            }
            this.queryRunner.SetConnectionString(toEvalualte.Indicator.Area.DataSource);
            string resultAsString = toEvalualte.Condition.Accept(new VisitorComponentToString(this.queryRunner));
            EvaluateConditionResult result = new EvaluateConditionResult { ConditionToString = resultAsString };
            try
            {
                DataType dataType = toEvalualte.Condition.Accept(new VisitorComponentEvaluate(this.queryRunner));
                result.ConditionResult = dataType.GetDataValue();
            }
            catch(EvaluationException ee)
            {
                result.ConditionResult = ee.Message;
            }
            return new IndicatorItemResult { IndicatorItem = toEvalualte, Result = result };
        }

        public void Remove(Guid id)
        {
            IndicatorItem item = this.itemRepository.Get(id);
            if(item != null)
            {
                this.itemRepository.Remove(item);
                this.indicatorRepository.Save();
            }
        }

        public IndicatorItem Update(Guid id, IndicatorItem item)
        {
            
            if(!this.IsValidItem(item))
            {
                throw new InvalidEntityException("El item es invalido");
            }
            IndicatorItem original = this.itemRepository.Get(id);
            if(original == null)
            {
                throw new EntityNotExistException(String.Format("El indicador de id {0} no existe", id.ToString()));
            }
            Indicator indicator = original.Indicator;
            if(indicator.IndicatorItems.Any(i => i.Name == item.Name && i.Id != original.Id))
            {
                throw new EntityExistException("Hay otro item con ese nombre.");
            }
            indicator.IndicatorItems.Remove(original);
            indicator.IndicatorItems.Add(item);
            this.itemRepository.Save();
            return item;
        }

        private bool IsValidItem(IndicatorItem item)
        {
            return this.ValidName(item.Name) && item.Condition != null 
                && item.Condition.Accept(new VisitorComponentValidation());
        }

        private bool ValidName(string name)
        {
            return !String.IsNullOrEmpty(name) && name.Trim() != "";
        }
    }

}