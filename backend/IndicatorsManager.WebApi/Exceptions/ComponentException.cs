using System;

namespace IndicatorsManager.WebApi.Exceptions
{
    public class ComponentException : Exception
    {
        public ComponentException(string message) : base(message) { }
        
    }
    
}