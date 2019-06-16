using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.IndicatorImporter.Interface;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using IndicatorsManager.Domain;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.IndicatorImporter.Interface.Exceptions;
using IndicatorsManager.BusinessLogic.Visitors;

namespace IndicatorsManager.BusinessLogic
{
    public class IndicatorImportLogic : IIndicatorImportLogic
    {
        private IEnumerable<IIndicatorImporter> importers;
        private IRepository<Area> areaRepository;
        
        public IndicatorImportLogic(IRepository<Area> areaRepository)
        {
            this.areaRepository = areaRepository;
            this.importers = LoadLibraries.LoadImporters();
        }

        public IEnumerable<ImporterInfo> GetIndicatorImporters()
        {
            return this.importers.Select(i => new ImporterInfo{ Name = i.GetName(), 
                Parameters = i.GetParameters() });
        }

        public ImportResult ImportIndicators(Guid areaId, string importerName, Dictionary<string, string> parameters)
        {
            ImportResult result = new ImportResult();
            Area area = this.areaRepository.Get(areaId);
            if(area == null)
            {
                throw new EntityNotExistException("The Area is invalid.");
            }
            IIndicatorImporter importer = this.importers.FirstOrDefault(i => i.GetName() == importerName);
            if(importer == null)
            {
                throw new ImportException(string.Format("There's no Importer with {0} name.", importerName));
            }
            try
            {
                IEnumerable<Indicator> indicators = importer.ImportIndicators(parameters)
                                                            .Select(i => ConvertIndicatorImportToIndicator(i));
                result.TotalIndicators = indicators.Count();
                IEnumerable<Indicator> validIndicators = indicators.Where(i => IsValidIndicator(i));
                result.IndicatorsImported = validIndicators.Count();
                area.Indicators.AddRange(validIndicators);
                areaRepository.Save();
            }
            catch(ImporterException ie)
            {
                result.Error = ie.Message;
            }
            catch(DataAccessException de)
            {
                result.Error = de.Message;
            }
            return result;
        }

        private Indicator ConvertIndicatorImportToIndicator(IndicatorImport indicatorImport) => new Indicator
        {
            Name = indicatorImport.Name,
            IndicatorItems = indicatorImport.Items.Select(ii => ConvertItemImportToIndicatorItem(ii)).ToList()
        };

        private IndicatorItem ConvertItemImportToIndicatorItem(IndicatorItemImport itemImport) => new IndicatorItem
        {
            Name = itemImport.Name,
            Condition = itemImport.Condition.Accept(new ConditionImportVisitorToDomain())
        };

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
    }
}