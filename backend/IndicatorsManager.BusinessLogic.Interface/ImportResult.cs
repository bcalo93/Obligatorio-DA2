
namespace IndicatorsManager.BusinessLogic.Interface
{
    public class ImportResult
    {
        public int IndicatorsImported { get; set; }
        public int TotalIndicators { get; set; }
        public string Error { get; set; }
    }
}