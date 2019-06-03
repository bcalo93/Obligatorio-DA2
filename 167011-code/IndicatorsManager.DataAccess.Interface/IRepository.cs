using System;
using System.Linq;
using System.Collections.Generic;
using IndicatorsManager.Domain;

namespace IndicatorsManager.DataAccess.Interface
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(Guid id, T entity);
        T Get(Guid id);
        ICollection<T> GetAll();
        void Remove(T id);
        bool ExistCondition(Func<T,bool> expression);
    }
}