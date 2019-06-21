using System;

namespace IndicatorsManager.BusinessLogic.Interface.Exceptions
{
    public class IndicatorException : BusinessLogicException
    {
        public IndicatorException(string message) : base(message) { }
        public IndicatorException(string message, Exception inner) : base(message, inner) { }
    }
}