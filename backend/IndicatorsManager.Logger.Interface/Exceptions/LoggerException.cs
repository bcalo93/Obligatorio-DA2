using System;

namespace IndicatorsManager.Logger.Interface.Exceptions
{
    public class LoggerException : Exception
    {
        public LoggerException(string message) : base(message) { }
        public LoggerException(string message, Exception inner) : base(message, inner) { }
    }
}