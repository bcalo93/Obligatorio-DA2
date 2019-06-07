
using System;

namespace IndicatorsManager.BusinessLogic.Interface.Exceptions
{
    public class UnauthorizedException : BusinessLogicException
    {
        public UnauthorizedException(string message) : base(message) { }
        public UnauthorizedException(string message, Exception inner) : base(message, inner) { }
    }
}