using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using System.Data.SqlClient;

namespace IndicatorsManager.DataAccess
{
    public class UserRepository : BaseRepository<User>, IUserQuery
    {
        public UserRepository(DbContext context) : base(context) { }
        
        public override User Get(Guid id)
        {
            try
            {
                return this.context.Set<User>().Where(u => u.Id == id).FirstOrDefault();
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

        public override IEnumerable<User> GetAll()
        {
            try
            {
                return this.context.Set<User>().ToList();
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }
        
        public User GetByUsername(string username)
        {
            try
            {
                return this.context.Set<User>()
                    .Where(u => u.Username == username && !u.IsDeleted)
                    .FirstOrDefault();
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }
    }
}