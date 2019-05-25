using System;

namespace IndicatorsManager.BusinessLogic.Interface.Exceptions
{
    public class EvaluationException : BusinessLogicException
    {
        public EvaluationException(string message) : base(message) { }
        public EvaluationException(string message, Exception inner) : base(message, inner) { }
        
    }
    
}