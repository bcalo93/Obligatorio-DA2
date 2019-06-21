using System;
using System.Collections.Generic;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IndicatorsManager.DataAccess
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DbContext context;
        protected const string CONNECTION_ERROR = "The service is unavailable.";

        public BaseRepository(DbContext context)
        {
            this.context = context;
        }

        public void Add(T entity)
        {
            try
            {
                this.context.Set<T>().Add(entity);
            }
            catch(InvalidOperationException ex)
            {
                throw new IdExistException("An entity with that Id already exist.", ex);
            }
        }

        public virtual void Remove(T entity)
        {
            this.context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            this.context.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            try 
            {
                this.context.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                throw new DataAccessException("An error occured when trying to save an entity.", ex);
            }
        }

        public abstract T Get(Guid id);

        public abstract IEnumerable<T> GetAll();

    }
}