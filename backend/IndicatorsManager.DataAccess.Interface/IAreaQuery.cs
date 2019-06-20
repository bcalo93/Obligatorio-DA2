using IndicatorsManager.Domain;

namespace IndicatorsManager.DataAccess.Interface
{
    public interface IAreaQuery
    {
        Area GetByName(string name);
        
    }
    
}