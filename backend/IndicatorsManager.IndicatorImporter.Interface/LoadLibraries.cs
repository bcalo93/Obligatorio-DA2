using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace IndicatorsManager.IndicatorImporter.Interface
{
    public static class LoadLibraries
    {
        public static IEnumerable<IIndicatorImporter> LoadImporters()
        {
            List<IIndicatorImporter> result = new List<IIndicatorImporter>();
            var dir = new DirectoryInfo(@".\importers");
            foreach (var fileInfo in dir.GetFiles())
            {
                Assembly currentAssembly = Assembly.LoadFile(fileInfo.FullName);
                result.AddRange(GetIIndicatorImporterInstances(currentAssembly));
            }
            return result;
        }

        private static IEnumerable<IIndicatorImporter> GetIIndicatorImporterInstances(Assembly assembly)
        {
            List<IIndicatorImporter> instances = new List<IIndicatorImporter>();
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IIndicatorImporter).IsAssignableFrom(type))
                    instances.Add((IIndicatorImporter)Activator.CreateInstance(type));
            }
            return instances;
        }
    }
}