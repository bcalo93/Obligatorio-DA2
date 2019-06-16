using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.IndicatorImporter.Interface;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.Domain;

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
            throw new NotImplementedException();
        }
    }
}