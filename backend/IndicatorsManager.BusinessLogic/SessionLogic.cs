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
    public class SessionLogic : ISessionLogic
    {
        private ITokenRepository repository;

        private IRepository<User> userRepo;

        private IRepository<Log> logger;


        public SessionLogic(ITokenRepository repository, IRepository<User> userRepo, IRepository<Log> logger)
        {
            this.repository = repository;
            this.userRepo = userRepo;
            this.logger = logger;
        }

        public bool IsValidToken(Guid token)
        {
            AuthenticationToken authToken = repository.GetByToken(token);
            return authToken != null;
        }

        public AuthenticationToken CreateToken(string username, string password)
        {
            var users = userRepo.GetAll();
            var user = users.FirstOrDefault(x => x.Username == username && x.Password == password);
            if (user == null)
            {
                return null;
            }
            AuthenticationToken newAuthToken = new AuthenticationToken() 
            {
                Token = Guid.NewGuid(),
                User = user
            };
            Log log = new Log()
            {
                DateTime = DateTime.Now,
                User = user
            };
            logger.Add(log);
            logger.Save();

            AuthenticationToken existingAuthToken = repository.GetByUser(user);
            if (existingAuthToken != null)
            {
                existingAuthToken.Update(newAuthToken);
                repository.Update(existingAuthToken);
                repository.Save();
                return existingAuthToken;
            }
            else
            {
                repository.Add(newAuthToken);
                repository.Save();
                return newAuthToken;
            }
        }

        public bool HasLevel(Guid token, Role role)
        {  
            var user = GetUser(token);
            if (user == null) {
                return false;
            }
            return user.Role == role;
        }

        public User GetUser(Guid token)
        {
            AuthenticationToken authToken = repository.GetByToken(token);
            if (authToken == null)
            {
                return null;
            }
            return userRepo.Get(authToken.User.Id);
        }


    }
}