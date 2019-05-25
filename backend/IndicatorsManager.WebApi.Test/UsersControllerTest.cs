using System;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using IndicatorsManager.WebApi.Controllers;
using IndicatorsManager.WebApi.Models;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IndicatorsManager.WebApi.Test
{
    [TestClass]
    public class UsersControllerTest
    {
        [TestMethod]
        public void PostOkTest()
        {
            UserPersistModel requestBody = new UserPersistModel
            {
                Name = "Name Post",
                LastName = "LastName Post",
                Username = "Username Post",
                Password = "Password Post",
                Email = "test@post.com",
                Role = Role.Manager
            };
            
            User createResult = new User
            {
                Id = Guid.NewGuid(), 
                Name = "Name Post",
                LastName = "LastName Post",
                Username = "Username Post",
                Password = "Password Post",
                Email = "test@post.com",
                Role = Role.Manager
            };

            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Create(It.IsAny<User>())).Returns(createResult);

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Post(requestBody);
            
            mock.VerifyAll();
            var createResponse = result as CreatedAtRouteResult;
            UserGetModel model = createResponse.Value as UserGetModel;
            AssertUsers(createResult, model);
            
        }

        [TestMethod]
        public void PostInvalidEntityExceptionTest()
        {
            UserPersistModel requestBody = new UserPersistModel
            {
                Name = "Name Post",
                LastName = "LastName Post",
                Username = "Username Post",
                Password = "Password Post",
                Email = "@post.com",
                Role = Role.Manager
            };
            

            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Create(It.IsAny<User>())).Throws(new InvalidEntityException("Los datos del usuario son invalidos."));

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Post(requestBody);
            
            mock.VerifyAll();
            var response = result as BadRequestObjectResult;
            Assert.AreEqual("Los datos del usuario son invalidos.", response.Value);
            
        }

        [TestMethod]
        public void PostEntityExistExceptionTest()
        {
            UserPersistModel requestBody = new UserPersistModel
            {
                Name = "Name Post",
                LastName = "LastName Post",
                Username = "Username Post",
                Password = "Password Post",
                Email = "@post.com",
                Role = Role.Manager
            };
            

            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Create(It.IsAny<User>())).Throws(new EntityExistException("El nombre de usuario que esta intentando crear ya existe."));

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Post(requestBody);
            
            mock.VerifyAll();
            var response = result as ConflictObjectResult;
            Assert.AreEqual("El nombre de usuario que esta intentando crear ya existe.", response.Value);
        }

        [TestMethod]
        public void PostDataAccessExceptionTest()
        {
            UserPersistModel requestBody = new UserPersistModel
            {
                Name = "Name Post",
                LastName = "LastName Post",
                Username = "Username Post",
                Password = "Password Post",
                Email = "@post.com",
                Role = Role.Manager
            };
            

            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Create(It.IsAny<User>())).Throws(new DataAccessException(""));

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Post(requestBody);

            mock.VerifyAll();
            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
            Assert.AreEqual("El servicio no esta disponible.", response.Value);
        }

        [TestMethod]
        public void GetUserOkTest()
        {
            Guid expectedId = Guid.NewGuid();
            User getResult = new User
            {
                Id = expectedId,
                Name = "Name Get",
                LastName = "LastName Get",
                Username = "Username Get",
                Password = "Password Get",
                Email = "test@get.com",
                Role = Role.Manager
            };

            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Get(expectedId)).Returns(getResult);

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Get(expectedId);

            mock.VerifyAll();
            var response = result as OkObjectResult;
            UserGetModel model = response.Value as UserGetModel;
            AssertUsers(getResult, model);
        }

        [TestMethod]
        public void GetUserNotFoundTest()
        {
            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Get(It.IsAny<Guid>())).Returns<IEnumerable<User>>(null);

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Get(Guid.NewGuid());

            mock.VerifyAll();
            var response = result as NotFoundObjectResult;
            Assert.AreEqual("El usuario no existe.", response.Value);
        }

        [TestMethod]
        public void GetUserDataAccessExceptionTest()
        {

            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Get(It.IsAny<Guid>())).Throws(new DataAccessException(""));

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Get(Guid.NewGuid());

            mock.VerifyAll();
            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
            Assert.AreEqual("El servicio no esta disponible.", response.Value);
        }

        [TestMethod]
        public void GetAllUserOkTest()
        {
            IEnumerable<User> getAllResult = CreateUsers(20);
            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.GetAll()).Returns(getAllResult);

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Get();

            mock.VerifyAll();
            var response = result as OkObjectResult;
            IEnumerable<UserGetModel> model = response.Value as IEnumerable<UserGetModel>;
            Assert.AreEqual(20, model.Count());
            foreach (User user in getAllResult)
            {
                Assert.IsTrue(model.Any(u => 
                    u.Id == user.Id &&
                    u.Name == user.Name &&
                    u.LastName == user.LastName &&
                    u.Username == user.Username &&
                    u.Role == user.Role &&
                    u.Email == user.Email
                ));
            }
        }

        [TestMethod]
        public void GetAllUserDataAccessExceptionTest()
        {
            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.GetAll()).Throws(new DataAccessException(""));

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Get();

            mock.VerifyAll();
            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
            Assert.AreEqual("El servicio no esta disponible.", response.Value);
            
        }

        [TestMethod]
        public void PutUserOkTest()
        {
            Guid expectedId = Guid.NewGuid();
            UserPersistModel requestBody = new UserPersistModel
            {
                Name = "Name Put",
                LastName = "LastName Put",
                Username = "Username Put",
                Password = "Password Put",
                Email = "test@put.com",
                Role = Role.Admin
            };

            User updateResult = new User
            {
                Id = expectedId,
                Name = "Name Put",
                LastName = "LastName Put",
                Username = "Username Put",
                Password = "Password Put",
                Email = "test@put.com",
                Role = Role.Admin
            };

            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Update(expectedId, It.IsAny<User>())).Returns(updateResult);

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Put(expectedId, requestBody);

            mock.VerifyAll();
            var response = result as OkObjectResult;
            UserGetModel model = response.Value as UserGetModel;
            AssertUsers(updateResult, model);
        }

        [TestMethod]
        public void PutNullResultTest()
        {
            UserPersistModel requestBody = new UserPersistModel
            {
                Name = "Name Put",
                LastName = "LastName Put",
                Username = "Username Put",
                Password = "Password Put",
                Email = "test@put.com",
                Role = Role.Manager
            };

            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<User>())).Returns<IEnumerable<User>>(null);

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Put(Guid.NewGuid(), requestBody);
            
            mock.VerifyAll();
            var response = result as NotFoundObjectResult;
            Assert.AreEqual("El Usuario Username Put no existe.", response.Value);
        }

        [TestMethod]
        public void PutInvalidEntityExceptionTest()
        {
            UserPersistModel requestBody = new UserPersistModel
            {
                Name = "Name Put",
                LastName = "LastName Put",
                Username = "Username Put",
                Password = "Password Put",
                Email = "@put.com",
                Role = Role.Manager
            };

            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<User>())).Throws(new InvalidEntityException("Los datos del usuario son invalidos."));

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Put(Guid.NewGuid(), requestBody);
            
            mock.VerifyAll();
            var response = result as BadRequestObjectResult;
            Assert.AreEqual("Los datos del usuario son invalidos.", response.Value);
            
        }

        [TestMethod]
        public void PutEntityExistExceptionTest()
        {
            UserPersistModel requestBody = new UserPersistModel
            {
                Name = "Name Put",
                LastName = "LastName Put",
                Username = "Username Put",
                Password = "Password Put",
                Email = "test@put.com",
                Role = Role.Manager
            };
            

            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<User>())).Throws(new EntityExistException("El nombre de usuario que esta intentando crear ya existe."));

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Put(Guid.NewGuid(), requestBody);
            
            mock.VerifyAll();
            var response = result as ConflictObjectResult;
            Assert.AreEqual("El nombre de usuario que esta intentando crear ya existe.", response.Value);
        }

        [TestMethod]
        public void PutDataAccessExceptionTest()
        {
            UserPersistModel requestBody = new UserPersistModel
            {
                Name = "Name Put",
                LastName = "LastName Put",
                Username = "Username Put",
                Password = "Password Put",
                Email = "test@put.com",
                Role = Role.Manager
            };

            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<User>())).Throws(new DataAccessException(""));

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Put(Guid.NewGuid(), requestBody);

            mock.VerifyAll();
            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
            Assert.AreEqual("El servicio no esta disponible.", response.Value);
        }

        [TestMethod]
        public void DeleteOkTest()
        {
            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Remove(It.IsAny<Guid>()));

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Delete(Guid.NewGuid());

            mock.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteDataAccessExceptionTest()
        {
            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Remove(It.IsAny<Guid>())).Throws(new DataAccessException(""));

            UsersController controller = new UsersController(mock.Object);
            var result = controller.Delete(Guid.NewGuid());

            mock.VerifyAll();
            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
            Assert.AreEqual("El servicio no esta disponible.", response.Value);
        }

        private void AssertUsers(User expected, UserPersistModel actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.LastName, actual.LastName);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Role, actual.Role);
        }

        private void AssertUsers(User expected, UserGetModel actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.LastName, actual.LastName);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Role, actual.Role);
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
