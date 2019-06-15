using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using IndicatorsManager.IndicatorImporter.Interface;
using IndicatorsManager.IndicatorImporter.Interface.Exceptions;

namespace IndicatorsManager.IndicatorImporter.Json
{
    public class IndicatorImporterJson : IIndicatorImporter
    {
        public string GetName()
        {
            return "Json Importer";
        }

        public Dictionary<string, string> GetParameters()
        {
            return new Dictionary<string, string>{ { "filePath", "<pathName>" } };
        }

        public IEnumerable<IndicatorImport> ImportIndicators(Dictionary<string, string> parameters)
        {
            string path = GetFilePath(parameters);
            try
            {
                string jsonString = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<IEnumerable<IndicatorImport>>(jsonString,
                    new ComponentJsonParser());
            }
            catch(FileNotFoundException fe)
            {
                throw new IncorrectParameterException("The file path is incorrect.", fe);
            }
            catch(JsonSerializationException je)
            {
                throw new ImporterException("The json format is incorrect.", je);
            }
        }

        private string GetFilePath(Dictionary<string, string> parameters)
        {
            string result;
            bool pathPresent = parameters.TryGetValue("filePath", out result);
            if(!pathPresent)
            {
                throw new IncorrectParameterException("filePath parameter is missing.");
            }
            if(string.IsNullOrEmpty(result) || result.Trim() == "")
            {
                throw new IncorrectParameterException("filePath parameter is an empty string.");
            }
            return result;
        }
    }
}