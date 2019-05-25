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
                throw new IdExistException("Una entidad con esta Id ya existe.", ex);
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
                throw new DataAccessException("Ocurrió un error guardando la entidad.", ex);
            }
        }

        public abstract T Get(Guid id);

        public abstract IEnumerable<T> GetAll();

    }
}