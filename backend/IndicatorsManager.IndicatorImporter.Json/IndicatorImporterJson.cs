using System;
using System.Collections.Generic;
using System.IO;
using IndicatorsManager.IndicatorImporter.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IndicatorsManager.IndicatorImporter.Json
{
    public class IndicatorImporterJson : IIndicatorImporter
    {
        public string GetName()
        {
            return "Json Importer";
        }

        public IEnumerable<IndicatorImport> ImportIndicators(string filePath)
        {
            
            string jsonString = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<IEnumerable<IndicatorImport>>(jsonString,
                new ComponentJsonParser());
        }
    }
}