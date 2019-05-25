using System;

namespace IndicatorsManager.BusinessLogic.Interface.Exceptions
{
    public class EntityNotExistException : BusinessLogicException
    {
        public EntityNotExistException(string message) : base(message) { }
        public EntityNotExistException(string message, Exception inner) : base(message, inner) { }
        
    }
}