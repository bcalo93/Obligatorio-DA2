using System;

namespace IndicatorsManager.BusinessLogic.Interface.Exceptions
{
    public class InvalidEntityException : BusinessLogicException
    {
        
        public InvalidEntityException(string message) : base(message) { }
        public InvalidEntityException(string message, Exception inner) : base(message, inner) { }
    }
    
}