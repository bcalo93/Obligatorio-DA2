using System;
using System.Collections.Generic;
using IndicatorsManager.BusinessLogic;
using IndicatorsManager.Domain;
using IndicatorsManager.WebApi.Controllers;
using IndicatorsManager.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IndicatorsManager.WebApi.Tests
{
    [TestClass]
    public class ConfigurationsControllerTests
    {
        private Guid USER_ID = Guid.NewGuid();      
        private Guid INDICATOR_ID_1 = Guid.NewGuid();
        private const String GET_USER_ROUTE = "GetUser";
        
        private Mock<IConfigurationLogic> mockConfiguration;

        private IndicatorConfigurationModel CreateConfiguration_1() => new IndicatorConfigurationModel()
        { 
            IsVisible = true, 
            Position = 1, 
            IndicatorId = INDICATOR_ID_1
        };

        private IndicatorConfigurationModel CreateConfiguration_1_updated() => new IndicatorConfigurationModel()
        { 
            IsVisible = false, 
            Position = 2, 
            IndicatorId = INDICATOR_ID_1
        };


        [TestInitialize]
        public void SetUp()
        {
            mockConfiguration = new Mock<IConfigurationLogic>(MockBehavior.Strict);
        }

        [TestMethod]
        public void AddUserConfigurations()
        {
            User user = new User();
            var newConfig = CreateConfiguration_1();

            user.IndicatorConfigurations.AddRange(new List<UserIndicator>{IndicatorConfigurationModel.ToEntity(newConfig)});

            List<IndicatorConfigurationModel> userConfigurations = new List<IndicatorConfigurationModel>(){ newConfig };

            mockConfiguration.Setup(m => m.AddConfiguration(It.IsAny<Guid>(), It.IsAny<List<UserIndicator>>())).Returns(user);

            var controller = new ConfigurationsController(mockConfiguration.Object);

            var result = controller.AddUserConfigurations(USER_ID, userConfigurations);
           
            var result_ = result as CreatedAtRouteResult;
            var modelOut = result_.Value as UserModelOut;

            mockConfiguration.VerifyAll();

            Assert.IsNotNull(result_);
            Assert.IsInstanceOfType(result_, typeof(CreatedAtRouteResult));
            Assert.AreEqual(GET_USER_ROUTE, result_.RouteName);  

            UserModelOut userModelOut = UserModelOut.ToModel(user);
            Assert.AreEqual(userModelOut.IndicatorConfigurations.Count, modelOut.IndicatorConfigurations.Count);  

        }


        [TestMethod]
        public void UpdateUserConfigurations()
        {
            User user = new User();
            var newConfig = CreateConfiguration_1_updated();
            user.IndicatorConfigurations.AddRange(new List<UserIndicator>{IndicatorConfigurationModel.ToEntity(newConfig)});

            mockConfiguration.Setup(m => m.UpdateConfiguration(It.IsAny<Guid>(), It.IsAny<UserIndicator>())).Returns(user);

            var controller = new ConfigurationsController(mockConfiguration.Object);

            var result = controller.UpdateUserConfigurations(USER_ID, newConfig);
           
            var result_ = result as OkObjectResult;
            var modelOut = result_.Value as UserModelOut;

            mockConfiguration.VerifyAll();

            Assert.IsNotNull(result_);
            Assert.IsInstanceOfType(result_, typeof(OkObjectResult)); 
            Assert.IsNotNull(modelOut.IndicatorConfigurations.Find(x => x.Position == 2 && x.IsVisible == false));  
        }


        [TestMethod]
        public void GetTopHiddenIndicators()
        {
            int TopHiddenLimit = 10;

            List<Indicator> topHiddenIndicators = new List<Indicator>();

            mockConfiguration.Setup(m => m.GetTopHiddenIndicators(It.IsAny<int>())).Returns(topHiddenIndicators);

            var controller = new ConfigurationsController(mockConfiguration.Object);

            var result = controller.GetTopHiddenIndicators(TopHiddenLimit);
           
            var result_ = result as OkObjectResult;
            var modelOut = result_.Value as List<IndicatorModel>;

            mockConfiguration.VerifyAll();

            Assert.IsNotNull(result_);
            Assert.IsInstanceOfType(result_, typeof(OkObjectResult));
            Assert.AreEqual(0,topHiddenIndicators.Count);  
        }
    }
}