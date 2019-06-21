using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.DataAccess.Test
{
    [TestClass]
    public class IndicatorRepositoryTest : BaseTest
    {
        private Area testArea = new Area { Name = "Test Area", DataSource = "Test Data Source" };
        private User userTest = new User 
        { 
            Name = "Test User", 
            LastName = "Test Surname", 
            Email = "mail@test.com",
            Username = "Username Test",
            Password = "Password Test",
            Role = Role.Manager
        };

        [TestMethod]
        public void CreateIndicatorOkTest()
        {
            // Init data
            ItemNumeric numeric = new ItemNumeric{ Position = 1, NumberValue = 2 };
            ItemQuery query = new ItemQuery{ Position = 2, QueryTextValue = "SELECT COUNT(*) FROM TABLE" };
            Condition condition = new EqualsCondition{ Components = new List<Component>{ numeric, query } };
            IndicatorItem item = new IndicatorItem { Name = "Red", Condition = condition };
            Indicator indicator = new Indicator{ Name = "Indicator Test", Area = testArea };
            indicator.IndicatorItems.Add(item);

            IRepository<Indicator> repositoryCreate = new IndicatorRepository(CreateContext(dbName));
            repositoryCreate.Add(indicator);
            repositoryCreate.Save();

            IRepository<Indicator> repositoryGet = new IndicatorRepository(CreateContext(dbName));
            Indicator result = repositoryGet.Get(indicator.Id);
            Assert.AreEqual("Indicator Test", result.Name);
            Assert.AreEqual("Test Area", result.Area.Name);
            Assert.AreEqual(1, result.IndicatorItems.Count);
            
            IndicatorItem itemResult = result.IndicatorItems.ElementAt(0);
            Assert.AreEqual("Red", itemResult.Name);
            
            EqualsCondition conditionResult =  itemResult.Condition as EqualsCondition;
            Assert.AreEqual(0, conditionResult.Position);
            ItemNumeric numericResult = conditionResult.Components.Single(c => c.Position == 1) as ItemNumeric;
            Assert.AreEqual(2, numericResult.NumberValue);
            ItemQuery queryResult = conditionResult.Components.Single(c => c.Position == 2) as ItemQuery;
            Assert.AreEqual("SELECT COUNT(*) FROM TABLE", queryResult.QueryTextValue);
            
        }

        [TestMethod]
        public void CreateIndicatorMultipleConditionLevelsOkTest()
        {
            // Init data
            Indicator indicator = new Indicator{ Name = "Indicator Test", Area = testArea };
            
            ItemNumeric numeric = new ItemNumeric{ Position = 1, NumberValue = 5 };
            ItemQuery query1 = new ItemQuery{ Position = 2, QueryTextValue = "SELECT MAX x FROM TABLE" };
            Condition condition1 = new EqualsCondition{ Position = 1, Components = new List<Component>{ numeric, query1 } };
            
            ItemQuery query2 = new ItemQuery{ Position = 2, QueryTextValue = "SELECT MIN x FROM TABLE" };
            ItemText text = new ItemText{ Position = 1, TextValue = "Test Texto" };
            Condition condition2 = new MinorCondition{ Position = 2, Components = new List<Component>{ text, query2 } };

            Condition condition3 = new AndCondition { Components = new List<Component>{ condition1, condition2 } };
            indicator.IndicatorItems.Add(new IndicatorItem { Name = "Yellow", Condition = condition3 });

            IRepository<Indicator> repository = new IndicatorRepository(CreateContext(dbName));
            repository.Add(indicator);
            repository.Save();

            IRepository<Indicator> repository2 = new IndicatorRepository(CreateContext(dbName));
            Indicator result = repository2.Get(indicator.Id);
            Assert.AreEqual("Indicator Test", result.Name);
            Assert.AreEqual("Test Area", result.Area.Name);
            Assert.AreEqual(1, result.IndicatorItems.Count);
            
            IndicatorItem itemResult = result.IndicatorItems.ElementAt(0);
            Assert.AreEqual("Yellow", itemResult.Name);
            
            AndCondition conditionResult =  itemResult.Condition as AndCondition;
            Assert.AreEqual(0, conditionResult.Position);

            EqualsCondition equalsResult = conditionResult.Components.Single(c => c.Position == 1) as EqualsCondition;
            ItemNumeric numericResult = equalsResult.Components.Single(c => c.Position == 1) as ItemNumeric;
            Assert.AreEqual(5, numericResult.NumberValue);
            ItemQuery queryResult1 = equalsResult.Components.Single(c => c.Position == 2) as ItemQuery;
            Assert.AreEqual("SELECT MAX x FROM TABLE", queryResult1.QueryTextValue);

            MinorCondition minorResult = conditionResult.Components.Single(c => c.Position == 2) as MinorCondition;
            ItemText textResult = minorResult.Components.Single(c => c.Position == 1) as ItemText;
            Assert.AreEqual("Test Texto", textResult.TextValue); 
            ItemQuery queryResult = minorResult.Components.Single(c => c.Position == 2) as ItemQuery;
            Assert.AreEqual("SELECT MIN x FROM TABLE", queryResult.QueryTextValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateIndicatorNullTest()
        {
            IRepository<Indicator> repository = new IndicatorRepository(CreateContext(dbName));
            repository.Add(null); 
        }
        
        [TestMethod]
        public void GetIndicatorIdNotExistTest()
        {
            IRepository<Indicator> repository = new IndicatorRepository(CreateContext(dbName));
            repository.Get(Guid.NewGuid());
        }

        [TestMethod]
        public void GetAllIndicatorOkTest()
        {
            IEnumerable<Indicator> data = CreateData(20);
            IRepository<Indicator> repository = new IndicatorRepository(CreateContext(dbName));
            
            IEnumerable<Indicator> result = repository.GetAll();
            
            Assert.AreEqual(20, result.Count());
            Assert.IsTrue(result.All(i => i.Area.Name == testArea.Name));
            Assert.IsTrue(result.All(i => i.UserIndicators.All(ui => ui.User.Username == "Username Test")));

            foreach (Indicator indicator in result)
            {
                Assert.IsTrue(data.Any(i => i.Name == indicator.Name && i.Id == indicator.Id));
                Assert.AreEqual(3, indicator.IndicatorItems.Count);
                Assert.IsTrue(indicator.IndicatorItems.Any(ii => ii.Name == "Red"));
                Assert.IsTrue(indicator.IndicatorItems.Any(ii => ii.Name == "Yellow"));
                Assert.IsTrue(indicator.IndicatorItems.Any(ii => ii.Name == "Green"));
            }
        }

        [TestMethod]
        public void GetAllIndicatorsNoIndicatorsTest() 
        {
            IRepository<Indicator> repository = new IndicatorRepository(CreateContext(dbName));

            IEnumerable<Indicator> result = repository.GetAll();
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void DeleteIndicatorOkTest() 
        {
            Indicator data = CreateData(1).Single();

            IRepository<Indicator> deleteRepo = new IndicatorRepository(CreateContext(dbName));
            Indicator toDelete = deleteRepo.Get(data.Id);

            deleteRepo.Remove(toDelete);
            deleteRepo.Save();

            //DbContext checkContext = CreateContext(dbName);
            IRepository<Indicator> checkResult = new IndicatorRepository(Context);
            IEnumerable<Indicator> result = checkResult.GetAll();
            Assert.AreEqual(0, result.Count());
            
            
            IEnumerable<IndicatorItem> items = Context.Set<IndicatorItem>().ToList();
            Assert.AreEqual(0, items.Count());
            IEnumerable<Condition> conditions = Context.Set<Condition>().ToList();
            Assert.AreEqual(0, conditions.Count());
            Assert.AreEqual(0, Context.Set<UserIndicator>().Count());
            Assert.IsTrue(Context.Set<User>().Any(u => u.Id == userTest.Id));

        }

        [TestMethod]
        public void DeleteIndicatorWithOtherIndicatorsOkTest() 
        {
            Indicator data = CreateData(20).ElementAt(10);

            IRepository<Indicator> deleteRepo = new IndicatorRepository(CreateContext(dbName));
            Indicator toDelete = deleteRepo.Get(data.Id);

            deleteRepo.Remove(toDelete);
            deleteRepo.Save();

            IRepository<Indicator> checkResult = new IndicatorRepository(CreateContext(dbName));
            IEnumerable<Indicator> result = checkResult.GetAll();
            Assert.AreEqual(19, result.Count());
            Assert.IsNull(checkResult.Get(toDelete.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void DeleteIndicatorNullTest() 
        {
            CreateData(20);

            IRepository<Indicator> deleteRepo = new IndicatorRepository(CreateContext(dbName));
            deleteRepo.Remove(null);
        }

        [TestMethod]
        public void ModifyIndicatorOkTest() 
        {
            Indicator data = CreateData(1).Single();

            IRepository<Indicator> modifyRepo = new IndicatorRepository(CreateContext(dbName));
            Indicator toUpdate = modifyRepo.Get(data.Id);
            Indicator newValues = new Indicator{ Name = "Name Changed" };

            toUpdate.Update(newValues);
            
            modifyRepo.Update(toUpdate);
            modifyRepo.Save();

            IRepository<Indicator> checkResult = new IndicatorRepository(CreateContext(dbName));
            Indicator result = checkResult.Get(toUpdate.Id);
            Assert.AreEqual("Name Changed", result.Name);
            Assert.AreEqual("Test Area", result.Area.Name);
            Assert.AreEqual(3, result.IndicatorItems.Count);
            Assert.IsTrue(result.IndicatorItems.Any(ii => ii.Name == "Red"));
            Assert.IsTrue(result.IndicatorItems.Any(ii => ii.Name == "Yellow"));
            Assert.IsTrue(result.IndicatorItems.Any(ii => ii.Name == "Green"));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void ModifyIndicatorNotExistTest() 
        {
            CreateData(20);

            IRepository<Indicator> modifyRepo = new IndicatorRepository(CreateContext(dbName));
            Indicator toUpdate = new Indicator{ Id = Guid.NewGuid(), Name = "Name Changed" };
            
            modifyRepo.Update(toUpdate);
            modifyRepo.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ModifyIndicatorNullTest() 
        {
            IRepository<Indicator> modifyRepo = new IndicatorRepository(CreateContext(dbName));
            modifyRepo.Update(null);
        }

        [TestMethod]
        public void GetManagerIndicatorsOkTest() 
        {
            IRepository<Indicator> setup = new IndicatorRepository(CreateContext(dbName));
            User user = new User
            { 
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager
            };

            Area area1 = new Area
            {
                Name = "Test Area 1",
                DataSource = "Connection String",
                UserAreas = new List<UserArea> { new UserArea{ User = user} }
            };

            Indicator indicator11 = new Indicator
            {
                Area = area1,
                Name = "Test Indicator 11",
                UserIndicators = new List<UserIndicator> { new UserIndicator { User = user, IsVisible = true, Position = 2 } }
            };

            Indicator indicator12 = new Indicator
            {
                Area = area1,
                Name = "Test Indicator 12",
                UserIndicators = new List<UserIndicator> { new UserIndicator { User = user, IsVisible = false, Position = 1 } }
            };

            Indicator indicator13 = new Indicator
            {
                Area = area1,
                Name = "Test Indicator 13"
            };

            Area area2 = new Area
            {
                Name = "Test Area 2",
                DataSource = "Connection String"
            };

            Indicator indicator21 = new Indicator
            {
                Area = area2,
                Name = "Test Indicator 21"
            };

            Indicator indicator22 = new Indicator
            {
                Area = area2,
                Name = "Test Indicator 22"
            };

            setup.Add(indicator11);
            setup.Add(indicator12);
            setup.Add(indicator13);
            setup.Add(indicator21);
            setup.Add(indicator22);
            setup.Save();

            IndicatorRepository repository = new IndicatorRepository(CreateContext(dbName));
            IEnumerable<Indicator> result = repository.GetManagerIndicators(user.Id);
            Assert.AreEqual(3, result.Count());
            UserIndicator firstIndicator = result.Single(i => i.Name == "Test Indicator 11").UserIndicators.Single();
            Assert.IsTrue(firstIndicator.IsVisible);
            Assert.AreEqual(2, firstIndicator.Position);
            UserIndicator secondIndicator = result.Single(i => i.Name == "Test Indicator 12").UserIndicators.Single();
            Assert.IsFalse(secondIndicator.IsVisible);
            Assert.AreEqual(1, secondIndicator.Position);
            Indicator thirdIndicator = result.Single(i => i.Name == "Test Indicator 13");
            Assert.AreEqual(0, thirdIndicator.UserIndicators.Count);
        }

        [TestMethod]
        public void GetManagerIndicatorsUserNotExistTest() 
        {
            IRepository<Indicator> setup = new IndicatorRepository(CreateContext(dbName));
            User user = new User
            { 
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager
            };

            Area area1 = new Area
            {
                Name = "Test Area 1",
                DataSource = "Connection String",
                UserAreas = new List<UserArea> { new UserArea{ User = user} }
            };

            Indicator indicator11 = new Indicator
            {
                Area = area1,
                Name = "Test Indicator 11",
                UserIndicators = new List<UserIndicator> { new UserIndicator { User = user, IsVisible = true, Position = 2 } }
            };

            Indicator indicator12 = new Indicator
            {
                Area = area1,
                Name = "Test Indicator 12",
                UserIndicators = new List<UserIndicator> { new UserIndicator { User = user, IsVisible = false, Position = 1 } }
            };

            Area area2 = new Area
            {
                Name = "Test Area 2",
                DataSource = "Connection String"
            };

            Indicator indicator21 = new Indicator
            {
                Area = area2,
                Name = "Test Indicator 21"
            };

            Indicator indicator22 = new Indicator
            {
                Area = area2,
                Name = "Test Indicator 22"
            };

            setup.Add(indicator11);
            setup.Add(indicator12);
            setup.Add(indicator21);
            setup.Add(indicator22);
            setup.Save();

            IndicatorRepository repository = new IndicatorRepository(CreateContext(dbName));
            IEnumerable<Indicator> result = repository.GetManagerIndicators(Guid.NewGuid());
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetManagerIndicatorsUserWithNoAreasTest() 
        {
            IRepository<User> setupUser = new UserRepository(CreateContext(dbName));
            User user = new User
            { 
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager
            };

            setupUser.Add(user);
            setupUser.Save();

            IRepository<Indicator> setupArea = new IndicatorRepository(CreateContext(dbName));
            Area area1 = new Area
            {
                Name = "Test Area 1",
                DataSource = "Connection String"
            };

            Indicator indicator11 = new Indicator
            {
                Area = area1,
                Name = "Test Indicator 11"
            };

            Indicator indicator12 = new Indicator
            {
                Area = area1,
                Name = "Test Indicator 12"
            };

            Area area2 = new Area
            {
                Name = "Test Area 2",
                DataSource = "Connection String"
            };

            Indicator indicator21 = new Indicator
            {
                Area = area2,
                Name = "Test Indicator 21"
            };

            Indicator indicator22 = new Indicator
            {
                Area = area2,
                Name = "Test Indicator 22"
            };

            setupArea.Add(indicator11);
            setupArea.Add(indicator12);
            setupArea.Add(indicator21);
            setupArea.Add(indicator22);
            setupArea.Save();

            IndicatorRepository repository = new IndicatorRepository(CreateContext(dbName));
            IEnumerable<Indicator> result = repository.GetManagerIndicators(user.Id);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetMostHiddenIndicatorsTest() 
        {
            CreateIndicatorsWithUserIndicators();

            IRepository<Indicator> repository = new IndicatorRepository(CreateContext(dbName));
            IIndicatorQuery query = new IndicatorRepository(CreateContext(dbName));

            IEnumerable<Indicator> users = query.GetMostHiddenIndicators(2);

            Assert.AreEqual(users.Count(), 2);
        }

        private IEnumerable<Indicator> CreateData(int amount)
        {
            IRepository<Indicator> repository = new IndicatorRepository(CreateContext(dbName));
            List<string> itemNames = new List<string> { "Red", "Yellow", "Green" };
            List<Indicator> result = new List<Indicator>();
            for(int i = 0; i < amount; i++)
            {
                Indicator indicator = new Indicator{ Name = "Test Indicator " + i, Area = testArea };
                indicator.UserIndicators.Add(new UserIndicator { User = userTest });
                
                foreach (string color in itemNames)
                {
                    ItemNumeric numeric = new ItemNumeric{ Position = 1, NumberValue = 5 };
                    ItemQuery query1 = new ItemQuery{ Position = 2, QueryTextValue = "SELECT MAX x FROM TABLE" };
                    Condition condition1 = new EqualsCondition{ Position = 1, Components = new List<Component> { numeric, query1 } };
                    
                    ItemQuery query2 = new ItemQuery{ Position = 1, QueryTextValue = "SELECT MIN x FROM TABLE" };
                    ItemText text = new ItemText{ Position = 2, TextValue = "Test Texto" };
                    Condition condition2 = new MinorCondition{ Position = 2, Components = new List<Component> { text, query2 } };

                    Condition condition3 = new AndCondition { Components = new List<Component> { condition1,  condition2 } };
                    indicator.IndicatorItems.Add(new IndicatorItem { Name = color, Condition = condition3 });
                }
                repository.Add(indicator);
                result.Add(indicator);
            }
            repository.Save();
            return result;
        }

        private IEnumerable<Indicator> CreateIndicatorsWithUserIndicators()
        {
            IRepository<Indicator> repository = new IndicatorRepository(CreateContext(dbName));
            List<Indicator> result = new List<Indicator>();           

            Indicator indicator1 = new Indicator{ Name = "Test Indicator 1", Area = testArea };
            indicator1.UserIndicators.Add(new UserIndicator { User = CreateUser(1), IsVisible =  true });    
            indicator1.UserIndicators.Add(new UserIndicator { User = CreateUser(2), IsVisible =  false });        
            indicator1.UserIndicators.Add(new UserIndicator { User = CreateUser(3), IsVisible =  true });               
            repository.Add(indicator1);
            result.Add(indicator1);

            Indicator indicator2 = new Indicator{ Name = "Test Indicator 2", Area = testArea };
            indicator2.UserIndicators.Add(new UserIndicator { User = CreateUser(4), IsVisible =  false }); 
            indicator2.UserIndicators.Add(new UserIndicator { User = CreateUser(5), IsVisible =  true });    
            indicator2.UserIndicators.Add(new UserIndicator { User = CreateUser(6), IsVisible =  false });              
            repository.Add(indicator2);
            result.Add(indicator2);

            Indicator indicator3 = new Indicator{ Name = "Test Indicator 2", Area = testArea };
            indicator3.UserIndicators.Add(new UserIndicator { User = CreateUser(7), IsVisible =  false });    
            indicator3.UserIndicators.Add(new UserIndicator { User = CreateUser(8), IsVisible =  false });     
            indicator3.UserIndicators.Add(new UserIndicator { User = CreateUser(9), IsVisible =  false });          
            repository.Add(indicator3);
            result.Add(indicator3);
            
            repository.Save();
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