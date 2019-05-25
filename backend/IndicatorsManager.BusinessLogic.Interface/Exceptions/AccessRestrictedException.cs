using System;

namespace IndicatorsManager.BusinessLogic.Interface.Exceptions
{
    public class AccessRestrictedException: BusinessLogicException
    {
        public AccessRestrictedException(string message) : base(message) { }

        public AccessRestrictedException(string message, Exception inner) : base(message, inner) { }
        
    }
    
}