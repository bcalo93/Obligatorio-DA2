using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.Logger.Interface;

namespace IndicatorsManager.BusinessLogic.Test
{
    [TestClass]
    public class ReportLogicTest 
    {
        private Mock<ILogger> mockLogger;
        private Mock<IIndicatorQuery> mockIndicators;
        private Mock<IRepository<User>> mockUserRepository;
        private IReportLogic logic;

        [TestInitialize]
        public void InitMocks()
        {
            mockLogger = new Mock<ILogger>();
            mockIndicators = new Mock<IIndicatorQuery>();
            mockUserRepository = new Mock<IRepository<User>>();
            logic = new ReportLogic(mockLogger.Object, mockIndicators.Object, mockUserRepository.Object);
        }

        [TestCleanup]
        public void VerifyAll()
        {
            mockLogger.VerifyAll();
            mockIndicators.VerifyAll();
        }
        
        
        [TestMethod]
        public void GetMostLoggedInManagerTest()
        {
            List<User> allUsers = new List<User>(CreateUserData(10, 0, Role.Manager));
            allUsers.AddRange(CreateUserData(10, 10, Role.Admin));

            mockUserRepository.Setup(m => m.GetAll()).Returns(allUsers);
            mockLogger.Setup(m => m.GetMostLoggedInUsers())
                .Returns(CreateUsernameList(20));
            IEnumerable<User> result = logic.GetMostLoggedInManagers(10);
            Assert.AreEqual(10, result.Count());
            Assert.IsTrue(result.All(u => u.Role == Role.Manager));
        }

        [TestMethod]
        public void GetMostHiddenIndicators()
        {
            mockIndicators.Setup(m => m.GetMostHiddenIndicators(It.IsAny<int>()))
                .Returns(CreateIndicators(10));
            IEnumerable<Indicator> result = logic.GetMostHiddenIndicators(10);
            Assert.AreEqual(10, result.Count());
        }

        private IEnumerable<string> CreateUsernameList(int amount)
        {
            List<string> result = new List<string>();
            for(int i = 0; i < amount; i++)
            {
                result.Add("username " + i);
            }
            return result;
        }

        private List<User> CreateUserData(int amount, int offset, Role role)
        {
            List<User> result = new List<User>();
            for(int i = 0; i < amount; i++)
            {
                int dif = i + offset;
                result.Add(new User 
                {
                    Id = Guid.NewGuid(),
                    Name = "Name " + dif,
                    LastName = "Lastname" + dif,
                    Username = "username " + dif,
                    Password = "password " + dif,
                    Email = "mail" + dif + "@mail.com",
                    Role = role,
                    IsDeleted = false

                });
            }
            return result;
        }

        private IEnumerable<Indicator> CreateIndicators(int amount)
        {
            List<string> itemNames = new List<string> { "Red", "Yellow", "Green" };
            List<Indicator> result = new List<Indicator>();
            for(int i = 0; i < amount; i++)
            {
                Indicator indicator = new Indicator{ Name = "Test Indicator " + i, Area = new Area { Name = "Area" + i, DataSource = "Data Source" } };
                indicator.UserIndicators.Add(new UserIndicator { User = new User 
                                                                        { 
                                                                            Name = "Test User" + i, 
                                                                            LastName = "Test Surname", 
                                                                            Email = "mail@test.com",
                                                                            Username = "Username Test",
                                                                            Password = "Password Test",
                                                                            Role = Role.Manager
                                                                        }
                                                                } );                
                
                result.Add(indicator);
            }
            return result;
        }
    }
    
}
