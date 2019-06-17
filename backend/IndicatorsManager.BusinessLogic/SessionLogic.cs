using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.Domain;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;

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
            return authToken != null && authToken.User != null && !authToken.User.IsDeleted;
        }

        public AuthenticationToken CreateToken(string username, string password)
        {
            User user = CheckCredentials(username, password);
            AuthenticationToken authToken = repository.GetByUser(user);
                        
            if (authToken != null)
            {
                authToken.Token = Guid.NewGuid();
                repository.Update(authToken);
            }
            else
            {
                authToken = new AuthenticationToken() { Token = Guid.NewGuid(), User = user };
                repository.Add(authToken);
            }
            repository.Save();
            Log log = new Log()
            {
                DateTime = DateTime.Now,
                User = user
            };
            logger.Add(log);
            logger.Save();
            return authToken;
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

        private User CheckCredentials(string username, string password)
        {
            IEnumerable<User> users = userRepo.GetAll();
            User result = users.FirstOrDefault(x => x.Username == username && 
                x.Password == password && !x.IsDeleted);
            if(result == null)
            {
                throw new UnauthorizedException("The credentials are invalid.");
            }
            return result;
        }
    }
}