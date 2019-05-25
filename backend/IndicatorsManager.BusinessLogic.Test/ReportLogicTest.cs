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
    public class ReportLogicTest 
    {
        [TestMethod]
        public void GetUsersMostLogsTest()
        {
            var mockLog = new Mock<ILogQuery>(MockBehavior.Strict);
            mockLog.Setup(m => m.GetUsersMostLogs(It.IsAny<int>())).Returns(CreateUserData(10));

            var mockInd = new Mock<IIndicatorQuery>(MockBehavior.Strict);
            
            IReportLogic logic = new ReportLogic(mockLog.Object, mockInd.Object);
            IEnumerable<User> result = logic.GetUsersMostLogs(10);

            mockLog.VerifyAll();
            Assert.AreEqual(10, result.Count());
        }

        [TestMethod]
        public void GetMostHiddenIndicators()
        {
            var mockLog = new Mock<ILogQuery>(MockBehavior.Strict);            

            var mockInd = new Mock<IIndicatorQuery>(MockBehavior.Strict);
            mockInd.Setup(m => m.GetMostHiddenIndicators(It.IsAny<int>())).Returns(CreateIndicators(10));
            
            IReportLogic logic = new ReportLogic(mockLog.Object, mockInd.Object);
            IEnumerable<Indicator> result = logic.GetMostHiddenIndicators(10);

            mockLog.VerifyAll();
            Assert.AreEqual(10, result.Count());
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
