using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IndicatorsManager.DataAccess;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.DataAccess.Test
{
    [TestClass]
    public class AreaRepositoryTest : BaseTest
    {
        [TestMethod]
        public void CreateAreaOKTest()
        {
            IRepository<Area> repository = new AreaRepository(Context);

            var id = Guid.NewGuid();

            Area areaTest = new Area
            {
                Id = id,
                Name = "Area Name",
                DataSource = "Area DataSource"
            };

            repository.Add(areaTest);
            repository.Save();

            var areas = repository.GetAll().ToList();

            Assert.AreEqual(areas[0].Id, id);
        }

        [TestMethod]
        [ExpectedException(typeof(IdExistException))]        
        public void CreateDuplicateAreaTest()
        {
            IRepository<Area> repository = new AreaRepository(Context);

            var id = Guid.NewGuid();

            Area areaTest1 = new Area
            {
                Id = id,
                Name = "Area Name",
                DataSource = "Area DataSource"
            };

            Area areaTest2 = new Area
            {
                Id = id,
                Name = "Area Name",
                DataSource = "Area DataSource"
            };

            repository.Add(areaTest1);
            repository.Add(areaTest2);
            repository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]        
        public void CreateNullAreaTest()
        {
            IRepository<Area> repository = new AreaRepository(Context);
            
            repository.Add(null);
            repository.Save();
        }

        [TestMethod]
        public void ModifyAreaOkTest()
        {
            IRepository<Area> repository = new AreaRepository(Context);
            Area area = CreateAreaData(repository, 10).ElementAt(5);
            
            Area update = new Area {
                Name = "Name Changed",
                DataSource = "DataSource Changed"
            };
            
            area.Update(update);
            repository.Update(area);
            repository.Save();

            Area result = repository.Get(area.Id);
            Assert.IsTrue(result.Name == update.Name && result.DataSource == update.DataSource);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void ModifyAreaNotExistTest()
        {
            IRepository<Area> repository = new AreaRepository(Context);
            CreateAreaData(repository, 10);

            Area update = new Area {
                Id = Guid.NewGuid(),
                Name = "Name Changed",
                DataSource = "DataSource Changed"
            };

            repository.Update(update);
            repository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ModifyAreaNullTest()
        {
            IRepository<Area> repository = new AreaRepository(Context);
            repository.Update(null);
        }

        [TestMethod]
        public void RemoveAreaOKTest()
        {
            IRepository<Area> repository = new AreaRepository(Context);
            Area removed = CreateAreaData(repository, 10).ElementAt(5);

            repository.Remove(removed);

            IEnumerable<Area> result = repository.GetAll();

            Assert.IsTrue(result.Contains(removed));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void RemoveAreaNotExistTest()
        {
            IRepository<Area> repository = new AreaRepository(Context);
            CreateAreaData(repository, 10);

            Area area = new Area {
                Id = Guid.NewGuid(),
                Name = "Name Area",
                DataSource = "DataSource Area"
            };
            
            repository.Remove(area);
            repository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullAreaTest()
        {
            IRepository<Area> repository = new AreaRepository(Context);
            repository.Remove(null);
        }

        [TestMethod]
        public void GetAllAreasOKTest()
        {
            IRepository<Area> repository = new AreaRepository(Context);

            int lengthRepo = 10;

            IEnumerable<Area> data = CreateAreaData(repository, lengthRepo);

            IEnumerable<Area> result = repository.GetAll();
            
            Assert.AreEqual(lengthRepo, result.Count());
            foreach(Area expected in data)
            {
                Assert.IsTrue(result.Any(
                    a => a.Id == expected.Id && 
                    a.Name == expected.Name &&
                    a.DataSource == expected.DataSource));
            }
        }

        [TestMethod]
        public void GetAreaOKTest()
        {
            IRepository<Area> repository = new AreaRepository(Context);
            Area expected = CreateAreaData(repository, 10).ElementAt(5);

            Area result = repository.Get(expected.Id);

            Assert.AreEqual(expected.Name, result.Name);
        }

        [TestMethod]
        public void GetAreaNotExistTest()
        {
            IRepository<Area> repository = new AreaRepository(Context);
            CreateAreaData(repository, 10);
            
            Area result = repository.Get(Guid.NewGuid());
            Assert.IsNull(result);
        }
        
        [TestMethod]
        public void GetAreaByNameOkTest()
        {
            AreaRepository repository = new AreaRepository(Context);
            Area expected = CreateAreaData(repository, 10).ElementAt(5);

            Area result = repository.GetByName(expected.Name);
            Assert.AreEqual(expected.Id, result.Id); 
        }

        [TestMethod]
        public void GetAreaByNameNotExistTest()
        {
            AreaRepository repository = new AreaRepository(Context);
            CreateAreaData(repository, 10);

            Area result = repository.GetByName("Name not exists");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddIndicatorToAreaOkTest()
        {
            IRepository<Area> initArea = new AreaRepository(CreateContext(dbName));
            Area create = new Area{ Name = "Test Add Indicator", DataSource = "Data Source" };
            initArea.Add(create);
            initArea.Save();

            // Add Indicator to Area
            IRepository<Area> addIndicator = new AreaRepository(CreateContext(dbName));
            Area created = addIndicator.Get(create.Id);
            created.Indicators.Add(new Indicator{ Name = "Indicator Test"});
            addIndicator.Save();


            IRepository<Area> checkResult = new AreaRepository(CreateContext(dbName));
            Assert.AreEqual(1, checkResult.GetAll().Count());

            Area result = checkResult.Get(create.Id);
            Assert.AreEqual(1, result.Indicators.Count());
            Assert.AreEqual("Indicator Test", result.Indicators.Single().Name);
            Assert.AreEqual(create.Id, result.Indicators.Single().Area.Id);
        }

        private IEnumerable<Area> CreateAreaData(IRepository<Area> repository, int amount)
        {
            List<Area> result = new List<Area>();
            for(int i = 0; i < amount; i++)
            {
                Area area = new Area
                {
                    Id = Guid.NewGuid(),
                    Name = "Name " + i,
                    DataSource = "DataSource " + i
                };
                repository.Add(area);
                result.Add(area);
            }
            repository.Save();
            return result;
        }

    }

    
}