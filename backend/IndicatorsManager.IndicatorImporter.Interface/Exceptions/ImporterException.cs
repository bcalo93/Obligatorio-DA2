using System;

namespace IndicatorsManager.IndicatorImporter.Interface.Exceptions
{
    public class ImporterException : Exception
    {
        public ImporterException(string message) : base(message) { }
        public ImporterException(string message, Exception inner) : base(message, inner) { }
    }
}