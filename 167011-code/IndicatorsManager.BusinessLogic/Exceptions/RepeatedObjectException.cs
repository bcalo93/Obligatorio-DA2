using System;

namespace IndicatorsManager.BusinessLogic.Exceptions
{
    public class RepeatedObjectException : Exception
    {
        public RepeatedObjectException(string message) : base(message) { }
    }
}