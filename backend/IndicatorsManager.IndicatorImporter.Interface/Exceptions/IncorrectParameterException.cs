using System;

namespace IndicatorsManager.IndicatorImporter.Interface.Exceptions
{
    public class IncorrectParameterException : ImporterException
    {
        public IncorrectParameterException(string message) : base(message) { }
        public IncorrectParameterException(string message, Exception inner) : base(message, inner) { }
    }
}