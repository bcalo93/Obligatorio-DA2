using System;

namespace IndicatorsManager.BusinessLogic.Exceptions
{
    public class ElementNotFoundException : Exception
    {
        public ElementNotFoundException(string message) : base(message) { }
    }
}