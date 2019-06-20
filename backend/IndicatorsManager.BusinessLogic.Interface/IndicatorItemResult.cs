using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public class IndicatorItemResult
    {
        public IndicatorItem IndicatorItem { get; set; }
        public EvaluateConditionResult Result { get; set; }

        public IndicatorItemResult() { }
        
    }
}