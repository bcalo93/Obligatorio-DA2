
namespace IndicatorsManager.DataAccess.Interface
{
    public interface IQueryRunner
    {
        void SetConnectionString(string connectionString);
        object RunQuery(string query);
        
    }
    
}