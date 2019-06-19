using System;
using IndicatorsManager.Logger.Interface;

namespace IndicatorsManager.WebApi.Models
{
    public class LogModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime LogDate { get; set; }
        public string LogType { get; set; }

        public LogModel() { }

        public LogModel(Log log)
        {
            this.Id = log.Id;
            this.Username = log.Username;
            this.LogDate = log.LogDate;
            this.LogType = log.LogType;
        }
    }
}