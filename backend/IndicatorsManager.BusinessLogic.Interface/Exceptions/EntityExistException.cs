using System;

namespace  IndicatorsManager.BusinessLogic.Interface.Exceptions
{
    public class EntityExistException : BusinessLogicException
    {
        public EntityExistException(string message) : base(message) { }
        public EntityExistException(string message, Exception inner) : base(message, inner) { }

        
    }
    
}