using System;
using System.Collections.Generic;

namespace IndicatorsManager.WebApi.Models
{
    public class ImportModel
    {
        public Guid AreaId { get; set; }
        public string ImporterName { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}