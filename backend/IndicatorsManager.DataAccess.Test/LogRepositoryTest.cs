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
    public class LogRepositoryTest : BaseTest
    {
        [TestMethod]
        public void CreateLogOKTest()
        {
            IRepository<Log> repository = new LogRepository(Context);

            var id = Guid.NewGuid();

            Log log = new Log
            {
                Id = id,
                DateTime = DateTime.Now,
                User = CreateUser(Guid.NewGuid())
            };

            repository.Add(log);
            repository.Save();

            var logs = repository.GetAll().ToList();

            Assert.AreEqual(logs[0].Id, id);
        }
 
        [TestMethod]
        [ExpectedException(typeof(IdExistException))]        
        public void CreateDuplicateLogTest()
        {
            IRepository<Log> repository = new LogRepository(Context);

            var id = Guid.NewGuid();

            Log LogTest1 = new Log
            {
                Id = id,
                DateTime = DateTime.Now,
                User = CreateUser(Guid.NewGuid())
            };

            Log LogTest2 = new Log
            {
                Id = id,
                DateTime = DateTime.Now,
                User = CreateUser(Guid.NewGuid())
            };

            repository.Add(LogTest1);
            repository.Add(LogTest2);
            repository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]        
        public void CreateNullLogTest()
        {
            IRepository<Log> repository = new LogRepository(Context);
            
            repository.Add(null);
            repository.Save();
        }

        [TestMethod]
        public void ModifyLogOkTest()
        {
            IRepository<Log> repository = new LogRepository(Context);
            Log Log = CreateLogData(repository, 10).ElementAt(5);
            
            Log update = new Log {
                DateTime = DateTime.Now,
                User = CreateUser(Guid.NewGuid())
            };
            
            Log.Update(update);
            repository.Update(Log);
            repository.Save();

            Log result = repository.Get(Log.Id);
            Assert.IsTrue(result.DateTime == update.DateTime && result.User == update.User);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void ModifyLogNotExistTest()
        {
            IRepository<Log> repository = new LogRepository(Context);
            CreateLogData(repository, 10);

            Log update = new Log {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                User = CreateUser(Guid.NewGuid())
            };

            repository.Update(update);
            repository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ModifyLogNullTest()
        {
            IRepository<Log> repository = new LogRepository(Context);
            repository.Update(null);
        }

        [TestMethod]
        public void RemoveLogOKTest()
        {
            IRepository<Log> repository = new LogRepository(Context);
            Log removed = CreateLogData(repository, 10).ElementAt(5);

            repository.Remove(removed);

            IEnumerable<Log> result = repository.GetAll();

            Assert.IsTrue(result.Contains(removed));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void RemoveLogNotExistTest()
        {
            IRepository<Log> repository = new LogRepository(Context);
            CreateLogData(repository, 10);

            Log Log = new Log {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                User = CreateUser(Guid.NewGuid())
            };
            
            repository.Remove(Log);
            repository.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullLogTest()
        {
            IRepository<Log> repository = new LogRepository(Context);
            repository.Remove(null);
        }

        [TestMethod]
        public void GetAllLogsOKTest()
        {
            IRepository<Log> repository = new LogRepository(Context);

            int lengthRepo = 10;

            IEnumerable<Log> data = CreateLogData(repository, lengthRepo);

            IEnumerable<Log> result = repository.GetAll();
            
            Assert.AreEqual(lengthRepo, result.Count());
            foreach(Log expected in data)
            {
                Assert.IsTrue(result.Any(
                    a => a.Id == expected.Id && 
                    a.DateTime == expected.DateTime &&
                    a.User == expected.User));
            }
        }

        [TestMethod]
        public void GetLogOKTest()
        {
            IRepository<Log> repository = new LogRepository(Context);
            Log expected = CreateLogData(repository, 10).ElementAt(5);

            Log result = repository.Get(expected.Id);

            Assert.AreEqual(expected.DateTime, result.DateTime);
        }

        [TestMethod]
        public void GetLogNotExistTest()
        {
            IRepository<Log> repository = new LogRepository(Context);
            CreateLogData(repository, 10);
            
            Log result = repository.Get(Guid.NewGuid());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUsersMostLogsTest()
        {
            IRepository<Log> repository = new LogRepository(Context);
            ILogQuery query = new LogRepository(Context);

            CreateLogsPerUser(repository);

            IEnumerable<User> users = query.GetUsersMostLogs(2);

            Assert.AreEqual(users.Count(), 2);
        }




        private IEnumerable<Log> CreateLogData(IRepository<Log> repository, int amount)
        {
            List<Log> result = new List<Log>();
            for(int i = 0; i < amount; i++)
            {
                Log Log = new Log
                {
                    Id = Guid.NewGuid(),
                    DateTime = DateTime.Now,
                    User = CreateUser(Guid.NewGuid())
                };
                repository.Add(Log);
                result.Add(Log);
            }
            repository.Save();
            return result;
        }

        private void CreateLogsPerUser(IRepository<Log> repository)
        {
            User user1 = CreateUser(1);
            User user2 = CreateUser(2);
            User user3 = CreateUser(3);
            for(int i = 0; i < 3; i++)
            {
                Log log = new Log
                {
                    Id = Guid.NewGuid(),
                    DateTime = DateTime.Now,
                    User = user1
                };
                repository.Add(log);
            }
            Log log3 = new Log
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                User = user3
            };
            repository.Add(log3);
            for(int i = 0; i < 2; i++)
            {
                Log log = new Log
                {
                    Id = Guid.NewGuid(),
                    DateTime = DateTime.Now,
                    User = user2
                };
                repository.Add(log);
            }        
            repository.Save();
        }

        private User CreateUser(Guid id) 
        {
            User create = new User
            {
                Id = id,
                Name = "Name",
                LastName = "LastName",
                Username = "Username",
                Password = "Password",
                Email = "user@email.com",
                Role = Role.Manager,
                IsDeleted = false
            };
            return create;
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