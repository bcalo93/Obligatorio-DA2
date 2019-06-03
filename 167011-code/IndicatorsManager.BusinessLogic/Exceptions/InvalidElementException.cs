using System;

namespace IndicatorsManager.BusinessLogic.Exceptions
{
    public class InvalidElementException : Exception
    {
        public InvalidElementException(string message) : base(message) { }
    }
}