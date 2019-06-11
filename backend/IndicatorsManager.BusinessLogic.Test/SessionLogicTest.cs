using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.BusinessLogic;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using System.Linq;

namespace IndicatorsManager.BusinessLogic.Test
{
    [TestClass]
    public class SessionLogicTest 
    {
        private Mock<IRepository<Log>> mockLogger;
        private Mock<IRepository<User>> mockUserRepo;
        private Mock<ITokenRepository> mockTokenRepo;
        private ISessionLogic session;

        [TestInitialize]
        public void InitMocks()
        {
            mockLogger = new Mock<IRepository<Log>>(MockBehavior.Strict);
            mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockTokenRepo = new Mock<ITokenRepository>(MockBehavior.Strict);
            session = new SessionLogic(mockTokenRepo.Object, mockUserRepo.Object, mockLogger.Object);
        }

        [TestCleanup]
        public void VerifyAll()
        {
            mockLogger.VerifyAll();
            mockUserRepo.VerifyAll();
            mockTokenRepo.VerifyAll();
        }

        [TestMethod]
        public void CreateNewAuthenticationTokenOkTest()
        {
            var users = CreateUsers(10);
            var user = users.ElementAt(5);

            mockLogger.Setup(m => m.Add(It.IsAny<Log>()));
            mockLogger.Setup(m => m.Save());
                    
            mockUserRepo.Setup(m => m.GetAll()).Returns(users);

            mockTokenRepo.Setup(m => m.GetByUser(It.IsAny<User>())).Returns<IEnumerable<AuthenticationToken>>(null);
            mockTokenRepo.Setup(m => m.Add(It.IsAny<AuthenticationToken>()));
            mockTokenRepo.Setup(m => m.Save());
            
            AuthenticationToken result = session.CreateToken(user.Username, user.Password);

            Assert.AreEqual(user.Id, result.User.Id);
        }
        
        [TestMethod]
        public void CreateUpdateAuthenticationTokenOkTest()
        {
            var users = CreateUsers(10);
            var user = users.ElementAt(5);
            Guid token = Guid.NewGuid();
            Guid id = Guid.NewGuid();

            AuthenticationToken authToken = new AuthenticationToken
            {
                Id = id,
                Token = token,
                User = user
            };
            
            mockLogger.Setup(m => m.Add(It.IsAny<Log>()));
            mockLogger.Setup(m => m.Save());
                      
            mockUserRepo.Setup(m => m.GetAll()).Returns(users);

            mockTokenRepo.Setup(m => m.GetByUser(It.IsAny<User>())).Returns(authToken);
            mockTokenRepo.Setup(m => m.Update(It.IsAny<AuthenticationToken>()));
            mockTokenRepo.Setup(m => m.Save());
            
            AuthenticationToken result = session.CreateToken(user.Username, user.Password);

            Assert.AreNotEqual(token, result.Token);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedException))]
        public void CreateAuthenticationTokenInvalidUsernameOrPasswordTest()
        {
            var users = CreateUsers(10);
            
            mockUserRepo.Setup(m => m.GetAll()).Returns(users);
            
            AuthenticationToken result = session.CreateToken("invalid username", "invalid password");
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedException))]
        public void CreateAuthenticationTokenUserDeletedTest()
        {
            var users = CreateUsers(10);
            User deleted = users.ElementAt(6);
            deleted.IsDeleted = true;
            
            mockUserRepo.Setup(m => m.GetAll()).Returns(users);
            
            AuthenticationToken result = session.CreateToken(deleted.Username, deleted.Password);
        }

        [TestMethod]
        public void IsValidTokenOkTest()
        {
            AuthenticationToken authToken = new AuthenticationToken
            {
                Token = Guid.NewGuid(),
                User = CreateUser(Guid.NewGuid(), false)
            };
            
            mockTokenRepo.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(authToken);
            
            bool result = session.IsValidToken(authToken.Token);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidTokenUserDeletedOkTest()
        {
            AuthenticationToken authToken = new AuthenticationToken
            {
                Token = Guid.NewGuid(),
                User = CreateUser(Guid.NewGuid(), true)
            };
            
            mockTokenRepo.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(authToken);
            
            bool result = session.IsValidToken(authToken.Token);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidTokenUserNullOkTest()
        {
            AuthenticationToken authToken = new AuthenticationToken
            {
                Token = Guid.NewGuid()
            };
            
            mockTokenRepo.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(authToken);
            
            bool result = session.IsValidToken(authToken.Token);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsNotValidTokenOkTest()
        {
            mockTokenRepo.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns<IEnumerable<AuthenticationToken>>(null);
            
            SessionLogic session = new SessionLogic(mockTokenRepo.Object, mockUserRepo.Object, mockLogger.Object);
            bool result = session.IsValidToken(Guid.NewGuid());

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetUserOkTest()
        {
            User user = CreateUser(Guid.NewGuid(), false);

            AuthenticationToken authToken = new AuthenticationToken
            {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid(),
                User = user
            };

            mockUserRepo.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);

            mockTokenRepo.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(authToken);
            
            SessionLogic session = new SessionLogic(mockTokenRepo.Object, mockUserRepo.Object, mockLogger.Object);
            User result = session.GetUser(authToken.Token);

            Assert.AreEqual(user.Id, result.Id);
        }

        [TestMethod]
        public void GetUserAuthTokenNotValidTest()
        {
            mockTokenRepo.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns<IEnumerable<AuthenticationToken>>(null);
            
            User result = session.GetUser(Guid.NewGuid());

            Assert.IsNull(result);
        }

        [TestMethod]
        public void HasLevelOkTest()
        {
            User user = CreateUser(Guid.NewGuid(), false);

            AuthenticationToken authToken = new AuthenticationToken
            {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid(),
                User = user
            };

            mockUserRepo.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);

            mockTokenRepo.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(authToken);
            
            bool result = session.HasLevel(authToken.Token, Role.Manager);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasLevelNotValidUserTest()
        {
            User user = CreateUser(Guid.NewGuid(), false);

            AuthenticationToken authToken = new AuthenticationToken
            {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid(),
                User = user
            };

            mockUserRepo.Setup(m => m.Get(It.IsAny<Guid>())).Returns<IEnumerable<User>>(null);

            mockTokenRepo.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(authToken);
            
            bool result = session.HasLevel(authToken.Token, Role.Manager);

            Assert.IsFalse(result);
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
                    User = CreateUser(Guid.NewGuid(), false)
                };
                repository.Add(authToken);
                result.Add(authToken);
            }
            repository.Save();
            return result;
        }

        private IEnumerable<User> CreateUsers(int amount)
        {
            List<User> result = new List<User>();
            for(int i = 0; i < amount; i++)
            {
                result.Add(new User 
                {
                    Id = Guid.NewGuid(),
                    Name = "TestName" + i,
                    LastName = "TestLastName" + i,
                    Username = "TestUsername" + i,
                    Password = "Password" + i,
                    Email = "mail@mail.com",
                    Role = i % 2 == 0 ? Role.Admin : Role.Manager,
                    IsDeleted = i % 2 == 0
                });
            }
            return result;
        }

        private User CreateUser(Guid id, bool isDeleted) 
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
                IsDeleted = isDeleted
            };
            return create;
        }

    }
}