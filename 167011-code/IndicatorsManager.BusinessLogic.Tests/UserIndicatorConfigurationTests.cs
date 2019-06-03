using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using IndicatorsManager.BusinessLogic;
using IndicatorsManager.BusinessLogic.Exceptions;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface;
using System;
using IndicatorsManager.BusinessLogic.Interface;

namespace IndicatorsManager.BusinessLogic.Tests
{
    [TestClass]
    public class UserUserIndicatorTests
    {
        private Guid USER_ID = Guid.NewGuid();
        private Guid USER_INVALID_ID = Guid.Empty;
        private const string USER_NAME = "Ramiro";
        private const string USER_LASTNAME = "Gonzalez Coitiño";
        private const string USER_USERNAME = "Username";
        private const string USER_PASSWORD = "Password";
        private const string USER_EMAIL = "ramirogc21@gmail.com";
        private const string USER_INVALID_EMAIL = "ramirogc21__gmail.com";
        private const string USER_AREANAME = "Area 1";
        private const string USER_UPDATED_NAME = "Raul";
        
        private Guid INDICATOR_ID_1 = Guid.NewGuid();
        private Guid INDICATOR_ID_2 = Guid.NewGuid();

    
        private Mock<ILogic<User>> mockUserService;
        private Mock<ILogic<Indicator>> mockIndicatorSerivce;
        private Mock<IConfigurationRepository> mockIndicatorConfigRepository;


        [TestInitialize]
        public void Setup()
        {
            mockUserService = new Mock<ILogic<User>>(MockBehavior.Strict);
            mockIndicatorSerivce = new Mock<ILogic<Indicator>>(MockBehavior.Strict);
            mockIndicatorConfigRepository = new Mock<IConfigurationRepository>(MockBehavior.Strict);
        }

        public User CreateManager()
        {
            var user = new User()
            { 
                Name = USER_NAME,
                LastName = USER_LASTNAME,
                UserName = USER_USERNAME,
                Password = USER_PASSWORD,    
                Email = USER_EMAIL,
                Role = UserRole.MANAGER,
            };            
            return user;
        }
        
        public UserIndicator CreateConfiguration_1() => new UserIndicator()
        { 
            IsVisible = true, 
            Position = 1, 
            IndicatorId = INDICATOR_ID_1
        };
        public UserIndicator CreateConfiguration_2() => new UserIndicator()
        { 
            IsVisible = true, 
            Position = 2, 
            IndicatorId = INDICATOR_ID_2

        };
        public UserIndicator CreateConfiguration_2_Updated() => new UserIndicator()
        { 
            IsVisible = false, 
            Position = 2, 
            IndicatorId = INDICATOR_ID_2
        };

        [TestMethod]
        public void AddUserIndicatorToManagerTest()
        {
            var newConfig = CreateConfiguration_1();
            List<UserIndicator> userConfigurations = new List<UserIndicator>(){ newConfig };
            
            var manager = CreateManager();
            manager.IndicatorConfigurations.AddRange(userConfigurations);

            mockUserService.Setup(m => m.ExistCondition(It.IsAny<Func<User,bool>>())).Returns(true);
            mockIndicatorSerivce.Setup(m => m.ExistCondition(It.IsAny<Func<Indicator,bool>>())).Returns(true);
            mockUserService.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<User>())).Returns(manager);

            var userConfigLogic = new UserConfigurationLogic(mockUserService.Object, mockIndicatorSerivce.Object, mockIndicatorConfigRepository.Object);

            var updatedUser = userConfigLogic.AddConfiguration(USER_ID, userConfigurations);
           
            mockUserService.VerifyAll();
            mockIndicatorSerivce.VerifyAll();
            mockIndicatorConfigRepository.VerifyAll();

            Assert.AreEqual(manager.IndicatorConfigurations, updatedUser.IndicatorConfigurations);
        }


        [TestMethod]
        public void UpdateConfigurationToManager()
        {
            var config_1 = CreateConfiguration_1();
            var config_2 = CreateConfiguration_2();
            var config_2_updated = CreateConfiguration_2_Updated();
            List<UserIndicator> userConfigurations = new List<UserIndicator>(){ config_1, config_2 };
            List<UserIndicator> configsUpdated = new List<UserIndicator>(){ config_1, config_2_updated };

            var manager = CreateManager();
            manager.IndicatorConfigurations.AddRange(userConfigurations);
            
            var managerUpdated = CreateManager();
            managerUpdated.IndicatorConfigurations.AddRange(configsUpdated);

            mockUserService.Setup(m => m.ExistCondition(It.IsAny<Func<User,bool>>())).Returns(true);
            mockIndicatorConfigRepository.Setup(m => m.ExistCondition(It.IsAny<Func<UserIndicator,bool>>())).Returns(true);
            mockUserService.Setup(m => m.Get(It.IsAny<Guid>())).Returns(manager);
            mockUserService.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<User>())).Returns(managerUpdated);
            
            var userConfigLogic = new UserConfigurationLogic(mockUserService.Object, mockIndicatorSerivce.Object, mockIndicatorConfigRepository.Object);

            var updatedUser = userConfigLogic.UpdateConfiguration(USER_ID,config_2_updated);
           
            mockUserService.VerifyAll();
            mockIndicatorSerivce.VerifyAll();
            mockIndicatorConfigRepository.VerifyAll();

            Assert.AreNotEqual(
                manager.IndicatorConfigurations.Find(x => x.IndicatorId == INDICATOR_ID_2), 
                managerUpdated.IndicatorConfigurations.Find(x => x.IndicatorId == INDICATOR_ID_2)
            );
        }

        [TestMethod]
        public void GetTopHiddenIndicators()
        {
            int limitQueryItems = 10; 

            List<Indicator> queryResult = new List<Indicator>();
            mockIndicatorConfigRepository.Setup(m => m.GetTopHiddenIndicators(It.IsAny<int>())).Returns(queryResult);
            
            var userConfigLogic = new UserConfigurationLogic(mockUserService.Object, mockIndicatorSerivce.Object, mockIndicatorConfigRepository.Object);

            var result = userConfigLogic.GetTopHiddenIndicators(limitQueryItems);
           
            mockUserService.VerifyAll();
            mockIndicatorSerivce.VerifyAll();
            mockIndicatorConfigRepository.VerifyAll();

            Assert.IsNotNull(result);
        }
    }
}
