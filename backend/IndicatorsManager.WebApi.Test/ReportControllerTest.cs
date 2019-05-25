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
    public class ReportsControllerTest
    {
        
        [TestMethod]
        public void GetUsersMostLogsTest()
        {
            Mock<IReportLogic> mock = new Mock<IReportLogic>(MockBehavior.Strict);
            mock.Setup(m => m.GetUsersMostLogs(It.IsAny<int>())).Returns(CreateUserData(10));

            ReportsController controller = new ReportsController(mock.Object);

            var result = controller.GetTopUsers();

            mock.VerifyAll();

            var response = result as OkObjectResult;
            IEnumerable<UserGetModel> model = response.Value as IEnumerable<UserGetModel>;
            Assert.AreEqual(10, model.Count());
        }

        [TestMethod]
        public void GetTopHiddenIndicatorsTest()
        {
            Mock<IReportLogic> mock = new Mock<IReportLogic>(MockBehavior.Strict);
            mock.Setup(m => m.GetMostHiddenIndicators(It.IsAny<int>())).Returns(CreateIndicatorsWithUserIndicators());

            ReportsController controller = new ReportsController(mock.Object);

            var result = controller.GetTopHiddenIndicators();

            mock.VerifyAll();

            var response = result as OkObjectResult;
            IEnumerable<IndicatorOnlyModel> model = response.Value as IEnumerable<IndicatorOnlyModel>;
            Assert.AreEqual(3, model.Count());
        }



        private IEnumerable<User> CreateUserData(int amount)
        {
            List<User> result = new List<User>();
            for(int i = 0; i < amount; i++)
            {
                User create = new User
                {
                    Name = "Name " + i,
                    LastName = "LastName " + i,
                    Username = "Username " + i,
                    Password = "Password " + i,
                    Email = "test" + i + "@email.com",
                    Role = Role.Manager
                };
                result.Add(create);
            }
            return result;
        }

        private IEnumerable<Indicator> CreateIndicatorsWithUserIndicators()
        {
            List<Indicator> result = new List<Indicator>();           

            Indicator indicator1 = new Indicator{ Name = "Test Indicator 1", Area = new Area { Name = "Test Area", DataSource = "Data Source" } };
            indicator1.UserIndicators.Add(new UserIndicator { User = CreateUser(1), IsVisible =  true });    
            indicator1.UserIndicators.Add(new UserIndicator { User = CreateUser(2), IsVisible =  false });        
            indicator1.UserIndicators.Add(new UserIndicator { User = CreateUser(3), IsVisible =  true });               
            result.Add(indicator1);

            Indicator indicator2 = new Indicator{ Name = "Test Indicator 2", Area = new Area { Name = "Test Area", DataSource = "Data Source" } };
            indicator2.UserIndicators.Add(new UserIndicator { User = CreateUser(4), IsVisible =  false }); 
            indicator2.UserIndicators.Add(new UserIndicator { User = CreateUser(5), IsVisible =  true });    
            indicator2.UserIndicators.Add(new UserIndicator { User = CreateUser(6), IsVisible =  false });              
            result.Add(indicator2);

            Indicator indicator3 = new Indicator{ Name = "Test Indicator 2", Area = new Area { Name = "Test Area", DataSource = "Data Source" } };
            indicator3.UserIndicators.Add(new UserIndicator { User = CreateUser(7), IsVisible =  false });    
            indicator3.UserIndicators.Add(new UserIndicator { User = CreateUser(8), IsVisible =  false });     
            indicator3.UserIndicators.Add(new UserIndicator { User = CreateUser(9), IsVisible =  false });          
            result.Add(indicator3);
            
            return result;
        }

        private User CreateUser(int i) 
        {
            User create = new User
            {
                Name = "Name" + i,
                LastName = "LastName" + i,
                Username = "Username" + i,
                Password = "Password",
                Email = "user@email.com",
                Role = Role.Manager,
                IsDeleted = false
            };
            return create;
        }

    }
    
}