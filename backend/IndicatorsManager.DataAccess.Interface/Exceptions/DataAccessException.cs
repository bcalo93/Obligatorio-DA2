using System;

namespace IndicatorsManager.DataAccess.Interface.Exceptions
{
    public class DataAccessException : ArgumentException
    {
        public DataAccessException(string message) : base(message) { }
        public DataAccessException(string message, Exception inner) : base(message, inner) { }

    }
    
}