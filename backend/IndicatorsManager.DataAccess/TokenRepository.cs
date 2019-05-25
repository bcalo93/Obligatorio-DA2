using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.DataAccess
{
    public class TokenRepository : ITokenRepository
    {
        
        private DbContext Context;
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
                throw new IdExistException("Una entidad con esta Id ya existe.", ex);
            }
        }

        public void Update(AuthenticationToken entity) 
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public AuthenticationToken GetByToken(Guid token)
        {
            return Context.Set<AuthenticationToken>().Where(x => x.Token == token).FirstOrDefault();
        }

        public AuthenticationToken GetByUser(User user)
        {
            return this.Context.Set<AuthenticationToken>().Where(a => a.User.Id == user.Id).FirstOrDefault();
        }

        public void Save() 
        {
            try 
            {
                this.Context.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                throw new DataAccessException("Ocurrió un error guardando AuthenticationTokens.", ex);
            }
        }

    }
}