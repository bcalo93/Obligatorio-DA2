using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Moq;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.BusinessLogic;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.Domain;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.BusinessLogic.Test
{
    [TestClass]
    public class IndicatorLogicTest
    {
        private Mock<IIndicatorQuery> mockQuery;
        private Mock<IRepository<Area>> mockArea;
        private Mock<IRepository<User>> mockUser;
        private Mock<IRepository<Indicator>> mockIndicator;
        private Mock<IQueryRunner> mockRunner;
        private Mock<ITokenRepository> mockToken;
        private IIndicatorLogic logic;

        [TestInitialize]
        public void InitMocks()
        {
            mockQuery = new Mock<IIndicatorQuery>(MockBehavior.Strict);
            mockArea = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockUser = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockIndicator = new Mock<IRepository<Indicator>>(MockBehavior.Strict);
            mockRunner = new Mock<IQueryRunner>(MockBehavior.Strict);
            mockToken = new Mock<ITokenRepository>(MockBehavior.Strict);
            logic = new IndicatorLogic(mockIndicator.Object, mockArea.Object, mockUser.Object, mockQuery.Object,
                mockRunner.Object, mockToken.Object); 
        }

        [TestCleanup]
        public void VerifyAllMocks()
        {
            mockQuery.VerifyAll();
            mockArea.VerifyAll();
            mockIndicator.VerifyAll();
            mockToken.VerifyAll();
        }
        
        [TestMethod]
        public void CreateIndicatorOkTest()
        {
            Guid areaId = Guid.NewGuid();
            Area expectedArea = new Area { Id = areaId, Name = "Test Area" };

            Indicator create = new Indicator{  Name = "Test Create" };
            create.IndicatorItems.Add(new IndicatorItem { Name = "Red", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });
            create.IndicatorItems.Add(new IndicatorItem { Name = "Yellow", Condition = new OrCondition{ 
                Components = CreateConditionTestList() } });
            create.IndicatorItems.Add(new IndicatorItem { Name = "Green", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });
            
            mockArea.Setup(m => m.Get(areaId)).Returns(expectedArea);
            mockArea.Setup(m => m.Save());

            Indicator result = logic.Create(areaId, create);

            Assert.AreEqual(create.Id, result.Id);
            Assert.AreEqual(create.Name, result.Name);
            Assert.AreEqual(3, result.IndicatorItems.Count);
            Assert.IsTrue(result.IndicatorItems.Any(ii => ii.Name == "Red"));
            Assert.IsTrue(result.IndicatorItems.Any(ii => ii.Name == "Yellow"));
            Assert.IsTrue(result.IndicatorItems.Any(ii => ii.Name == "Green"));
            Assert.IsTrue(expectedArea.Indicators.Any(i => i.Name == result.Name));
        }

        [TestMethod]
        public void CreateIndicatorNoIndicatorItemsOkTest()
        {
            Guid areaId = Guid.NewGuid();
            Area expectedArea = new Area { Id = areaId, Name = "Test Area" };

            Indicator create = new Indicator{  Name = "Test Create" };

            mockArea.Setup(m => m.Get(areaId)).Returns(expectedArea);
            mockArea.Setup(m => m.Save());

            Indicator result = logic.Create(areaId, create);

            Assert.AreEqual(create.Id, result.Id);
            Assert.AreEqual(create.Name, result.Name);
            Assert.AreEqual(0, result.IndicatorItems.Count);
            Assert.IsTrue(expectedArea.Indicators.Any(i => i.Name == result.Name));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateIndicatorNameEmptyTest()
        {
            Guid areaId = Guid.NewGuid();
            Area expectedArea = new Area { Id = areaId, Name = "Test Area" };

            Indicator create = new Indicator{  Name = "" };

            Indicator result = logic.Create(areaId, create);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateIndicatorToManyIndicatorItemsTest()
        {
            Guid areaId = Guid.NewGuid();
            Area expectedArea = new Area { Id = areaId, Name = "Test Area" };

            Indicator create = new Indicator{  Name = "Test Indicator with many IndicatorItems" };
            create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "Red", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });
            create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "Yellow", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });
            create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "Green", Condition = new OrCondition{ 
                Components = CreateConditionTestList() } });
            create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "Violet", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });

            Indicator result = logic.Create(areaId, create);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateIndicatorWithRepeatedIndicatorItemsTest()
        {
            Guid areaId = Guid.NewGuid();
            Area expectedArea = new Area { Id = areaId, Name = "Test Area" };

            Indicator create = new Indicator{  Name = "Test Indicator with many IndicatorItems" };
            create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "Red", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });
            create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "Yellow", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });
            create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "Yellow", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });

            Indicator result = logic.Create(areaId, create);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateIndicatorIndicatorItemWithEmptyStringTest()
        {
            Guid areaId = Guid.NewGuid();
            Area expectedArea = new Area { Id = areaId, Name = "Test Area" };

            Indicator create = new Indicator{  Name = "Test Indicator with many IndicatorItems" };
            create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "Red", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });
            create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "Yellow", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });
            create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });

            Indicator result = logic.Create(areaId, create);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateIndicatorConditionWithTextElementsNullTest()
        {
            Guid areaId = Guid.NewGuid();
            Area expectedArea = new Area { Id = areaId, Name = "Test Area" };

            Indicator create = new Indicator{  Name = "Test Indicator Wrong Text at Indicator Item" };
            
            IndicatorItem wrongItem = new IndicatorItem{ Name ="Green", Condition = new MayorEqualsCondition{ Components = new List<Component> 
            { 
                new ItemNumeric { Position = 1, NumberValue = 0 },
                new ItemText { Position = 2, TextValue = null }
            }}};

            create.IndicatorItems.Add(wrongItem);
        
            Indicator result = logic.Create(areaId, create);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateIndicatorConditionWithQueryElementsNullTest()
        {
            Guid areaId = Guid.NewGuid();
            Area expectedArea = new Area { Id = areaId, Name = "Test Area" };

            Indicator create = new Indicator{  Name = "Test Indicator Wrong Query at Indicator Item" };
            
            IndicatorItem wrongItem = new IndicatorItem{ Name ="Red", Condition = new MinorCondition{ Components = new List<Component> 
            { 
                new ItemQuery { Position = 1, QueryTextValue = null },
                new ItemText { Position = 2, TextValue = "Value" }
            }}};

            create.IndicatorItems.Add(wrongItem);

            Indicator result = logic.Create(areaId, create);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateIndicatorConditionWithQueryElementsEmptyTest()
        {
            Guid areaId = Guid.NewGuid();
            Area expectedArea = new Area { Id = areaId, Name = "Test Area" };

            Indicator create = new Indicator{  Name = "Test Indicator Wrong Query at Indicator Item" };
            
            IndicatorItem wrongItem = new IndicatorItem{ Name ="Red", Condition = new MinorEqualsCondition{ Components = new List<Component> 
            { 
                new ItemQuery { Position = 1, QueryTextValue = "" },
                new ItemText { Position = 2, TextValue = "Value" }
            }}};

            create.IndicatorItems.Add(wrongItem);
        
            Indicator result = logic.Create(areaId, create);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateIndicatorNumericConditionWithToManyElementsTest()
        {
            Guid areaId = Guid.NewGuid();
            Area expectedArea = new Area { Id = areaId, Name = "Test Area" };

            Indicator create = new Indicator{  Name = "Test Indicator with wrong formed Numeric Condition" };
            
            IndicatorItem wrongItem = new IndicatorItem{ Name ="Red", Condition = new MinorCondition{ Components = new List<Component> 
            { 
                new ItemQuery { Position = 1, QueryTextValue = "SELECT FROM TABLE" },
                new ItemText { Position = 2, TextValue = "Value" },
                new ItemNumeric { Position = 3, NumberValue = 20 }
            }}};

            create.IndicatorItems.Add(wrongItem);

            Indicator result = logic.Create(areaId, create);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateIndicatorConditionPositionRepeatedTest()
        {
            Guid areaId = Guid.NewGuid();
            Area expectedArea = new Area { Id = areaId, Name = "Test Area" };

            Indicator create = new Indicator{  Name = "Test Indicator with position repeated" };
            
            IndicatorItem wrongItem = new IndicatorItem{ Name ="Red", Condition = new MinorCondition{ Components = new List<Component> 
            { 
                new ItemQuery { Position = 1, QueryTextValue = "SELECT FROM TABLE" },
                new ItemText { Position = 1, TextValue = "Value" }
            }}};

            create.IndicatorItems.Add(wrongItem);

            Indicator result = logic.Create(areaId, create);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotExistException))]
        public void CreateIndicatorAreaNotExistTest()
        {
            Guid areaId = Guid.NewGuid();

            Indicator create = new Indicator{  Name = "Test Indicator with many IndicatorItems" };

            mockArea.Setup(m => m.Get(areaId)).Returns<IEnumerable<Area>>(null);

            Indicator result = logic.Create(areaId, create);
        }

        [TestMethod]
        public void GetIndicatorResultOkTest()
        {
            Guid indicatorId  = Guid.NewGuid();
            string queryString = "SELECT COUNT(*) FROM TABLE";

            //Data Init
            Area area = new Area{ Name = "Test Area", DataSource = "DataSource" }; 

            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 30 };
            MayorCondition mayor = new MayorCondition { Components = new List<Component> { number, query } };
            
            Indicator toGet = new Indicator { Name = "Indicator Test" };
            toGet.Area = area;
            toGet.IndicatorItems.Add(new IndicatorItem { Name = "Red", Condition = mayor } );

            mockIndicator.Setup(m => m.Get(indicatorId)).Returns(toGet);
            mockRunner.Setup(m => m.SetConnectionString(It.IsAny<string>()));
            mockRunner.Setup(m => m.RunQuery(queryString)).Returns(31);

            IndicatorResult result = logic.Get(indicatorId);
            Assert.AreEqual("Indicator Test", result.Indicator.Name);
            Assert.AreEqual(1, result.ItemsResults.Count());
            IndicatorItemResult itemResult = result.ItemsResults.First();
            Assert.AreEqual("Red", itemResult.IndicatorItem.Name);
            Assert.IsTrue((bool)itemResult.Result.ConditionResult);
            Assert.AreEqual("(31 > 30)", itemResult.Result.ConditionToString);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public void GetIndicatorResultEvaluationExceptionTest()
        {
            Guid indicatorId  = Guid.NewGuid();
            string queryString = "SELECT COUNT(*) FROM TABLE";

            //Data Init
            Area area = new Area{ Name = "Test Area", DataSource = "DataSource" }; 

            ItemQuery query = new ItemQuery { Position = 1, QueryTextValue = queryString};
            ItemNumeric number = new ItemNumeric { Position = 2, NumberValue = 200 };
            MayorCondition mayor = new MayorCondition { Components = new List<Component> { number, query } };
            
            Indicator toGet = new Indicator { Name = "Indicator Test" };
            toGet.Area = area;
            toGet.IndicatorItems.Add(new IndicatorItem { Name = "Red", Condition = mayor } );

            mockIndicator.Setup(m => m.Get(indicatorId)).Returns(toGet);
            mockRunner.Setup(m => m.SetConnectionString(It.IsAny<string>()));
            mockRunner.Setup(m => m.RunQuery(queryString)).Throws(new EvaluationException("An exception has occurred."));

            IndicatorResult result = logic.Get(indicatorId);
            Assert.AreEqual("Indicator Test", result.Indicator.Name);
            Assert.AreEqual(1, result.ItemsResults.Count());
            IndicatorItemResult itemResult = result.ItemsResults.First();
            Assert.AreEqual("Red", itemResult.IndicatorItem.Name);
            Assert.AreEqual("An exception has occurred.", (string)itemResult.Result.ConditionResult);
            Assert.AreEqual("(La consulta SELECT COUNT(*) FROM TABLE es incorrecta. > 200)", itemResult.Result.ConditionToString);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotExistException))]
        public void GetIndicatorResutIndicatorNotExistTest()
        {
            Guid indicatorId  = Guid.NewGuid();

            mockIndicator.Setup(m => m.Get(indicatorId)).Returns<Indicator>(null);

            IndicatorResult result = logic.Get(indicatorId);
        }

        [TestMethod]
        public void GetAllIndicatorByAreaIdOkTest()
        {
            Guid areaId = Guid.NewGuid();
            Area areaResult = new Area { Id = areaId, Name = "Area Result" };

            List<Indicator> areaIndicators = new List<Indicator>();
            for(int i = 0; i < 10; i++)
            {
                Indicator create = new Indicator{ Id = Guid.NewGuid(), Name = "Test Create" + i };
                create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "Red", Condition = new OrCondition{ 
                Components = CreateConditionTestList() } });
                create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "Yellow", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });
                create.IndicatorItems.Add(new IndicatorItem { Id = Guid.NewGuid(), Name = "Green", Condition = new AndCondition{ 
                Components = CreateConditionTestList() } });
                areaIndicators.Add(create);
            }
            areaResult.Indicators = areaIndicators;
            mockArea.Setup(m => m.Get(areaId)).Returns(areaResult);

            IEnumerable<Indicator> result = logic.GetAll(areaId);

            Assert.AreEqual(10, result.Count());
            foreach (Indicator indicator in result)
            {
                Assert.IsTrue(areaIndicators.Any(i => i.Id == indicator.Id && i.Name == indicator.Name));
                Assert.AreEqual(3, indicator.IndicatorItems.Count);
                Assert.IsTrue(indicator.IndicatorItems.Any(ii => ii.Name == "Red"));
                Assert.IsTrue(indicator.IndicatorItems.Any(ii => ii.Name == "Yellow"));
                Assert.IsTrue(indicator.IndicatorItems.Any(ii => ii.Name == "Green"));
            }
        }

        [TestMethod]
        public void GetAllIndicatorsByAreaNoIndicatorsTest()
        {
            Guid areaId = Guid.NewGuid();
            Area areaResult = new Area { Id = areaId, Name = "Area Result" };

            mockArea.Setup(m => m.Get(areaId)).Returns(areaResult);

            IEnumerable<Indicator> result = logic.GetAll(areaId);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotExistException))]
        public void GetAllIndocatorsByAreaIdAreaNotExistTest()
        {
            Guid areaId = Guid.NewGuid();

            mockArea.Setup(m => m.Get(areaId)).Returns<IEnumerable<Area>>(null);

            IEnumerable<Indicator> result = logic.GetAll(areaId);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void GetAllIndicatorByAreaIdDataAccessExceptionTest()
        {
            Guid areaId = Guid.NewGuid();
            mockArea.Setup(m => m.Get(areaId)).Throws(new DataAccessException(""));

            IEnumerable<Indicator> result = logic.GetAll(areaId);
        }

        [TestMethod]
        public void UpdateIndicatorOkTest()
        {
            Guid itemId = Guid.NewGuid();
            Indicator getResult = new Indicator{ Id = itemId, Name = "Original Name" };
            Indicator update = new Indicator{ Name = "Indicator Updated" };

            mockIndicator.Setup(m => m.Get(itemId)).Returns(getResult);
            mockIndicator.Setup(m => m.Update(It.IsAny<Indicator>()));
            mockIndicator.Setup(m => m.Save());
            
            Indicator result = logic.Update(itemId, update);

            Assert.AreEqual(itemId, result.Id);
            Assert.AreEqual("Indicator Updated", result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotExistException))]
        public void UpdateIndicatorIndicatorNotFoundTest()
        {
            Guid itemId = Guid.NewGuid();
            Indicator update = new Indicator{ Name = "Indicator Updated" };

            mockIndicator.Setup(m => m.Get(itemId)).Returns<IEnumerable<Indicator>>(null);

            Indicator result = logic.Update(itemId, update);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void UpdateIndicatorNullTest()
        {
            Guid itemId = Guid.NewGuid();

            Indicator result = logic.Update(itemId, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void UpdateIndicatorWrongNameTest()
        {
            Guid itemId = Guid.NewGuid();
            Indicator update = new Indicator{ Name = "" };

            Indicator result = logic.Update(itemId, update);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void UpdateIndicatorDataAccessExceptionTest()
        {
            Guid itemId = Guid.NewGuid();
            Indicator update = new Indicator{ Name = "Name Changed" };

            mockIndicator.Setup(m => m.Get(itemId)).Throws(new DataAccessException(""));
            
            Indicator result = logic.Update(itemId, update);
        }

        [TestMethod]
        public void DeleteIndicatorOkTest()
        {
            Guid itemId = Guid.NewGuid();
            Indicator getResult = new Indicator{ Id = itemId, Name = "Indicator Test" };

            mockIndicator.Setup(m => m.Get(itemId)).Returns(getResult);
            mockIndicator.Setup(m => m.Remove(getResult));
            mockIndicator.Setup(m => m.Save());
            
            logic.Remove(itemId);
        }

        [TestMethod]
        public void DeleteIndicatorNotFoundTest()
        {
            Guid itemId = Guid.NewGuid();

            mockIndicator.Setup(m => m.Get(itemId)).Returns<IEnumerable<Indicator>>(null);
            
            logic.Remove(itemId);
        }

        [TestMethod]
        public void DeleteIndicatorDataAccessExceptionTest()
        {
            Guid itemId = Guid.NewGuid();

            mockIndicator.Setup(m => m.Get(itemId)).Returns<IEnumerable<Indicator>>(null);
            
            logic.Remove(itemId);
        }

        [TestMethod]
        public void GetManagerIndicatorsWithConfigurationOkTest()
        {
            Guid userId = Guid.NewGuid();
            List<Indicator> getResult = new List<Indicator>();
            for (int i = 0; i < 10; i++)
            {
                getResult.Add(new Indicator
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Test Get Indicator " + i,
                    UserIndicators = new List<UserIndicator> 
                    {
                        new UserIndicator { UserId = userId, Position = i, IsVisible = false } 
                    } 
                });
            }
            mockToken.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(
                new AuthenticationToken { Id = Guid.NewGuid(), User = new User{ Id = userId, Role = Role.Manager }});
            mockQuery.Setup(m => m.GetManagerIndicators(userId)).Returns(getResult);
            
            IEnumerable<IndicatorConfiguration> result = logic.GetManagerIndicators(Guid.NewGuid());
            
            int currentPosition = 0;
            foreach (IndicatorConfiguration config in result)
            {
                Assert.IsTrue(getResult.Any(i => i.Id == config.Indicator.Id && 
                    i.Name == config.Indicator.Name && i.UserIndicators.Single().Position == config.Position));
                Assert.AreEqual(currentPosition, config.Position);
                Assert.IsFalse(config.IsVisible);
                currentPosition++;
            }
        }

        [TestMethod]
        public void GetManagerIndicatorsWithoutConfigurationOkTest()
        {
            Guid userId = Guid.NewGuid();
            List<Indicator> getResult = new List<Indicator>();
            for (int i = 0; i < 10; i++)
            {
                getResult.Add(new Indicator
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Test Get Indicator " + i
                });
            }
            mockToken.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(
                new AuthenticationToken { Id = Guid.NewGuid(), User = new User{ Id = userId, Role = Role.Manager }});
            mockQuery.Setup(m => m.GetManagerIndicators(userId)).Returns(getResult);
            
            IEnumerable<IndicatorConfiguration> result = logic.GetManagerIndicators(Guid.NewGuid());
            
            foreach (IndicatorConfiguration config in result)
            {
                Assert.IsTrue(getResult.Any(i => i.Id == config.Indicator.Id && 
                    i.Name == config.Indicator.Name));
                Assert.IsNull(config.Position);
                Assert.IsTrue(config.IsVisible);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void GetManagerIndicatorsDataAccessExceptionTest()
        {
            Guid userId = Guid.NewGuid();
            mockToken.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(
                new AuthenticationToken { Id = Guid.NewGuid(), User = new User{ Id = userId, Role = Role.Manager }});
            mockQuery.Setup(m => m.GetManagerIndicators(userId)).Throws(new DataAccessException(""));

            logic.GetManagerIndicators(userId);
        }

        [TestMethod]
        public void GetManagerActiveIndicatorsOkTest()
        {
            Guid userId = Guid.NewGuid();
            
            // Data Init
            mockToken.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(
                new AuthenticationToken { Id = Guid.NewGuid(), User = new User{ Id = userId, Role = Role.Manager }});
            mockQuery.Setup(m => m.GetManagerIndicators(userId))
                .Returns(CreateSimpleIndicatorData(userId, 3, true));
            mockRunner.Setup(m => m.SetConnectionString(It.IsAny<string>()));
            mockRunner.SetupSequence(m => m.RunQuery(It.IsAny<string>()))
                .Returns(10)
                .Returns(5)
                .Returns(3)
                .Returns(5)
                .Returns(3)
                .Returns(5)
                .Returns(1)
                .Returns("Test")
                .Returns(true);

            IEnumerable<ActiveIndicator> result = logic.GetManagerActiveIndicators(Guid.NewGuid());
            Assert.AreEqual(2, result.Count());
            ActiveIndicator indicator1 = result.ElementAt(0);
            Assert.AreEqual("Test Indicator 0", indicator1.Indicator.Name);
            Assert.AreEqual(1, indicator1.ActiveItems.Count());
            Assert.IsTrue(indicator1.ActiveItems.Any(i => i.Name == "Yellow"));
            Assert.IsTrue(indicator1.IsVisible);
            Assert.AreEqual("Test Alias", indicator1.Alias);

            ActiveIndicator indicator2 = result.ElementAt(1);
            Assert.AreEqual("Test Indicator 1", indicator2.Indicator.Name);
            Assert.AreEqual(2, indicator2.ActiveItems.Count());
            Assert.IsTrue(indicator2.ActiveItems.Any(i => i.Name == "Red"));
            Assert.IsTrue(indicator2.ActiveItems.Any(i => i.Name == "Green"));
            Assert.IsTrue(indicator2.IsVisible);
            Assert.AreEqual("Test Alias", indicator2.Alias);
        }

        [TestMethod]
        public void GetManagerActiveIndicatorsEvaluationExceptionTest()
        {
            Guid userId = Guid.NewGuid();
            
            // Data Init
            mockToken.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(
                new AuthenticationToken { Id = Guid.NewGuid(), User = new User{ Id = userId, Role = Role.Manager }});
            mockQuery.Setup(m => m.GetManagerIndicators(userId))
                .Returns(CreateSimpleIndicatorData(userId, 1, true));
            mockRunner.Setup(m => m.SetConnectionString(It.IsAny<string>()));
            mockRunner.SetupSequence(m => m.RunQuery(It.IsAny<string>()))
                .Throws(new EvaluationException(""))
                .Returns(10)
                .Returns(5);

            IEnumerable<ActiveIndicator> result = logic.GetManagerActiveIndicators(Guid.NewGuid());
            Assert.AreEqual(1, result.Count());
            ActiveIndicator indicator = result.Single();
            Assert.AreEqual("Test Indicator 0", indicator.Indicator.Name);
            Assert.AreEqual(1, indicator.ActiveItems.Count());
            Assert.IsTrue(indicator.ActiveItems.Any(i => i.Name == "Green"));
            Assert.IsTrue(indicator.IsVisible);
            Assert.AreEqual("Test Alias", indicator.Alias);
        }

        [TestMethod]
        public void GetManagerActiveIndicatorsNoConfigurationTest()
        {
            Guid userId = Guid.NewGuid();
            
            // Data Init
            mockToken.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(
                new AuthenticationToken { Id = Guid.NewGuid(), User = new User{ Id = userId, Role = Role.Manager }});
            mockQuery.Setup(m => m.GetManagerIndicators(userId))
                .Returns(CreateSimpleIndicatorData(1));
            mockRunner.Setup(m => m.SetConnectionString(It.IsAny<string>()));
            mockRunner.SetupSequence(m => m.RunQuery(It.IsAny<string>()))
                .Returns(6)
                .Returns(10)
                .Returns(5);

            IEnumerable<ActiveIndicator> result = logic.GetManagerActiveIndicators(Guid.NewGuid());
            Assert.AreEqual(1, result.Count());
            ActiveIndicator indicator = result.Single();
            Assert.AreEqual("Test Indicator 0", indicator.Indicator.Name);
            Assert.AreEqual(1, indicator.ActiveItems.Count());
            Assert.IsTrue(indicator.ActiveItems.Any(i => i.Name == "Green"));
            Assert.IsNull(indicator.Position);
            Assert.IsTrue(indicator.IsVisible);
        }

        [TestMethod]
        public void GetManagerActiveIndicatorsVisibleFalseTest()
        {
            Guid userId = Guid.NewGuid();
            
            mockToken.Setup(m => m.GetByToken(It.IsAny<Guid>())).Returns(
                new AuthenticationToken { Id = Guid.NewGuid(), User = new User{ Id = userId, Role = Role.Manager }});
            mockQuery.Setup(m => m.GetManagerIndicators(userId))
                .Returns(CreateSimpleIndicatorData(userId, 3, false));

            IEnumerable<ActiveIndicator> result = logic.GetManagerActiveIndicators(Guid.NewGuid());
            Assert.AreEqual(0, result.Count());
        }
        
        private List<Component> CreateConditionTestList()
        {
            ItemNumeric numeric = new ItemNumeric{ Id = Guid.NewGuid(), Position = 1, NumberValue = 5 };
            ItemQuery query1 = new ItemQuery{ Id = Guid.NewGuid(), Position = 2, QueryTextValue = "SELECT MAX x FROM TABLE" };
            Condition condition1 = new EqualsCondition{ Id = Guid.NewGuid(), Position = 1, Components = new List<Component> { numeric, query1 } };
            
            ItemQuery query2 = new ItemQuery{ Id = Guid.NewGuid(), Position = 1, QueryTextValue = "SELECT MIN x FROM TABLE" };
            ItemText text1 = new ItemText{ Id = Guid.NewGuid(), Position = 2, TextValue = "Test Texto" };
            Condition condition2 = new MinorCondition{ Id = Guid.NewGuid(), Position = 2, Components = new List<Component> { text1 , query2 } };

            ItemQuery query3 = new ItemQuery{ Id = Guid.NewGuid(), Position = 1, QueryTextValue = "SELECT MIN x FROM TABLE" };
            ItemText text2 = new ItemText{ Id = Guid.NewGuid(), Position = 2, TextValue = "Test Texto" };
            Condition condition3 = new MinorCondition{ Id = Guid.NewGuid(), Position = 3, Components = new List<Component> { text2, query3 } };
            return new List<Component> { condition1, condition2, condition3};
        }

        private IEnumerable<Indicator> CreateSimpleIndicatorData(Guid userId, int amount, bool isVisible)
        {
            List<string> itemNames = new List<string> { "Red", "Yellow", "Green" };
            List<Indicator> result = new List<Indicator>();
            for(int i = 0; i < amount; i++)
            {
                Indicator indicator = new Indicator{ Name = "Test Indicator " + i, 
                    Area = new Area { Id = Guid.NewGuid(), Name = "Test Area", DataSource = "Data Source" } };
                User user = CreateUser(userId, i);
                indicator.UserIndicators.Add(new UserIndicator { UserId = user.Id, 
                    User = user, IsVisible = isVisible, Alias = "Test Alias" });
                foreach (string color in itemNames)
                {
                    ItemNumeric numeric = new ItemNumeric{ Position = 1, NumberValue = 5 };
                    ItemQuery query1 = new ItemQuery{ Position = 2, QueryTextValue = "SELECT MAX x FROM TABLE" };
                    Condition condition = new EqualsCondition{ Position = 1, Components = new List<Component> { numeric, query1 } };
                    indicator.IndicatorItems.Add(new IndicatorItem { Name = color, Condition = condition });
                }
                result.Add(indicator);
            }
            return result;
        }

        private IEnumerable<Indicator> CreateSimpleIndicatorData(int amount)
        {
            List<string> itemNames = new List<string> { "Red", "Yellow", "Green" };
            List<Indicator> result = new List<Indicator>();
            for(int i = 0; i < amount; i++)
            {
                Indicator indicator = new Indicator{ Name = "Test Indicator " + i, 
                    Area = new Area { Id = Guid.NewGuid(), Name = "Test Area", DataSource = "Data Source" } };
                foreach (string color in itemNames)
                {
                    ItemNumeric numeric = new ItemNumeric{ Position = 1, NumberValue = 5 };
                    ItemQuery query1 = new ItemQuery{ Position = 2, QueryTextValue = "SELECT MAX x FROM TABLE" };
                    Condition condition = new EqualsCondition{ Position = 1, Components = new List<Component> { numeric, query1 } };
                    indicator.IndicatorItems.Add(new IndicatorItem { Name = color, Condition = condition });
                }
                result.Add(indicator);
            }
            return result;
        }

        private User CreateUser(Guid userId, int i) 
        {
            User create = new User
            {
                Id = userId,
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