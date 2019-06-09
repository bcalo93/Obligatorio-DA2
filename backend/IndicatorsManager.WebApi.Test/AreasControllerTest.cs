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
    public class AreasControllerTest
    {
        private Mock<ILogic<Area>> mockArea;
        private Mock<IIndicatorLogic> mockIndicator;
        private Mock<IUserAreaLogic> mockUserAreaLogic;
        private AreasController controller;

        [TestInitialize]
        public void InitMocks()
        {
            mockArea = new Mock<ILogic<Area>>(MockBehavior.Strict);
            mockIndicator = new Mock<IIndicatorLogic>(MockBehavior.Strict);
            mockUserAreaLogic = new Mock<IUserAreaLogic>(MockBehavior.Strict);
            controller = new AreasController(mockArea.Object, mockUserAreaLogic.Object, mockIndicator.Object);
        }

        [TestCleanup]
        public void VerifyAll()
        {
            mockArea.VerifyAll();
            mockIndicator.VerifyAll();
            mockUserAreaLogic.VerifyAll();
        }

        [TestMethod]
        public void PostOkTest()
        {
            AreaModel requestBody = new AreaModel
            {
                Name = "Name Post",
                DataSource = "DataSource Post"
            };
            
            Area createResult = new Area
            {
                Id = Guid.NewGuid(), 
                Name = "Name Post",
                DataSource = "DataSource Post"
            };
            mockArea.Setup(m => m.Create(It.IsAny<Area>())).Returns(createResult);

            var result = controller.Post(requestBody);
            
            var createResponse = result as CreatedAtRouteResult;
            AreaModel model = createResponse.Value as AreaModel;
            AssertAreasAreEqual(createResult, model);
        }

        [TestMethod]
        public void PostInvalidEntityExceptionTest()
        {
            AreaModel requestBody = new AreaModel
            {
                Name = "Name Post",
                DataSource = "DataSource Post"
            };
            mockArea.Setup(m => m.Create(It.IsAny<Area>())).Throws(new InvalidEntityException("Los datos del área no son válidos"));

            var result = controller.Post(requestBody);
            
            var response = result as BadRequestObjectResult;
            Assert.AreEqual("Los datos del área no son válidos", response.Value);
            
        }

        [TestMethod]
        public void PostEntityExistExceptionTest()
        {
            AreaModel requestBody = new AreaModel
            {
                Name = "Name Post",
                DataSource = "DataSource Post"
            };
            mockArea.Setup(m => m.Create(It.IsAny<Area>())).Throws(new EntityExistException("El nombre de área ya existe."));

            var result = controller.Post(requestBody);
            
            var response = result as ConflictObjectResult;
            Assert.AreEqual("El nombre de área ya existe.", response.Value);
        }

        [TestMethod]
        public void PostDataAccessExceptionTest()
        {
            AreaModel requestBody = new AreaModel
            {
                Name = "Name Post",
                DataSource = "DataSource Post"
            };            
            mockArea.Setup(m => m.Create(It.IsAny<Area>())).Throws(new DataAccessException(""));

            var result = controller.Post(requestBody);

            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
        }

        [TestMethod]
        public void GetAreaOkTest()
        {
            Guid expectedId = Guid.NewGuid();
            Area getResult = new Area
            {
                Id = expectedId,
                Name = "Name Get",
                DataSource = "DataSource Get"
            };
            mockArea.Setup(m => m.Get(expectedId)).Returns(getResult);

            var result = controller.Get(expectedId);

            var response = result as OkObjectResult;
            AreaModel model = response.Value as AreaModel;
            AssertAreasAreEqual(getResult, model);
        }

        [TestMethod]
        public void GetUsersFromAreaOkTest()
        {
            Guid expectedId = Guid.NewGuid();
            int length = 5;
            Area getResult = new Area
            {
                Id = expectedId,
                Name = "Name Get",
                DataSource = "DataSource Get",
                UserAreas = CreateUserAreas(expectedId, length)
            };

            mockArea.Setup(m => m.Get(expectedId)).Returns(getResult);
            
            var result = controller.Get(expectedId);

            var response = result as OkObjectResult;
            AreaModel model = response.Value as AreaModel;
            Assert.AreEqual(length, model.Users.Count);
        }

        [TestMethod]
        public void GetAreaNotFoundTest()
        {
            mockArea.Setup(m => m.Get(It.IsAny<Guid>())).Returns<IEnumerable<Area>>(null);

            var result = controller.Get(Guid.NewGuid());

            var response = result as NotFoundObjectResult;
            Assert.AreEqual("El área no existe.", response.Value);
        }

        [TestMethod]
        public void GetAreaDataAccessExceptionTest()
        {
            mockArea.Setup(m => m.Get(It.IsAny<Guid>())).Throws(new DataAccessException(""));

            var result = controller.Get(Guid.NewGuid());

            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
        }

        [TestMethod]
        public void GetAllAreaOkTest()
        {
            IEnumerable<Area> areas = CreateAreas(10);
            mockArea.Setup(m => m.GetAll()).Returns(areas);

            var result = controller.Get();

            var response = result as OkObjectResult;
            IEnumerable<AreaModel> models = response.Value as IEnumerable<AreaModel>;
            Assert.AreEqual(10, models.Count());
        }

        [TestMethod]
        public void GetAllAreaDataAccessExceptionTest()
        {
            mockArea.Setup(m => m.GetAll()).Throws(new DataAccessException(""));

            var result = controller.Get();

            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);     
        }

        [TestMethod]
        public void PutAreaOkTest()
        {
            Guid expectedId = Guid.NewGuid();
            AreaModel requestBody = new AreaModel
            {
                Name = "Name Put",
                DataSource = "DataSource Put"
            };

            Area updateResult = new Area
            {
                Id = expectedId,
                Name = "Name Put",
                DataSource = "DataSource Put"
            };
            mockArea.Setup(m => m.Update(expectedId, It.IsAny<Area>())).Returns(updateResult);

            var result = controller.Put(expectedId, requestBody);

            var response = result as OkObjectResult;
            AreaModel model = response.Value as AreaModel;
            AssertAreasAreEqual(updateResult, model);
        }

        [TestMethod]
        public void PutNullResultTest()
        {
            AreaModel requestBody = new AreaModel
            {
                Name = "Name Put",
                DataSource = "DataSource Put"
            };
            mockArea.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<Area>())).Returns<IEnumerable<Area>>(null);

            var result = controller.Put(Guid.NewGuid(), requestBody);
            
            var response = result as NotFoundObjectResult;
            Assert.AreEqual("El área no existe.", response.Value);
        }

        [TestMethod]
        public void PutInvalidEntityExceptionTest()
        {
            AreaModel requestBody = new AreaModel
            {
                Name = "Name Put",
                DataSource = "DataSource Put"
            };
            mockArea.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<Area>())).Throws(new InvalidEntityException("Los datos del área no son válidos"));

            var result = controller.Put(Guid.NewGuid(), requestBody);
            
            var response = result as BadRequestObjectResult;
            Assert.AreEqual("Los datos del área no son válidos", response.Value);
            
        }

        [TestMethod]
        public void PutEntityExistExceptionTest()
        {
            AreaModel requestBody = new AreaModel
            {
                Name = "Name Put",
                DataSource = "DataSource Put"
            };
            mockArea.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<Area>())).Throws(new EntityExistException("El nombre de área ya existe."));

            var result = controller.Put(Guid.NewGuid(), requestBody);
            
            var response = result as ConflictObjectResult;
            Assert.AreEqual("El nombre de área ya existe.", response.Value);
        }

        [TestMethod]
        public void PutDataAccessExceptionTest()
        {
            AreaModel requestBody = new AreaModel
            {
                Name = "Name Put",
                DataSource = "DataSource Put"
            };
            mockArea.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<Area>())).Throws(new DataAccessException(""));

            var result = controller.Put(Guid.NewGuid(), requestBody);

            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
        }        

        [TestMethod]
        public void DeleteOkTest()
        {
            mockArea.Setup(m => m.Remove(It.IsAny<Guid>()));

            var result = controller.Delete(Guid.NewGuid());

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteDataAccessExceptionTest()
        {
            mockArea.Setup(m => m.Remove(It.IsAny<Guid>())).Throws(new DataAccessException(""));

            var result = controller.Delete(Guid.NewGuid());

            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
        }

        [TestMethod]
        public void PostUserAreaOkTest()
        {
            Guid areaId = Guid.NewGuid();
            Guid body = Guid.NewGuid();
            Area area = new Area() { Id = areaId };

            mockUserAreaLogic.Setup(m => m.AddAreaManager(It.IsAny<Guid>(), It.IsAny<Guid>()));
            mockArea.Setup(m => m.Get(areaId)).Returns(area);

            var result = controller.Post(areaId, body);
            
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }  

        [TestMethod]
        public void PostUserAreaInvalidEntityTest()
        {
            Guid areaId = Guid.NewGuid();
            Guid body = Guid.NewGuid();

            string msj = "El usuario no existe";

            mockUserAreaLogic.Setup(m => m.AddAreaManager(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new InvalidEntityException(msj));

            var result = controller.Post(areaId, body);
            
            var response = result as BadRequestObjectResult;
            Assert.AreEqual(msj, response.Value);
        }  
        
        [TestMethod]
        public void PostUserAreaDataAccessExceptionTest()
        {
            Guid areaId = Guid.NewGuid();
            Guid body = Guid.NewGuid();

            mockUserAreaLogic.Setup(m => m.AddAreaManager(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new DataAccessException(""));

            var result = controller.Post(areaId, body);
            
            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
        }  

        [TestMethod]
        public void DeleteUserAreaOkTest()
        {
            mockUserAreaLogic.Setup(m => m.RemoveAreaManager(It.IsAny<Guid>(), It.IsAny<Guid>()));
            
            var result = controller.Delete(Guid.NewGuid(), Guid.NewGuid());

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteUserAreaInvalidEntityTest()
        {
            string msj = "El usuario no existe";

            mockUserAreaLogic.Setup(m => m.RemoveAreaManager(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new InvalidEntityException(msj));

            var result = controller.Delete(Guid.NewGuid(), Guid.NewGuid());

            var response = result as BadRequestObjectResult;
            Assert.AreEqual(msj, response.Value);
        }

        [TestMethod]
        public void DeleteUserAreaDataAccessExceptionTest()
        {
            mockUserAreaLogic.Setup(m => m.RemoveAreaManager(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new DataAccessException(""));

            var result = controller.Delete(Guid.NewGuid(), Guid.NewGuid());

            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
        }
        
        [TestMethod]
        public void AddIndicatorOkTest_1()
        {
            Guid areaId = Guid.NewGuid();

            Indicator createResult = new Indicator
            {
                Id = Guid.NewGuid(),
                Name = "Test Indicator",
                IndicatorItems = new List<IndicatorItem>
                { 
                    new IndicatorItem
                    {
                        Id = Guid.NewGuid(),
                        Name = "Red",
                        Condition = new MayorCondition
                        { 
                            Id = Guid.NewGuid(),
                            Position = 1,
                            Components = new List<Component> 
                            { 
                                new ItemQuery{ Id = Guid.NewGuid(), Position = 1, QueryTextValue ="Test" },
                                new ItemNumeric { Id = Guid.NewGuid(), Position = 2, NumberValue = 20 }
                            }
                        }
                    } 
                }
            };
            
            mockIndicator.Setup(m => m.Create(areaId, It.IsAny<Indicator>())).Returns(createResult);

            // Request Body
            IndicatorCreateModel requestBody = new IndicatorCreateModel 
            { 
                Name = "Test Indicator", 
                Items = new List<IndicatorItemPersistModel>
                {
                    new IndicatorItemPersistModel 
                    { 
                        Name = "Red", 
                        Condition = new ConditionModel 
                        {
                            Position = 1,
                            ConditionType = ConditionType.Mayor,
                            Components = new List<ComponentModel>
                            {
                                new StringItemModel { Position = 1, Value = "Test" , Type = StringType.Sql },
                                new IntItemModel { Position = 2, Value = 20 }
                            }
                        }
                    }
                }
            };

            var result = controller.AddIndicator(areaId, requestBody);
            var createResponse = result as OkObjectResult;
            IndicatorGetModel model = createResponse.Value as IndicatorGetModel;
            Assert.AreEqual(createResult.Id, model.Id);
            Assert.AreEqual("Test Indicator", model.Name);
            IndicatorItemGetModel item = model.Items.Single();
            Assert.AreNotEqual(Guid.Empty, item.Id);
            Assert.AreEqual("Red", item.Name);
            ConditionModel condition = item.Condition as ConditionModel;
            Assert.AreEqual(1, condition.Position);
            Assert.AreEqual(ConditionType.Mayor, condition.ConditionType);
            StringItemModel query = condition.Components.Single(c => c.Position == 1) as StringItemModel;
            Assert.AreEqual("Test", query.Value);
            Assert.AreEqual(StringType.Sql, query.Type);
            IntItemModel number = condition.Components.Single(c => c.Position == 2) as IntItemModel;
            Assert.AreEqual(20, number.Value);
        }

        [TestMethod]
        public void AddIndicatorOkTest_2()
        {
            Guid areaId = Guid.NewGuid();
            DateTime expectedDate = new DateTime(2019, 2, 23);
            Indicator createResult = new Indicator
            {
                Id = Guid.NewGuid(),
                Name = "Test Indicator",
                IndicatorItems = new List<IndicatorItem>
                { 
                    new IndicatorItem
                    {
                        Id = Guid.NewGuid(),
                        Name = "Red",
                        Condition = new MayorCondition
                        { 
                            Id = Guid.NewGuid(),
                            Position = 1,
                            Components = new List<Component> 
                            { 
                                new ItemBoolean{ Id = Guid.NewGuid(), Position = 1, Boolean = true },
                                new ItemDate { Id = Guid.NewGuid(), Position = 2, Date = expectedDate }
                            }
                        }
                    } 
                }
            };
            
            mockIndicator.Setup(m => m.Create(areaId, It.IsAny<Indicator>())).Returns(createResult);

            // Request Body
            IndicatorCreateModel requestBody = new IndicatorCreateModel 
            { 
                Name = "Test Indicator", 
                Items = new List<IndicatorItemPersistModel>
                {
                    new IndicatorItemPersistModel 
                    { 
                        Name = "Red", 
                        Condition = new ConditionModel 
                        {
                            Position = 1,
                            ConditionType = ConditionType.Mayor,
                            Components = new List<ComponentModel>
                            {
                                new BooleanItemModel { Position = 1, BooleanValue = true },
                                new DateItemModel { Position = 2, DateValue = expectedDate }
                            }
                        }
                    }
                }
            };

            var result = controller.AddIndicator(areaId, requestBody);
            var createResponse = result as OkObjectResult;
            IndicatorGetModel model = createResponse.Value as IndicatorGetModel;
            Assert.AreEqual(createResult.Id, model.Id);
            Assert.AreEqual("Test Indicator", model.Name);
            IndicatorItemGetModel item = model.Items.Single();
            Assert.AreNotEqual(Guid.Empty, item.Id);
            Assert.AreEqual("Red", item.Name);
            ConditionModel condition = item.Condition as ConditionModel;
            Assert.AreEqual(1, condition.Position);
            Assert.AreEqual(ConditionType.Mayor, condition.ConditionType);
            BooleanItemModel boolean = condition.Components.Single(c => c.Position == 1) as BooleanItemModel;
            Assert.IsTrue( boolean.BooleanValue);
            DateItemModel date = condition.Components.Single(c => c.Position == 2) as DateItemModel;
            Assert.AreEqual(expectedDate, date.DateValue);
        }

        [TestMethod]
        public void AddIndicatorInvalidEntityExceptionTest()
        {
            Guid areaId = Guid.NewGuid();
            
            mockIndicator.Setup(m => m.Create(areaId, It.IsAny<Indicator>())).Throws(new InvalidEntityException("El Indicador o alguna de las parte ses invalido."));

            // Request Body
            IndicatorCreateModel requestBody = new IndicatorCreateModel { Name = "" };

            var result = controller.AddIndicator(areaId, requestBody);
            var createResponse = result as BadRequestObjectResult;
            Assert.AreEqual("El Indicador o alguna de las parte ses invalido.", createResponse.Value);
        }

        [TestMethod]
        public void AddIndicatorEntityNotExistExceptionTest()
        {
            Guid areaId = Guid.NewGuid();
            
            mockIndicator.Setup(m => m.Create(areaId, It.IsAny<Indicator>())).Throws(new EntityNotExistException("El Area es invalida."));

            // Request Body
            IndicatorCreateModel requestBody = new IndicatorCreateModel { Name = "Test Indicator" };

            var result = controller.AddIndicator(areaId, requestBody);
            var createResponse = result as NotFoundObjectResult;
            Assert.AreEqual("El Area es invalida.", createResponse.Value);
        }

        [TestMethod]
        public void AddIndicatorDataAccessExceptionTest()
        {
            Guid areaId = Guid.NewGuid();
            
            mockIndicator.Setup(m => m.Create(areaId, It.IsAny<Indicator>())).Throws(new DataAccessException(""));

            // Request Body
            IndicatorCreateModel requestBody = new IndicatorCreateModel { Name = "Test Indicator" };

            var result = controller.AddIndicator(areaId, requestBody);
            var response = result as ObjectResult;
            Assert.AreEqual(503, response.StatusCode);
        }

        private void AssertAreasAreEqual(Area area, AreaModel model)
        {
            bool areEquals = (area.Id == model.Id) && (area.Name == model.Name) && (area.DataSource == model.DataSource);
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

        private List<UserArea> CreateUserAreas(Guid areaId, int amount)
        {
            List<UserArea> result = new List<UserArea>();

            Area area = new Area() { Id = areaId };
            
            for(int i = 0; i < amount; i++)
            {
                User newUser = new User
                {
                    Name = "Test Name" + i,
                    LastName = "Test LastName" + i,
                    Username = "Username Test" + i,
                    Password = "Password Test" + i,
                    Email = "test@email.com" + i,
                    Role = Role.Manager
                };

                result.Add(new UserArea(newUser, area));
            }  

            return result;
        }

    }
}