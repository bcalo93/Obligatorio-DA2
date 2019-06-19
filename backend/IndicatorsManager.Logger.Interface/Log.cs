using System;

namespace IndicatorsManager.Logger.Interface
{
    public class Log
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string LogType { get; set; }
        public DateTime LogDate { get; set; }

        public Log() { }
    }
}