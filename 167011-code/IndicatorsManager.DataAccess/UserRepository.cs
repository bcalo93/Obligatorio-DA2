using System;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IndicatorsManager.DataAccess
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(DbContext Context) : base(Context) { }
        
        public override User Get(Guid id)
        {
            return Context.Set<User>().Include("Areas").Where(s => s.Id == id).FirstOrDefault<User>();
        }

        public override ICollection<User> GetAll()
        {
            return Context.Set<User>().Include("Areas").ToList();
        }
    }
}
