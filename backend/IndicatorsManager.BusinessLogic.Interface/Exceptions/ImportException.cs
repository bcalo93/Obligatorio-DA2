using System;
namespace IndicatorsManager.BusinessLogic.Interface.Exceptions
{
    public class ImportException : BusinessLogicException
    {
        public ImportException(string message) : base(message) { }
        public ImportException(string message, Exception inner) : base(message, inner) { }
    }
}