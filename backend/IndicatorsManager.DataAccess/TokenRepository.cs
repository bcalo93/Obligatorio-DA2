using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using System.Data.SqlClient;

namespace IndicatorsManager.DataAccess
{
    public class TokenRepository : ITokenRepository
    {
        private DbContext Context;
        private const string CONNECTION_ERROR = "The service is unavailable.";

        public TokenRepository(DbContext context)
        {
            this.Context = context;
        }

        public void Add(AuthenticationToken entity) 
        {
            try
            {
                Context.Set<AuthenticationToken>().Add(entity);
            }
            catch(InvalidOperationException ex)
            {
                throw new IdExistException("An entity with that Id already exist.", ex);
            }
        }

        public void Update(AuthenticationToken entity) 
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public AuthenticationToken GetByToken(Guid token)
        {
            try
            {
                return Context.Set<AuthenticationToken>()
                    .Where(x => x.Token == token)
                    .FirstOrDefault();
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

        public AuthenticationToken GetByUser(User user)
        {
            try
            {
                return this.Context.Set<AuthenticationToken>()
                    .Where(a => a.User.Id == user.Id)
                    .FirstOrDefault();
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

        public void Save() 
        {
            try 
            {
                this.Context.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                throw new DataAccessException("An error's occurred when trying to save the token.", ex);
            }
        }

    }
}