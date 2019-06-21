using System;

namespace IndicatorsManager.DataAccess.Interface.Exceptions
{
    public class IdExistException : DataAccessException
    {
        public IdExistException(string message) : base(message) { }
        public IdExistException(string message, Exception inner) : base(message, inner) { }
    }
    
}