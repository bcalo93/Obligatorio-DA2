using System;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public interface IUserAreaLogic
    {
        void AddAreaManager(Guid areaId, Guid userId);

        void RemoveAreaManager(Guid areaId, Guid userId);

    }
}