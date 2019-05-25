using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.DataAccess
{
    public class UserRepository : BaseRepository<User>, IUserQuery
    {
        public UserRepository(DbContext context) : base(context) { }
        
        public override User Get(Guid id)
        {
            return this.context.Set<User>().Where(u => u.Id == id).FirstOrDefault();
        }

        public override IEnumerable<User> GetAll()
        {
            return this.context.Set<User>().ToList();
        }
        
        public User GetByUsername(string username)
        {
            return this.context.Set<User>().Where(u => u.Username == username && !u.IsDeleted).FirstOrDefault();
        }
    }
}