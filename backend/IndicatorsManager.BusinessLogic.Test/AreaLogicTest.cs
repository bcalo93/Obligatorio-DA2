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
    public class AreaLogicTest 
    {

        [TestMethod]
        public void CreateAreaOkTest()
        {
             Area create = new Area
            {
                Name = "Area Name",
                DataSource = "Area DataSource"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByName(It.IsAny<string>())).Returns<IEnumerable<Area>>(null);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);

            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Add(It.IsAny<Area>()));
            mockRepo.Setup(m => m.Save());
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Create(create);

            mockRepo.VerifyAll();
            AssertAreasAreEqual(create, result);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateAreaNullTest()
        {
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Create(null);

            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(EntityExistException))]
        public void CreateAreaWithExistingIdTest()
        {
             Area area = new Area
            {
                Name = "Test Name",
                DataSource = "Test DataSource"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByName(It.IsAny<string>())).Returns<IEnumerable<Area>>(null);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            mockRepo.Setup(m => m.Add(It.IsAny<Area>())).Throws(new IdExistException(""));

            Area result = logic.Create(area);

            mockRepo.VerifyAll();
        } 

        [TestMethod]
        [ExpectedException(typeof(EntityExistException))]
        public void CreateAreaWithExistingNameTest()
        {
             Area area = new Area
            {
                Name = "Test Name",
                DataSource = "Test DataSource"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByName(It.IsAny<string>())).Returns(new Area{ Id = Guid.NewGuid() });

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);

            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            
            Area result = logic.Create(area);

            mockRepo.VerifyAll();
        }

	    [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateAreaNullPropertyTest()
        {
             Area create = new Area
            {
                DataSource = "Test DataSource"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Create(create);

            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void CreateAreaEmptyStringPropertyTest()
        {
             Area create = new Area
            {
                Name = "Test Name",
                DataSource = " "
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Create(create);

            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void CreateAreaDataAccessExceptionlTest()
        {
            Area create = new Area
            {
                Name = "Area Name",
                DataSource = "Area DataSource"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByName(It.IsAny<string>())).Returns<IEnumerable<Area>>(null);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Add(It.IsAny<Area>()));
            mockRepo.Setup(m => m.Save()).Throws(new DataAccessException(""));

            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Create(create);

            mockRepo.VerifyAll();
        }


        [TestMethod]
        public void GetAreaOkTest()
        {
            Guid expectedId = Guid.NewGuid();
            Area getResult = new Area
            {
                Id = expectedId,
                Name = "Area Name",
                DataSource = "Area DataSource"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Get(expectedId);

            mockRepo.VerifyAll();
            AssertAreasAreEqual(getResult, result);
        }

        [TestMethod]
        public void GetAreaNotExistTest()
        {
            Guid expectedId = Guid.NewGuid();

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns<IEnumerable<Area>>(null);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Get(expectedId);

            mockRepo.VerifyAll();
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void GetAreaDataAccessExceptionTest()
        {
            Guid expectedId = Guid.NewGuid();

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Throws(new DataAccessException(""));
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Get(expectedId);

            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void GetAllAreaOkTest()
        {
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.GetAll()).Returns(CreateAreas(10));
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            IEnumerable<Area> result = logic.GetAll();

            mockRepo.VerifyAll();
            Assert.AreEqual(10, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void GetAllDataAccessExceptionTest()
        {
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.GetAll()).Throws(new DataAccessException(""));
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            IEnumerable<Area> result = logic.GetAll();

            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void RemoveAreaOkTest()
        {
            Guid expectedId = Guid.NewGuid();
            Area getResult = new Area
            {
                Id = expectedId,
                Name = "Area Name",
                DataSource = "Area DataSource"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            mockRepo.Setup(m => m.Remove(getResult));
            mockRepo.Setup(m => m.Save());
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            logic.Remove(expectedId);

            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void RemoveAreaNotExistTest()
        {
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(It.IsAny<Guid>())).Returns<IEnumerable<Area>>(null);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            logic.Remove(Guid.NewGuid());

            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void RemoveAreaDataAccessExceptionTest()
        {
            Guid expectedId = Guid.NewGuid();
            Area getResult = new Area
            {
                Id = expectedId,
                Name = "Test Name",
                DataSource = "Test DataSource"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            mockRepo.Setup(m => m.Remove(getResult));
            mockRepo.Setup(m => m.Save()).Throws(new DataAccessException(""));
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            logic.Remove(expectedId);

            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void UpdateAreaOkTest()
        {
            Guid expectedId = Guid.NewGuid();
            Area getResult = new Area
            {
                Id = expectedId,
                Name = "Test Name",
                DataSource = "Test DataSource"
            };
             
            Area updateArea = new Area
            {
                Name = "Test Name Changed",
                DataSource = "Test DataSource Changed"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByName(It.IsAny<string>())).Returns<IEnumerable<Area>>(null);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            mockRepo.Setup(m => m.Update(It.IsAny<Area>()));
            mockRepo.Setup(m => m.Save());
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            
            Area result = logic.Update(expectedId, updateArea);

            mockRepo.VerifyAll();
            Assert.AreEqual(expectedId, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void UpdateAreaNullTest()
        {
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            
            Area result = logic.Update(Guid.NewGuid(), null);

            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void UpdateAreaIdNotFoundTest()
        {
            Guid expectedId = Guid.NewGuid();
            Area update = new Area
            {
                Name = "Test Name",
                DataSource = "Test DataSource"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByName(It.IsAny<string>())).Returns<IEnumerable<Area>>(null);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns<IEnumerable<Area>>(null);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Update(expectedId, update);

            mockRepo.VerifyAll();;
        }        

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void UpdateAreaNullPropertyTest()
        {
            Guid expectedId = Guid.NewGuid();
            Area update = new Area
            {
                DataSource = "Test DataSource"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Update(expectedId, update);

            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void UpdateAreaEmptyStringPropertyTest()
        {
            Guid expectedId = Guid.NewGuid();
            Area update = new Area
            {
                Name = "Test Name",
                DataSource = ""
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Update(expectedId, update);

            mockRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(EntityExistException))]
        public void UpdateAreaNameAlreadyExistTest()
        {
            Guid expectedId = Guid.NewGuid();
	        string duplicateName = "Duplicate Name";
            Area getResult = new Area
            {
                Id = expectedId,
                Name = "Test Name",
                DataSource = "Test DataSource"
            };

            Area update = new Area
            {
                Name = duplicateName,
                DataSource = "Test DataSource Changed"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByName(It.IsAny<string>())).Returns(new Area{ Id = Guid.NewGuid() });

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);

            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Update(expectedId, update);

            mockRepo.VerifyAll();
        }

        [TestMethod]
        public void UpdateAreaDataSourceOnlyTest()
        {
            Guid expectedId = Guid.NewGuid();
	        string name = "Test Name";
            Area getResult = new Area
            {
                Id = expectedId,
                Name = name,
                DataSource = "Test DataSource"
            };

            Area update = new Area
            {
                Name = name,
                DataSource = "Test DataSource Changed"
            };

            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);
            mockQuery.Setup(m => m.GetByName(It.IsAny<string>())).Returns(getResult);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);

            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(expectedId)).Returns(getResult);
            mockRepo.Setup(m => m.Update(It.IsAny<Area>()));
            mockRepo.Setup(m => m.Save());
            
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            Area result = logic.Update(expectedId, update);

            mockRepo.VerifyAll();
            Assert.AreEqual(getResult.Id, result.Id);
            Assert.AreEqual(getResult.Name, result.Name);
            Assert.AreEqual("Test DataSource Changed", result.DataSource);
        }

        [TestMethod]
        public void AddAreaManagerOkTest()
        {
            Guid areaId = Guid.NewGuid();
            Area area = new Area
            {
                Id = areaId,
                Name = "Test Name",
                DataSource = "Test DataSource"
            };

            Guid userId = Guid.NewGuid();
            User user = new User
            {
                Id = userId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager
            };
            
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserQuery = new Mock<IUserQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockUserRepo.Setup(m => m.Get(userId)).Returns(user);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(areaId)).Returns(area);
            mockRepo.Setup(m => m.Save());
            
            IUserAreaLogic uaLogic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            ILogic<Area> aLogic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            ILogic<User> uLogic = new UserLogic(mockUserRepo.Object, mockUserQuery.Object);
            
            uaLogic.AddAreaManager(areaId, userId);

            Area getArea = aLogic.Get(areaId);
            User getUser = uLogic.Get(userId);

            mockRepo.VerifyAll();
            mockUserRepo.VerifyAll();

            Assert.AreEqual(getArea.UserAreas.Count, 1);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void AddAreaManagerUserNotExistsTest()
        {
            Guid areaId = Guid.NewGuid();
            Area area = new Area
            {
                Id = areaId,
                Name = "Test Name",
                DataSource = "Test DataSource"
            };

            Guid userId = Guid.NewGuid();
            
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockUserRepo.Setup(m => m.Get(userId)).Returns<IEnumerable<User>>(null);
            mockUserRepo.Setup(m => m.Update(It.IsAny<User>()));
            mockUserRepo.Setup(m => m.Save());
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(areaId)).Returns(area);
            mockRepo.Setup(m => m.Update(It.IsAny<Area>()));
            mockRepo.Setup(m => m.Save());
            
            IUserAreaLogic uaLogic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            
            uaLogic.AddAreaManager(areaId, userId);

            Area result = logic.Get(areaId);

            mockRepo.VerifyAll();
            mockUserRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void AddAreaManagerUserDeletedTest()
        {
            Guid areaId = Guid.NewGuid();
            Area area = new Area
            {
                Id = areaId,
                Name = "Test Name",
                DataSource = "Test DataSource"
            };

            Guid userId = Guid.NewGuid();
            User user = new User
            {
                Id = userId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                IsDeleted = true,
                Role = Role.Manager
            };
            
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockUserRepo.Setup(m => m.Get(userId)).Returns(user);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(areaId)).Returns(area);
            mockRepo.Setup(m => m.Update(It.IsAny<Area>()));
            mockRepo.Setup(m => m.Save());
            
            IUserAreaLogic uaLogic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            
            uaLogic.AddAreaManager(areaId, userId);

            Area result = logic.Get(areaId);

            mockRepo.VerifyAll();
            mockUserRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void AddAreaManagerAreaNotExistTest()
        {
            Guid areaId = Guid.NewGuid();
            
            Guid userId = Guid.NewGuid();
            User user = new User
            {
                Id = userId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager
            };
            
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockUserRepo.Setup(m => m.Get(userId)).Returns(user);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(areaId)).Returns<IEnumerable<Area>>(null);
            
            IUserAreaLogic uaLogic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            
            uaLogic.AddAreaManager(areaId, userId);

            Area result = logic.Get(areaId);

            mockRepo.VerifyAll();
            mockUserRepo.VerifyAll();
        }

        [TestMethod]
        public void RemoveAreaManagerOkTest()
        {
            Guid areaId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            Area area = new Area
            {
                Id = areaId,
                Name = "Test Name",
                DataSource = "Test DataSource",
                UserAreas = CreateUserAreas(areaId, userId)
            };
            
            User user = new User
            {
                Id = userId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager,
                UserAreas = CreateUserAreas(areaId, userId)
            };

            int iniLength = area.UserAreas.Count;
            
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserQuery = new Mock<IUserQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockUserRepo.Setup(m => m.Get(userId)).Returns(user);
            mockUserRepo.Setup(m => m.Update(It.IsAny<User>()));
            mockUserRepo.Setup(m => m.Save());
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(areaId)).Returns(area);
            mockRepo.Setup(m => m.Update(It.IsAny<Area>()));
            mockRepo.Setup(m => m.Save());
            
            IUserAreaLogic uaLogic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            ILogic<Area> aLogic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            ILogic<User> uLogic = new UserLogic(mockUserRepo.Object, mockUserQuery.Object);

            uaLogic.RemoveAreaManager(areaId, userId);

            Area getArea = aLogic.Get(areaId);
            User getUser = uLogic.Get(userId);
            
            mockUserRepo.VerifyAll();
            mockRepo.VerifyAll();

            Assert.AreEqual(iniLength - 1, getArea.UserAreas.Count);
            Assert.AreEqual(iniLength - 1, getUser.UserAreas.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void RemoveAreaManagerUserNotExistsTest()
        {
            Guid areaId = Guid.NewGuid();
            Area area = new Area
            {
                Id = areaId,
                Name = "Test Name",
                DataSource = "Test DataSource"
            };

            Guid userId = Guid.NewGuid();
            
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockUserRepo.Setup(m => m.Get(userId)).Returns<IEnumerable<User>>(null);
            mockUserRepo.Setup(m => m.Update(It.IsAny<User>()));
            mockUserRepo.Setup(m => m.Save());
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(areaId)).Returns(area);
            mockRepo.Setup(m => m.Update(It.IsAny<Area>()));
            mockRepo.Setup(m => m.Save());
            
            IUserAreaLogic uaLogic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            
            uaLogic.RemoveAreaManager(areaId, userId);

            mockRepo.VerifyAll();
            mockUserRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void RemoveAreaManagerUserDeletedTest()
        {
            Guid areaId = Guid.NewGuid();
            Area area = new Area
            {
                Id = areaId,
                Name = "Test Name",
                DataSource = "Test DataSource"
            };

            Guid userId = Guid.NewGuid();
            User user = new User
            {
                Id = userId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                IsDeleted = true,
                Role = Role.Manager
            };
            
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockUserRepo.Setup(m => m.Get(userId)).Returns(user);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(areaId)).Returns(area);
            mockRepo.Setup(m => m.Update(It.IsAny<Area>()));
            mockRepo.Setup(m => m.Save());
            
            IUserAreaLogic uaLogic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            
            uaLogic.RemoveAreaManager(areaId, userId);

            mockRepo.VerifyAll();
            mockUserRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException))]
        public void RemoveAreaManagerAreaNotExistTest()
        {
            Guid areaId = Guid.NewGuid();
            
            Guid userId = Guid.NewGuid();
            User user = new User
            {
                Id = userId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager
            };
            
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockUserRepo.Setup(m => m.Get(userId)).Returns(user);
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(areaId)).Returns<IEnumerable<Area>>(null);
            
            IUserAreaLogic uaLogic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            ILogic<Area> logic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            
            uaLogic.RemoveAreaManager(areaId, userId);

            mockRepo.VerifyAll();
            mockUserRepo.VerifyAll();
        }

        [TestMethod]
        public void RemoveAreaManagerUserAreaNotExistTest()
        {
            Guid areaId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            Area area = new Area
            {
                Id = areaId,
                Name = "Test Name",
                DataSource = "Test DataSource",
                UserAreas = CreateUserAreas(areaId, Guid.NewGuid())
            };
            
            User user = new User
            {
                Id = userId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager,
                UserAreas = CreateUserAreas(areaId, Guid.NewGuid())
            };

            int iniLength = area.UserAreas.Count;
            
            var mockQuery = new Mock<IAreaQuery>(MockBehavior.Strict);

            var mockUserQuery = new Mock<IUserQuery>(MockBehavior.Strict);

            var mockUserRepo = new Mock<IRepository<User>>(MockBehavior.Strict);
            mockUserRepo.Setup(m => m.Get(userId)).Returns(user);
            mockUserRepo.Setup(m => m.Update(It.IsAny<User>()));
            mockUserRepo.Setup(m => m.Save());
            
            var mockRepo = new Mock<IRepository<Area>>(MockBehavior.Strict);
            mockRepo.Setup(m => m.Get(areaId)).Returns(area);
            mockRepo.Setup(m => m.Update(It.IsAny<Area>()));
            mockRepo.Setup(m => m.Save());
            
            IUserAreaLogic uaLogic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            ILogic<Area> aLogic = new AreaLogic(mockRepo.Object, mockUserRepo.Object, mockQuery.Object);
            ILogic<User> uLogic = new UserLogic(mockUserRepo.Object, mockUserQuery.Object);

            uaLogic.RemoveAreaManager(areaId, userId);

            Area getArea = aLogic.Get(areaId);
            User getUser = uLogic.Get(userId);
            
            mockUserRepo.VerifyAll();
            mockRepo.VerifyAll();

            Assert.AreEqual(iniLength, getArea.UserAreas.Count);
            Assert.AreEqual(iniLength, getUser.UserAreas.Count);
        }




        

        private void AssertAreasAreEqual(Area area1, Area area2)
        {
            bool areEquals = (area1.Id == area2.Id) && (area1.Name == area2.Name) && (area1.DataSource == area2.DataSource);
            Assert.IsTrue(areEquals);
        }

        private IEnumerable<Area> CreateAreas(int amount)
        {
            List<Area> result = new List<Area>();
            for(int i = 0; i < amount; i++)
            {
                result.Add(new Area 
                {
                    Id = Guid.NewGuid(),
                    Name = "AreaName" + i,
                    DataSource = "DataSourceName" + i
                });
            }
            return result;
        }

        private List<UserArea> CreateUserAreas(Guid areaId, Guid userId)
        {
            List<UserArea> result = new List<UserArea>();

            Area expectedArea = new Area 
            {
                Id = areaId,
                Name = "Test Name",
                DataSource = "Test DataSource"
            };
            
            for(int i = 0; i < 5; i++)
            {
                User newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Name" + i,
                    LastName = "Test LastName" + i,
                    Username = "Username Test" + i,
                    Password = "Password Test" + i,
                    Email = "test@email.com" + i,
                    Role = Role.Manager
                };

                result.Add(new UserArea(newUser, expectedArea));
            }            

            User expectedUser = new User
            {
                Id = userId,
                Name = "Test Name",
                LastName = "Test LastName",
                Username = "Username Test",
                Password = "Password Test",
                Email = "test@email.com",
                Role = Role.Manager
            };

            result.Add(new UserArea(expectedUser, expectedArea));

            return result;
        }

    }
}