using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq; 
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Controllers;
using IndicatorsManager.WebApi.Models;
using IndicatorsManager.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.WebApi.Test
{
    [TestClass]
    public class IndicatorsControllerTest
    {

        private Mock<IIndicatorLogic> indicatorMock;
        private Mock<ISessionLogic> sessionMock;
        private Mock<IIndicatorItemLogic> itemLogic;
        private IndicatorsController controller;

        [TestInitialize]
        public void InitMock()
        {
            indicatorMock = new Mock<IIndicatorLogic>(MockBehavior.Strict);
            sessionMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            itemLogic = new Mock<IIndicatorItemLogic>(MockBehavior.Strict);
            controller = new IndicatorsController(indicatorMock.Object, sessionMock.Object, itemLogic.Object);
        }

        [TestCleanup]
        public void VerifyAll()
        {
            indicatorMock.VerifyAll();
            sessionMock.VerifyAll();
            itemLogic.VerifyAll();
        }
        
        [TestMethod]
        public void PutOkTest()
        {
            Guid expectedId = Guid.NewGuid();
            IndicatorOnlyModel requestBody = new IndicatorOnlyModel
            {
                Name = "Name Put"
            };

            Indicator updateResult = new Indicator
            {
                Id = expectedId,
                Name = "Name Put"
            };

            indicatorMock.Setup(m => m.Update(expectedId, It.IsAny<Indicator>())).Returns(updateResult);

            var result = controller.Put(expectedId, requestBody);

            var response = result as OkObjectResult;
            IndicatorOnlyModel model = response.Value as IndicatorOnlyModel;
            Assert.AreEqual(model.Id, updateResult.Id);
            Assert.AreEqual(model.Name, updateResult.Name);
        }
        
        [TestMethod]
        public void PutIndicatorNotFoundTest()
        {
            Guid expectedId = Guid.NewGuid();
            string expectedError = String.Format("El indicador de id {0} no existe", expectedId.ToString());
            IndicatorOnlyModel requestBody = new IndicatorOnlyModel
            {
                Name = "Name Put"
            };

            indicatorMock.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<Indicator>()))
                .Throws(new EntityNotExistException(expectedError));

            var result = controller.Put(expectedId, requestBody);

            var response = result as NotFoundObjectResult;
            Assert.AreEqual(expectedError, response.Value);
        }

        [TestMethod]
        public void PutIndicatorNotValidTest()
        {
            Guid expectedId = Guid.NewGuid();
            IndicatorOnlyModel requestBody = new IndicatorOnlyModel
            {
                Name = ""
            };

            indicatorMock.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<Indicator>())).Throws(new InvalidEntityException("El indicador es invalido"));

            var result = controller.Put(expectedId, requestBody);

            var response = result as BadRequestObjectResult;
            Assert.AreEqual("El indicador es invalido", response.Value);
        }

        [TestMethod]
        public void PutIndicatorDataAccessExceptionTest()
        {
            Guid expectedId = Guid.NewGuid();
            IndicatorOnlyModel requestBody = new IndicatorOnlyModel
            {
                Name = "Test Put"
            };

            indicatorMock.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<Indicator>())).Throws(new DataAccessException("Error en el data access"));

            var result = controller.Put(expectedId, requestBody);

            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
            Assert.AreEqual("El servicio no esta disponible.", response.Value);
        }

        [TestMethod]
        public void DeleteOkTest()
        {
            Guid expectedId = Guid.NewGuid();

            indicatorMock.Setup(m => m.Remove(expectedId));

            var result = controller.Delete(expectedId);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteDataAccessExceptionTest()
        {
            Guid expectedId = Guid.NewGuid();

            indicatorMock.Setup(m => m.Remove(expectedId)).Throws(new DataAccessException(""));

            var result = controller.Delete(expectedId);

            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
            Assert.AreEqual("El servicio no esta disponible.", response.Value);
        } 
        
        [TestMethod]
        public void DeleteUserIndicatorOkTest()
        {
            indicatorMock.Setup(m => m.RemoveUserIndicator(It.IsAny<Guid>(), It.IsAny<Guid>()));
            
            var result = controller.Delete(Guid.NewGuid(), Guid.NewGuid());

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteUserIndicatorInvalidEntityTest()
        {
            string msj = "No existe";

            indicatorMock.Setup(m => m.RemoveUserIndicator(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new InvalidEntityException(msj));

            var result = controller.Delete(Guid.NewGuid(), Guid.NewGuid());

            var response = result as BadRequestObjectResult;
            Assert.AreEqual(msj, response.Value);
        }

        [TestMethod]
        public void DeleteUserIndicatorDataAccessExceptionTest()
        {
            indicatorMock.Setup(m => m.RemoveUserIndicator(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new DataAccessException(""));

            var result = controller.Delete(Guid.NewGuid(), Guid.NewGuid());

            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
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