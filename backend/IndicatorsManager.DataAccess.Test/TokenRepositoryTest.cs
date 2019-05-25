using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IndicatorsManager.DataAccess;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.DataAccess.Test
{
    [TestClass]
    public class TokenRepositoryTest : BaseTest
    {
        [TestMethod]
        public void CreateAuthenticationTokenOKTest()
        {
            ITokenRepository repository = new TokenRepository(Context);

            var token = Guid.NewGuid();

            AuthenticationToken authToken = new AuthenticationToken
            {
                Token = token,
                User = CreateUser(Guid.NewGuid())
            };

            repository.Add(authToken);
            repository.Save();

            Assert.AreEqual(repository.GetByToken(token).Token, token);
        }              

        [TestMethod]
        [ExpectedException(typeof(IdExistException))]        
        public void CreateDuplicateAuthenticationTokenTest()
        {
            ITokenRepository repository = new TokenRepository(Context);

            var id = Guid.NewGuid();

            AuthenticationToken AuthenticationTokenTest1 = new AuthenticationToken
            {
                Id = id,
                Token = Guid.NewGuid(),
                User = CreateUser(Guid.NewGuid())
            };

            AuthenticationToken AuthenticationTokenTest2 = new AuthenticationToken
            {
                Id = id,
                Token = Guid.NewGuid(),
                User = CreateUser(Guid.NewGuid())
            };

            repository.Add(AuthenticationTokenTest1);
            repository.Add(AuthenticationTokenTest2);
            repository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]        
        public void CreateNullAuthenticationTokenTest()
        {
            ITokenRepository repository = new TokenRepository(Context);
            
            repository.Add(null);
            repository.Save();
        }

        [TestMethod]
        public void ModifyAuthenticationTokenOkTest()
        {
            ITokenRepository repository = new TokenRepository(Context);
            AuthenticationToken authToken = CreateAuthenticationTokenData(repository, 10).ElementAt(5);
            
            AuthenticationToken update = new AuthenticationToken {
                Token = Guid.NewGuid(),
                User = CreateUser(Guid.NewGuid())
            };
            
            authToken.Update(update);
            repository.Update(authToken);
            repository.Save();

            AuthenticationToken result = repository.GetByToken(authToken.Token);
            Assert.IsTrue(result.Token == update.Token && result.User.Id == update.User.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void ModifyAuthenticationTokenNotExistTest()
        {
            ITokenRepository repository = new TokenRepository(Context);
            CreateAuthenticationTokenData(repository, 10);

            AuthenticationToken update = new AuthenticationToken {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid(),
                User = CreateUser(Guid.NewGuid())
            };

            repository.Update(update);
            repository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ModifyAuthenticationTokenNullTest()
        {
            ITokenRepository repository = new TokenRepository(Context);
            repository.Update(null);
        }
       

        [TestMethod]
        public void GetAuthenticationTokenByTokenOKTest()
        {
            ITokenRepository repository = new TokenRepository(Context);
            AuthenticationToken expected = CreateAuthenticationTokenData(repository, 10).ElementAt(5);

            AuthenticationToken result = repository.GetByToken(expected.Token);

            Assert.AreEqual(expected.Token, result.Token);
        }

        [TestMethod]
        public void GetAuthenticationTokenByTokenNotExistTest()
        {
            ITokenRepository repository = new TokenRepository(Context);
            CreateAuthenticationTokenData(repository, 10);
            
            AuthenticationToken result = repository.GetByToken(Guid.NewGuid());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAuthenticationTokenByUserOkTest()
        {
            TokenRepository repository = new TokenRepository(Context);
            AuthenticationToken expected = CreateAuthenticationTokenData(repository, 10).ElementAt(5);

            AuthenticationToken result = repository.GetByUser(expected.User);
            Assert.AreEqual(expected.Id, result.Id); 
        }

        [TestMethod]
        public void GetAuthenticationTokenByUserNotExistTest()
        {
            TokenRepository repository = new TokenRepository(Context);
            CreateAuthenticationTokenData(repository, 10);

            AuthenticationToken result = repository.GetByUser(CreateUser(Guid.NewGuid()));
            Assert.IsNull(result);
        }


        private IEnumerable<AuthenticationToken> CreateAuthenticationTokenData(ITokenRepository repository, int amount)
        {
            List<AuthenticationToken> result = new List<AuthenticationToken>();
            for(int i = 0; i < amount; i++)
            {
                AuthenticationToken authToken = new AuthenticationToken
                {
                    Id = Guid.NewGuid(),
                    Token = Guid.NewGuid(),
                    User = CreateUser(Guid.NewGuid())
                };
                repository.Add(authToken);
                result.Add(authToken);
            }
            repository.Save();
            return result;
        }

        private User CreateUser(Guid id) 
        {
            User create = new User
            {
                Id = id,
                Name = "Name",
                LastName = "LastName",
                Username = "Username",
                Password = "Password",
                Email = "user@email.com",
                Role = Role.Manager,
                IsDeleted = false
            };
            return create;
        }


    }

    
}