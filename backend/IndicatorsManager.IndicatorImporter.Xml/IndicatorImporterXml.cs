using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using IndicatorsManager.IndicatorImporter.Interface;
using IndicatorsManager.IndicatorImporter.Interface.Exceptions;

namespace IndicatorsManager.IndicatorImporter.Xml
{
    public class IndicatorImporterXml : IIndicatorImporter
    {
        public string GetName()
        {
            return "Xml Importer";
        }

        public IEnumerable<Parameter> GetParameters()
        {
            return new List<Parameter> { new Parameter { Name = "Path", Type = "Text", KeyName = "filePath" } };
        }

        public IEnumerable<IndicatorImport> ImportIndicators(Dictionary<string, string> parameters)
        {
            string filePath = GetFilePath(parameters);
            try
            {
                XDocument document = XDocument.Load(filePath);
                return document.Element("indicators").Elements("indicator").Select(i => XElementToIndicator(i));

            }
            catch (FileNotFoundException fe)
            {
                throw new IncorrectParameterException("The file path is incorrect.", fe);
            }
            catch(NullReferenceException ne)
            {
                throw new ImporterException("The xml format is incorrect.", ne);
            }
        }

        private IndicatorImport XElementToIndicator(XElement element)
        {
            try
            {
                return new IndicatorImport
                {
                    Name = element.Element("name").Value,
                    Items = element.Element("items").Elements("item").Select(ii => XElementToIndicatorItem(ii)).ToList()

                };
            }
            catch (NullReferenceException)
            {
                return new IndicatorImport();
            }
        }

        private IndicatorItemImport XElementToIndicatorItem(XElement element)
        {
            try
            {
                return new IndicatorItemImport
                {
                    Name = element.Element("name").Value,
                    Condition = XElementToComponentImport(element.Element("condition"))
                };
            }
            catch (NullReferenceException)
            {
                return new IndicatorItemImport();
            }
        }

        private ComponentImport XElementToComponentImport(XElement element)
        {
            try
            {
                if (element.Element("query") != null)
                {
                    return ConvertToQueryImport(element);
                }
                if (element.Element("number") != null)
                {
                    return ConvertToNumberImport(element);
                }
                if(element.Element("boolean") != null)
                {
                    return ConvertToBooleanImport(element);
                }
                if(element.Element("date") != null)
                {
                    return ConvertToDateImport(element);
                }
                if(element.Element("text") != null)
                {
                    return ConvertToTextImport(element);
                }
                if (element.Element("conditionType") != null)
                {
                    return ConvertToConditionImport(element);
                }
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is FormatException)
                {
                    return null;
                }
            }
            return null;
        }

        private ConditionImport ConvertToConditionImport(XElement element)
        {
            return new ConditionImport
            {
                ConditionType = (ConditionType)Enum.Parse(typeof(ConditionType), element.Element("conditionType").Value),
                Position = Int32.Parse(element.Element("position").Value),
                Components = element.Element("components").Elements("component").Select(c => XElementToComponentImport(c)).ToList()
            };
        }

        private ItemNumberImport ConvertToNumberImport(XElement element)
        {
            return new ItemNumberImport
            {
                Position = Int32.Parse(element.Element("position").Value),
                Number = Int32.Parse(element.Element("number").Value)
            };
        }

        private ItemQueryImport ConvertToQueryImport(XElement element)
        {
            return new ItemQueryImport
            {
                Position = Int32.Parse(element.Element("position").Value),
                Query = element.Element("query").Value
            };
        }

        private ItemBooleanImport ConvertToBooleanImport(XElement element)
        {
            return new ItemBooleanImport
            {
                Position = Int32.Parse(element.Element("position").Value),
                Boolean = element.Element("boolean").Value == "true" ? true : false
            };
        }

        private ItemDateImport ConvertToDateImport(XElement element)
        {
            return new ItemDateImport
            {
                Position = Int32.Parse(element.Element("position").Value),
                Date = DateTime.Parse(element.Element("date").Value)
            };
        }

        private ItemTextImport ConvertToTextImport(XElement element)
        {
            return new ItemTextImport
            {
                Position = Int32.Parse(element.Element("position").Value),
                Text = element.Element("text").Value
            };
        }

        private string GetFilePath(Dictionary<string, string> parameters)
        {
            string result;
            bool pathPresent = parameters.TryGetValue("filePath", out result);
            if (!pathPresent)
            {
                throw new IncorrectParameterException("filePath parameter is missing.");
            }
            if (string.IsNullOrEmpty(result) || result.Trim() == "")
            {
                throw new IncorrectParameterException("filePath parameter is an empty string.");
            }
            return result;
        }
    }
}
