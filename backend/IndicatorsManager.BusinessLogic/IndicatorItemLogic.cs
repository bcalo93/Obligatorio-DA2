using System;
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
                throw new InvalidEntityException("The item's data is invalid.");
            }
            Indicator indicator = this.indicatorRepository.Get(inidcatorId);
            if(indicator == null)
            {
                throw new EntityNotExistException("The indicator is invalid.");
            }
            if(indicator.IndicatorItems.Any(i => i.Name == item.Name))
            {
                throw new EntityExistException("An item with that name already exist.");
            }
            if(indicator.IndicatorItems.Count >= 3)
            {
                throw new IndicatorException("The Indicator has to many items.");
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
                throw new EntityNotExistException(string.Format("The item with id {0} doesn't exist.", id));
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
                throw new InvalidEntityException("The item is invalid");
            }
            IndicatorItem original = this.itemRepository.Get(id);
            if(original == null)
            {
                throw new EntityNotExistException(String.Format("The item with Id {0} doesn't exist", id.ToString()));
            }
            Indicator indicator = original.Indicator;
            if(indicator.IndicatorItems.Any(i => i.Name == item.Name && i.Id != original.Id))
            {
                throw new EntityExistException("There's other item with that name.");
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