using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using IndicatorsManager.DataAccess.Interface;

namespace IndicatorsManager.DataAccess
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext Context { get; set; }

        public Repository(DbContext Context)
        {
            this.Context = Context;
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
            this.Save();
        }

        public abstract T Get(Guid id);

        public virtual ICollection<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }

        protected void Save()
        {
            Context.SaveChanges();
        }

        public virtual void Update(Guid id, T entity)
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            this.Save();
        }

        public virtual bool ExistCondition(Func<T, bool> expression)
        {
            return Context.Set<T>().Any(expression);
        }

        public virtual void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
            this.Save();
        }
    }
}