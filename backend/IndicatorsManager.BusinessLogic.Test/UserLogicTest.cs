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
    public class UserLogicTest
    {
        [TestMethod]
        public void CreateUserOkTest()
        {
             User create = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByUsername(It.IsAny<string>())).Returns<IEnumerable<User>>(null);
            
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Add(It.IsAny<User>()));
            mockRepo.Setup(m => m.Save());
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Create(create);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
            Assert.AreEqual(create.Id, result.Id);
            UsersAssertAreEquals(create, result);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityExistException))]
        public void CreateUserWithExistingUsernameTest()
        {
             User create = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Password = "Password Test",
                Username = "Username Test",
                Email = "email@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByUsername(It.IsAny<string>())).Returns(new User{ Id = Guid.NewGuid(), Username = "Username Test", IsDeleted = false });
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Create(create);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateUserNullTest()
        {
            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Create(null);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void CreateUserDataAccessExceptionlTest()
        {
            User create = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByUsername(It.IsAny<string>())).Returns<IEnumerable<User>>(null);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Add(It.IsAny<User>()));
            mockRepo.Setup(m => m.Save()).Throws(new DataAccessException(""));

            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Create(create);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(EntityExistException))]
        public void CreateUserIdExistExceptionlTest()
        {
            User create = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByUsername(It.IsAny<string>())).Returns<IEnumerable<User>>(null);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Add(It.IsAny<User>())).Throws(new IdExistException("Una entidad con esta Id ya existe."));

            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Create(create);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateUserWrongEmailFormatTest()
        {
             User create = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByUsername(It.IsAny<string>())).Returns<IEnumerable<User>>(null);
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Create(create);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateUserNullPropertyTest()
        {
             User create = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Password = "Password Test",
                Email = "email@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByUsername(It.IsAny<string>())).Returns<IEnumerable<User>>(null);
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Create(create);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateUserEmptyStringPropertyTest()
        {
             User create = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Password = "",
                Username = "Username Test",
                Email = "email@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByUsername(It.IsAny<string>())).Returns<IEnumerable<User>>(null);
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Create(create);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateUserStringWithSpacePropertyTest()
        {
             User create = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Password = "Password Test",
                Username = "Username Test",
                Email = " ",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByUsername(It.IsAny<string>())).Returns<IEnumerable<User>>(null);
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Create(create);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void GetUserOkTest()
        {
            Guid expectedId = Guid.NewGuid();
            User getResult = new User
            {
                Id = expectedId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                IsDeleted = false,
                Role = Role.Admin
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Get(expectedId);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
            Assert.AreEqual(getResult.Id, result.Id);
            UsersAssertAreEquals(getResult, result);
        }

        [TestMethod]
        public void GetUserIsDeletedTest()
        {
            Guid expectedId = Guid.NewGuid();
            User getResult = new User
            {
                Id = expectedId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                IsDeleted = true,
                Role = Role.Admin
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Get(expectedId);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUserNotExistTest()
        {
            Guid expectedId = Guid.NewGuid();

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns<IEnumerable<User>>(null);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Get(expectedId);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void GetUserDataAccessExceptionTest()
        {
            Guid expectedId = Guid.NewGuid();

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Throws(new DataAccessException(""));
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Get(expectedId);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void GetAllUserOkTest()
        {
            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.GetAll()).Returns(CreateUsers(20));
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            IEnumerable<User> result = logic.GetAll();

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
            Assert.AreEqual(10, result.Count());
            Assert.IsTrue(result.All(u => !u.IsDeleted));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void GetAllDataAccessExceptionTest()
        {
            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.GetAll()).Throws(new DataAccessException(""));
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            IEnumerable<User> result = logic.GetAll();

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void RemoveUserOkTest()
        {
            Guid expectedId = Guid.NewGuid();
            User getResult = new User
            {
                Id = expectedId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                IsDeleted = false,
                Role = Role.Admin
            };
            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            mockRepo.Setup(m => m.Update(It.IsAny<User>()));
            mockRepo.Setup(m => m.Save());
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            logic.Remove(expectedId);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void RemoveUserIsDeletedTest()
        {
            Guid expectedId = Guid.NewGuid();
            User getResult = new User
            {
                Id = expectedId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                IsDeleted = true,
                Role = Role.Admin
            };
            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            logic.Remove(expectedId);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void RemoveUserNotExistTest()
        {
            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(It.IsAny<Guid>())).Returns<IEnumerable<User>>(null);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            logic.Remove(Guid.NewGuid());

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void RemoveUserDataAccessExceptionTest()
        {
            Guid expectedId = Guid.NewGuid();
            User getResult = new User
            {
                Id = expectedId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                IsDeleted = false,
                Role = Role.Admin
            };
            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            mockRepo.Setup(m => m.Update(It.IsAny<User>()));
            mockRepo.Setup(m => m.Save()).Throws(new DataAccessException(""));
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            logic.Remove(expectedId);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void UpdateUserOkTest()
        {
            Guid expectedId = Guid.NewGuid();
            User getResult = new User
            {
                Id = expectedId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                IsDeleted = false,
                Role = Role.Admin
            };
             
            User updateUser = new User
            {
                Name = "Test Name Changed",
                LastName = "Test LastName Changed",
                Username = "Username Test Changed",
                Password = "Password Test Changed",
                Email = "testChanged@email.com",
                IsDeleted = false,
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByUsername(It.IsAny<string>())).Returns<IEnumerable<User>>(null);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            mockRepo.Setup(m => m.Update(It.IsAny<User>()));
            mockRepo.Setup(m => m.Save());
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            
            User result = logic.Update(expectedId, updateUser);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
            Assert.AreEqual(expectedId, result.Id);
            UsersAssertAreEquals(updateUser, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void UpdateUserNullValueTest()
        {
            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            
            User result = logic.Update(Guid.NewGuid(), null);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void UserUpdateWrongEmailFormatTest()
        {
            Guid expectedId = Guid.NewGuid();
            User update = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "email",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Update(expectedId, update);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void UpdateUserNullPropertyTest()
        {
            Guid expectedId = Guid.NewGuid();
            User update = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Password = "Password Test",
                Email = "email@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Update(expectedId, update);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void UpdateUserEmptyStringPropertyTest()
        {
            Guid expectedId = Guid.NewGuid();
            User update = new User
            {
                Name = "",
                LastName = "Test LastName",
                Password = "Passowrd",
                Username = "Username Test",
                Email = "email@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Update(expectedId, update);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void UpdateUserStringWithSpacePropertyTest()
        {
            Guid expectedId = Guid.NewGuid();
            User update = new User
            {
                Name = "Test Name",
                LastName = " ",
                Password = "Password Test",
                Username = "Username Test",
                Email = "email@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Update(expectedId, update);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void UpdateUserIdNotFoundTest()
        {
            Guid expectedId = Guid.NewGuid();
            User update = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Password = "Password Test",
                Username = "Username Test",
                Email = "email@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns<IEnumerable<User>>(null);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Update(expectedId, update);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void UpdateUserIdIsDeletedTest()
        {
            Guid expectedId = Guid.NewGuid();
            User update = new User
            {
                Name = "Test Name",
                LastName = "Test LastName",
                Password = "Password Test",
                Username = "Username Test",
                Email = "email@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(new User { Id = expectedId, IsDeleted = true });
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Update(expectedId, update);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityExistException))]
        public void UpdateUserUsenameAlreadyExistTest()
        {
            Guid expectedId = Guid.NewGuid();
            User getResult = new User
            {
                Id = expectedId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                IsDeleted = false,
                Role = Role.Admin
            };

            User update = new User
            {
                Name = "Test Name Changed",
                LastName = "Test LastName Changed",
                Password = "Password Test Changed",
                Username = "Username Test",
                Email = "email@email.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByUsername(It.IsAny<string>())).Returns(new User{ Id = Guid.NewGuid(), IsDeleted = false, Username = update.Username });
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Update(expectedId, update);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void UpdateUserUsenameDoesntChangeTest()
        {
            Guid expectedId = Guid.NewGuid();
            User getResult = new User
            {
                Id = expectedId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                IsDeleted = false,
                Role = Role.Admin
            };

            User update = new User
            {
                Name = "Test Name Change",
                LastName = "Test LastName Change",
                Password = "Password Test Change",
                Username = "Username Test",
                Email = "email@emailchange.com",
                Role = Role.Manager
            };

            var mockQuery = new Mock<IUserQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByUsername(It.IsAny<string>())).Returns(getResult);
    
            var mockRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            mockRepo.Setup(m => m.Update(It.IsAny<User>()));
            mockRepo.Setup(m => m.Save());
            
            ILogic<User> logic = new UserLogic(mockRepo.Object, mockQuery.Object);
            User result = logic.Update(expectedId, update);

            mockQuery.VerifyAll();
            mockRepo.VerifyAll();
            Assert.AreEqual(expectedId, result.Id);
            UsersAssertAreEquals(update, result);
        }
        
        private void UsersAssertAreEquals(User expected, User actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.LastName, actual.LastName);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Role, actual.Role);
            Assert.AreEqual(expected.IsDeleted, actual.IsDeleted);
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
    }
    
}