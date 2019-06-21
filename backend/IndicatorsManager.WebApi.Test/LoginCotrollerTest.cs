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
    public class LoginControllerTest
    {

        [TestMethod]
        public void LoginOkTest()
        {
            LoginModel body = new LoginModel
            {
                Username = "username",
                Password = "password"
            };
            Guid token = Guid.NewGuid();
            User user = CreateUser();
            AuthenticationToken authToken = new AuthenticationToken()
            {
                Token = token,
                User = user
            };
            
            var mockSession = new Mock<ISessionLogic>(MockBehavior.Strict);
            mockSession.Setup(m => m.CreateToken(It.IsAny<string>(), It.IsAny<string>())).Returns(authToken);

            LoginController controller = new LoginController(mockSession.Object);
            var result = controller.Login(body);
            
            mockSession.VerifyAll();

            var createResponse = result as OkObjectResult;
            LoginModelOut resultToken = (LoginModelOut)(createResponse.Value as object);
            Assert.AreEqual(token, resultToken.Token);
            Assert.AreEqual(user.Name, resultToken.User.Name);
        }

        [TestMethod]
        public void LoginFailTest()
        {
            LoginModel body = new LoginModel
            {
                Username = "username",
                Password = "password"
            };            
            
            var mockSession = new Mock<ISessionLogic>(MockBehavior.Strict);
            mockSession.Setup(m => m.CreateToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<IEnumerable<AuthenticationToken>>(null);

            LoginController controller = new LoginController(mockSession.Object);
            var result = controller.Login(body);
            
            mockSession.VerifyAll();

            var response = result as BadRequestObjectResult;
            Assert.AreEqual("User/password invalid.", response.Value);
        }




        private User CreateUser() 
        {
            User create = new User
            {
                Id = Guid.NewGuid(),
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