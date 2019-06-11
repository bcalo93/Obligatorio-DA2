using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.Domain;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.BusinessLogic
{
    public class UserLogic : ILogic<User>
    {
        private IRepository<User> repository;
        private IUserQuery query;

        public UserLogic(IRepository<User> repository, IUserQuery query)
        {
            this.repository = repository;
            this.query = query;
        }

        public User Create(User user)
        {
            if(user == null || !user.IsValid())
            {
                throw new InvalidEntityException("The user's data is invalid.");
            }
            User checkUsername = this.query.GetByUsername(user.Username);
            if(checkUsername != null)
            {
                throw new EntityExistException("The username you are trying to create already exist.");
            }
            
            try
            {
                this.repository.Add(user);
                this.repository.Save();
            }
            catch(IdExistException ie)
            {
                throw new EntityExistException("A User with that Id already exist.", ie);
            }

            return user;
        }

        public User Get(Guid id)
        {
            User result = this.repository.Get(id);
            return result != null && !result.IsDeleted ? result : null;
        }

        public IEnumerable<User> GetAll()
        {
            return this.repository.GetAll().Where(u => !u.IsDeleted);
        }

        public void Remove(Guid id)
        {
            User toRemove = this.repository.Get(id);
            if(toRemove != null && !toRemove.IsDeleted)
            {
                toRemove.IsDeleted = true;
                this.repository.Update(toRemove);
                this.repository.Save();
            }
        }

        public User Update(Guid id, User user)
        {
            if(user == null || !user.IsValid())
            {
                throw new InvalidEntityException("Los datos del usuario son invalidos");
            }
            User toUpdate = this.repository.Get(id);
            if(toUpdate == null || toUpdate.IsDeleted)
            {
                return null;
            }
            User usernameCheck = this.query.GetByUsername(user.Username);
            if(usernameCheck != null && toUpdate.Id != usernameCheck.Id)
            {
                throw new EntityExistException("El nuevo nombre de usuario ya existe.");
            }
            toUpdate = toUpdate.Update(user);
            this.repository.Update(toUpdate);
            this.repository.Save();
            return toUpdate;
        }
    }

}